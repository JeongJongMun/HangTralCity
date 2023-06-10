using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class FriendList
{
    public List<string> friends;
}

public class FriendResponseBody  // 이름 변경
{
    public string statusCode;
    public string body;
}


public class FriendManage : MonoBehaviour
{
    public Button addBtn; // 친구 추가 버튼
    public TMP_InputField inputField; // 친구 이름 입력창
    public GameObject content; // 동적으로 추가할 친구 프리팹의 부모 오브젝트
    public GameObject friend; // 동적으로 추가 할 친구 프리팹
    Button[] deleteButtons; // 삭제 버튼들
    private void Start()
    {
        addBtn.onClick.AddListener(AddFriend);
        LoadFriend();
        SetDeleteBtnListener();

    }
    void SetDeleteBtnListener()
    {
        // 이름으로 모든 삭제 버튼을 찾음
        deleteButtons = FindObjectsOfType<Button>().Where(button => button.name == "DeleteBtn").ToArray();

        // 찾은 버튼들에 대해 동일한 삭제 함수를 연결
        foreach (Button button in deleteButtons)
        {
            button.onClick.AddListener(() => DeleteFriend(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text, button.transform.parent.gameObject));
        }
    }

    // 친구 추가
    void AddFriend()
    {
        string friendName;
        if (inputField.text != null)
        {
            friendName = inputField.text;
            inputField.text = null;
            // 친구 프리팹 추가
            GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
            // 이름 적용
            _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
            // 추가된 친구 프리팹의 삭제 버튼에 삭제 리스너 부착
            SetDeleteBtnListener();

            Debug.LogFormat("FriendName : {0}", friendName);
            StartCoroutine(UpdateFriendListInDynamoDB(friendName, _friend));
        }
    }

    // 친구 Db에서 삭제
    void DeleteFriend(string friendName, GameObject parentObject)
    {
        Destroy(parentObject);
        Debug.LogFormat("{0} Deleted", friendName);
        StartCoroutine(DeleteFriendFromDynamoDB(friendName));
    }

    // 친구 목록 Db에서 불러오기
    void LoadFriend()
    {
        StartCoroutine(GetFriendListFromDynamoDB());
    }

    void GoFriendDorm()
    {
        // 친구 기숙사 정보 Db에서 받아오고 놀러가기
    }
    void FriendChat()
    {
        // 친구와 1대1 채팅
    }

    IEnumerator UpdateFriendListInDynamoDB(string friendName, GameObject _friend)
    {
        string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/friend";

        var request = new UnityWebRequest(apiGatewayUrl, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"friendName\":\"{friendName}\", \"userName\":\"{PlayerInfo.playerInfo.nickname}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            FriendResponseBody response = JsonUtility.FromJson<FriendResponseBody>(request.downloadHandler.text);

            // Check if the user was not found
            if (response.statusCode == "400")
            {
                Debug.Log("User not found, friend not added!");
                Destroy(_friend); // If failed to add friend, destroy the added friend prefab
            }
            else
            {
                Debug.Log("Friend list update successful!");
                LoadFriend();  // Refresh friend list
            }
        }
        else
        {
            Debug.Log($"Friend list update failed: {request.error}");
            Debug.Log($"Response Code: {(int)request.responseCode}");
            Debug.Log($"Response: {request.downloadHandler.text}");
            Destroy(_friend); // If failed to add friend, destroy the added friend prefab
        }

        request.Dispose(); // Prevent memory leak
    }



    IEnumerator GetFriendListFromDynamoDB()
    {
        string apiGatewayUrl2 = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/";

        var request = new UnityWebRequest(apiGatewayUrl2, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"userName\":\"{PlayerInfo.playerInfo.nickname}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Get friend list successful!");
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Response: {request.downloadHandler.text}");

                FriendResponseBody response = JsonUtility.FromJson<FriendResponseBody>(jsonResponse);

                FriendList friendList = JsonUtility.FromJson<FriendList>(response.body);
                Debug.Log(friendList.friends.Count);

                foreach (Transform child in content.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (var friendName in friendList.friends)
                {
                    Debug.Log("Friend: " + friendName);

                    // 친구 프리팹 추가
                    GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
                    // 이름 적용
                    _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
                }
                SetDeleteBtnListener();

            }
            else
            {
                Debug.Log($"Get friend list failed: {request.error}");
                Debug.Log($"Response Code: {(int)request.responseCode}");
                Debug.Log($"Response: {request.downloadHandler.text}");
            }
        }
        finally
        {
            request.Dispose();
        }
    }

    IEnumerator DeleteFriendFromDynamoDB(string friendName)
    {
        string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/frienddelete";

        var request = new UnityWebRequest(apiGatewayUrl, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"friendName\":\"{friendName}\", \"userName\":\"{PlayerInfo.playerInfo.nickname}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Friend deletion successful!");
                LoadFriend();  // 친구 목록 갱신
            }
            else
            {
                Debug.Log($"Friend deletion failed: {request.error}");
                Debug.Log($"Response Code: {(int)request.responseCode}");
                Debug.Log($"Response: {request.downloadHandler.text}");
            }
        }
        finally
        {
            request.Dispose();
        }
    }

}
