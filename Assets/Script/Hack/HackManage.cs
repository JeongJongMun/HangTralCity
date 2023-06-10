using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class HackManage : MonoBehaviour
{

    [Header("Panel")]
    public GameObject writePanel;
    public GameObject postPanel;

    [Header("Main")]
    public Button writePostBtn; // �Խù� �ۼ� ��ư
    public Button post; // �������� �߰��� �Խù� (��ư)
    public GameObject content; // �Խù� content

    // �Խù� �ۼ�
    [Header("Write")]
    public TMP_InputField titleInputField; // �Խù� ����
    public TMP_InputField detailInputField; // ����
    public Button doneBtn; // �Խù� �ۼ� ��ư
    public Button writeBackBtn; // �ڷΰ��� ��ư

    // �Խù� ����
    [Header("Read")]
    public TMP_Text titleText; // �Խù��� ����
    public TMP_Text detailText; // ����
    public TMP_InputField commentInputField; // �Խù� ��� inputfield
    public Button commentBtn; // �Խù� ��۴ޱ� ��ư
    public GameObject comments; // ��� �޸� ��
    public GameObject commentGroup; // ��� ������
    public Button postBackBtn; // �ڷΰ��� ��ư

    private string saveUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/hack";


    void Start()
    {
        writePostBtn.onClick.AddListener(ClickWritePostBtn);
        writeBackBtn.onClick.AddListener(ClickBackBtn);
        postBackBtn.onClick.AddListener(ClickBackBtn);
        doneBtn.onClick.AddListener(WritePost);
        commentBtn.onClick.AddListener(WriteComment);
        StartCoroutine(LoadAllPosts());
    }

    // �Խù� �ۼ� ��ư Ŭ�� �Լ�
    void ClickWritePostBtn()
    {
        writePanel.SetActive(true);
    }
    // �ڷΰ��� ��ư Ŭ�� �Լ�
    void ClickBackBtn()
    {
        writePanel.SetActive(false);
        postPanel.SetActive(false);
    }

    // �Խù� �ۼ��ϴ� �Լ�
    void WritePost()
    {
        if (!titleInputField.text.IsNullOrEmpty() && !detailInputField.text.IsNullOrEmpty())
        {
            string title = titleInputField.text; // �Է��� ����
            string detail = detailInputField.text; // �Է��� ����
            string PK = PlayerInfo.playerInfo.nickname;
            string timestamp = System.DateTime.Now.ToString(); // ���� �ð��� ���ڿ��� ��ȯ

            // �Խù� ����
            Button _post = Instantiate(post, new Vector2(0, 0), Quaternion.identity, content.transform);
            _post.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
            _post.transform.GetChild(1).GetComponent<TMP_Text>().text = detail;

            StartCoroutine(SavePost(PK, title, detail, timestamp));
            titleInputField.text = null;
            detailInputField.text = null;
            writePanel.SetActive(false);
        }

    }
    // ��� �ۼ��ϴ� �Լ�
    void WriteComment()
    {
        if (!commentInputField.text.IsNullOrEmpty())
        {
            string comment = commentInputField.text; // �Է��� ���
            commentInputField.text = ""; // �Է�â �ʱ�ȭ
            GameObject _comment = Instantiate(commentGroup, new Vector2(0, 0), Quaternion.identity, comments.transform); // ��� ����
            _comment.transform.GetChild(0).GetComponent<TMP_Text>().text = "�͸�"; // �г��� ����
            _comment.transform.GetChild(1).GetComponent<TMP_Text>().text = comment; // ��� ���� ����
            commentInputField.text = null;
        }
    }

    IEnumerator SavePost(string PK, string title, string detail, string timestamp)
    {
        HackPost post = new HackPost { PK = PK, title = title, detail = detail, timestamp = timestamp };
        string bodyJson = JsonUtility.ToJson(post);
        string jsonData = bodyJson;

        var request = new UnityWebRequest(saveUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Post save request error: " + request.error);
        }
        else
        {
            Debug.Log("Post save response: " + request.downloadHandler.text);
        }
    }


    IEnumerator LoadAllPosts()
    {
        string loadUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/hack";
        using (UnityWebRequest www = UnityWebRequest.Get(loadUrl))
        {
            string jsonData = "{\"httpMethod\": \"GET\"}";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Post load request error: " + www.error);
            }
            else
            {
                Debug.Log("Post load response: " + www.downloadHandler.text);

                // Parse the API response
                APIResponse response = JsonUtility.FromJson<APIResponse>(www.downloadHandler.text);

                // Parse the actual posts from the body field
                HackPost[] posts = JsonConvert.DeserializeObject<HackPost[]>(response.body);

                foreach (HackPost post in posts)
                {
                    // Only process posts with all required fields
                    if (post.PK != null && post.title != null && post.detail != null)
                    {
                        Button _post = Instantiate(this.post, new Vector2(0, 0), Quaternion.identity, content.transform);
                        _post.transform.GetChild(0).GetComponent<TMP_Text>().text = post.title;
                        _post.transform.GetChild(1).GetComponent<TMP_Text>().text = post.detail;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class HackPost
{
    public string PK;  // PK �߰�
    public string title;
    public string detail;
    public string timestamp;  // timestamp �߰�
    public string username;
    public int? eyeCustom;
    public int? hatCustom;
    public List<string> friends;
    public Dictionary<string, string> furniture_positions;
}

[System.Serializable]
public class APIResponse
{
    public int statusCode;
    public string body;
}