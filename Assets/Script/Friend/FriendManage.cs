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
    [Header("GoPlay -> Off")]
    public GameObject addBtn; // 친구 추가 버튼
    public GameObject inputField; // 친구 이름 입력창
    public GameObject scrollView;

    [Header("GoPlay -> On")]
    public GameObject backBtn;

    [Header("Content : Prefab SpawnPos")]
    public GameObject content; // 동적으로 추가할 친구 프리팹의 부모 오브젝트

    [Header("Prefab")]
    public GameObject friend; // 동적으로 추가 할 친구 프리팹

    Button[] deleteButtons; // 삭제 버튼들
    Button[] goPlayButtons; // 놀러가기 버튼들

    private void Start()
    {
        addBtn.GetComponent<Button>().onClick.AddListener(AddFriend);
        backBtn.GetComponent<Button>().onClick.AddListener(BackToFriendList);
        LoadFriend();
        SetButtonListener();
    }

    void SetButtonListener()
    {
        // 이름으로 모든 삭제 버튼을 찾음
        deleteButtons = FindObjectsOfType<Button>().Where(button => button.name == "DeleteBtn").ToArray();
        // 이름으로 모든 놀러가기 버튼을 찾음
        goPlayButtons = FindObjectsOfType<Button>().Where(button => button.name == "GoPlayBtn").ToArray();

        // 찾은 버튼들에 대해 동일한 삭제 함수를 연결
        foreach (Button button in deleteButtons)
        {
            button.onClick.AddListener(() => DeleteFriend(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text, button.transform.parent.gameObject));
        }
        // 찾은 버튼들에 대해 동일한 놀러가기 함수를 연결
        foreach (Button button in goPlayButtons)
        {
            button.onClick.AddListener(() => GoPlayDorm(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text));
        }
    }

    // 친구 추가
    void AddFriend()
    {
        string friendName;
        if (inputField.GetComponent<TMP_InputField>().text != null)
        {
            friendName = inputField.GetComponent<TMP_InputField>().text;
            inputField.GetComponent<TMP_InputField>().text = null;
            // 친구 프리팹 추가
            GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
            // 이름 적용
            _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
            // 추가된 친구 프리팹의 버튼에 리스너 부착
            SetButtonListener();

            Debug.LogFormat("Friend Added : {0}", friendName);
            StartCoroutine(UpdateFriendListInDynamoDB(friendName, _friend));
        }
    }

    void DeleteFriend(string friendName, GameObject parentObject)
    {
        StartCoroutine(DeleteFriendFromDynamoDB(friendName, parentObject));

    }

    void LoadFriend()
    {
        // 친구 목록 Db에서 불러오기
        StartCoroutine(GetFriendListFromDynamoDB());
    }

    void GoPlayDorm(string name) // name = 기숙사 놀러가기 버튼 누른 프리팹의 이름
    {
        Debug.LogFormat("GoPlayDorm : {0}", name);
        // 기숙사 배경 보여지기
        addBtn.SetActive(false);
        inputField.SetActive(false);
        scrollView.SetActive(false);
        backBtn.SetActive(true);

        // 기숙사 정보 불러오기 및 가구 배치
    }
    void BackToFriendList()
    {
        addBtn.SetActive(true);
        inputField.SetActive(true);
        scrollView.SetActive(true);
        backBtn.SetActive(false);
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
                SetButtonListener();

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

    IEnumerator DeleteFriendFromDynamoDB(string friendName, GameObject parentObject)
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
                // DB에서 성공적으로 친구를 삭제한 후에 프리팹을 삭제
                Destroy(parentObject);
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
