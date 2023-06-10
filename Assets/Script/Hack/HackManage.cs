using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;

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

    void Start()
    {
        writePostBtn.onClick.AddListener(ClickWritePostBtn);
        writeBackBtn.onClick.AddListener(ClickBackBtn);
        postBackBtn.onClick.AddListener(ClickBackBtn);
        doneBtn.onClick.AddListener(WritePost);
        commentBtn.onClick.AddListener(WriteComment);
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
            Button _post = Instantiate(post, new Vector2(0, 0), Quaternion.identity, content.transform); // 게시물 생성
            _post.transform.GetChild(0).GetComponent<TMP_Text>().text = title; // 제목 적용
            _post.transform.GetChild(1).GetComponent<TMP_Text>().text = detail; // 내용 적용
            writePanel.SetActive(false); // 게시물 작성 panel 닫기
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
        }

    }

}
