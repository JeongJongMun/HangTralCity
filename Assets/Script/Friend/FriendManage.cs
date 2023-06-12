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

public class FriendResponseBody  // �̸� ����
{
    public string statusCode;
    public string body;
}


public class FriendManage : MonoBehaviour
{
    [Header("GoPlay -> Off")]
    public GameObject addBtn; // ģ�� �߰� ��ư
    public GameObject inputField; // ģ�� �̸� �Է�â
    public GameObject scrollView;

    [Header("GoPlay -> On")]
    public GameObject backBtn;

    [Header("Content : Prefab SpawnPos")]
    public GameObject content; // �������� �߰��� ģ�� �������� �θ� ������Ʈ

    [Header("Prefab")]
    public GameObject friend; // �������� �߰� �� ģ�� ������

    Button[] deleteButtons; // ���� ��ư��
    Button[] goPlayButtons; // ����� ��ư��

    private void Start()
    {
        addBtn.GetComponent<Button>().onClick.AddListener(AddFriend);
        backBtn.GetComponent<Button>().onClick.AddListener(BackToFriendList);
        LoadFriend();
        SetButtonListener();
    }

    void SetButtonListener()
    {
        // �̸����� ��� ���� ��ư�� ã��
        deleteButtons = FindObjectsOfType<Button>().Where(button => button.name == "DeleteBtn").ToArray();
        // �̸����� ��� ����� ��ư�� ã��
        goPlayButtons = FindObjectsOfType<Button>().Where(button => button.name == "GoPlayBtn").ToArray();

        // ã�� ��ư�鿡 ���� ������ ���� �Լ��� ����
        foreach (Button button in deleteButtons)
        {
            button.onClick.AddListener(() => DeleteFriend(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text, button.transform.parent.gameObject));
        }
        // ã�� ��ư�鿡 ���� ������ ����� �Լ��� ����
        foreach (Button button in goPlayButtons)
        {
            button.onClick.AddListener(() => GoPlayDorm(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text));
        }
    }

    // ģ�� �߰�
    void AddFriend()
    {
        string friendName;
        if (inputField.GetComponent<TMP_InputField>().text != null)
        {
            friendName = inputField.GetComponent<TMP_InputField>().text;
            inputField.GetComponent<TMP_InputField>().text = null;
            // ģ�� ������ �߰�
            GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
            // �̸� ����
            _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
            // �߰��� ģ�� �������� ��ư�� ������ ����
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
        // ģ�� ��� Db���� �ҷ�����
        StartCoroutine(GetFriendListFromDynamoDB());
    }

    void GoPlayDorm(string name) // name = ����� ����� ��ư ���� �������� �̸�
    {
        Debug.LogFormat("GoPlayDorm : {0}", name);
        // ����� ��� ��������
        addBtn.SetActive(false);
        inputField.SetActive(false);
        scrollView.SetActive(false);
        backBtn.SetActive(true);

        // ����� ���� �ҷ����� �� ���� ��ġ
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

                    // ģ�� ������ �߰�
                    GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
                    // �̸� ����
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
                // DB���� ���������� ģ���� ������ �Ŀ� �������� ����
                Destroy(parentObject);
                LoadFriend();  // ģ�� ��� ����
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
