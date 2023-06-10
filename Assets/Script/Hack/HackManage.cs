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
    public Button writePostBtn; // 게시물 작성 버튼
    public Button post; // 동적으로 추가할 게시물 (버튼)
    public GameObject content; // 게시물 content

    // 게시물 작성
    [Header("Write")]
    public TMP_InputField titleInputField; // 게시물 제목
    public TMP_InputField detailInputField; // 내용
    public Button doneBtn; // 게시물 작성 버튼
    public Button writeBackBtn; // 뒤로가기 버튼

    // 게시물 보기
    [Header("Read")]
    public TMP_Text titleText; // 게시물의 제목
    public TMP_Text detailText; // 내용
    public TMP_InputField commentInputField; // 게시물 댓글 inputfield
    public Button commentBtn; // 게시물 댓글달기 버튼
    public GameObject comments; // 댓글 달릴 곳
    public GameObject commentGroup; // 댓글 프리팹
    public Button postBackBtn; // 뒤로가기 버튼

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

    // 게시물 작성 버튼 클릭 함수
    void ClickWritePostBtn()
    {
        writePanel.SetActive(true);
    }
    // 뒤로가기 버튼 클릭 함수
    void ClickBackBtn()
    {
        writePanel.SetActive(false);
        postPanel.SetActive(false);
    }

    // 게시물 작성하는 함수
    void WritePost()
    {
        if (!titleInputField.text.IsNullOrEmpty() && !detailInputField.text.IsNullOrEmpty())
        {
            string title = titleInputField.text; // 입력한 제목
            string detail = detailInputField.text; // 입력한 내용
            string PK = PlayerInfo.playerInfo.nickname;
            string timestamp = System.DateTime.Now.ToString(); // 현재 시간을 문자열로 변환

            // 게시물 생성
            Button _post = Instantiate(post, new Vector2(0, 0), Quaternion.identity, content.transform);
            _post.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
            _post.transform.GetChild(1).GetComponent<TMP_Text>().text = detail;

            StartCoroutine(SavePost(PK, title, detail, timestamp));
            titleInputField.text = null;
            detailInputField.text = null;
            writePanel.SetActive(false);
        }

    }
    // 댓글 작성하는 함수
    void WriteComment()
    {
        if (!commentInputField.text.IsNullOrEmpty())
        {
            string comment = commentInputField.text; // 입력한 댓글
            commentInputField.text = ""; // 입력창 초기화
            GameObject _comment = Instantiate(commentGroup, new Vector2(0, 0), Quaternion.identity, comments.transform); // 댓글 생성
            _comment.transform.GetChild(0).GetComponent<TMP_Text>().text = "익명"; // 닉네임 적용
            _comment.transform.GetChild(1).GetComponent<TMP_Text>().text = comment; // 댓글 내용 적용
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
    public string PK;  // PK 추가
    public string title;
    public string detail;
    public string timestamp;  // timestamp 추가
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