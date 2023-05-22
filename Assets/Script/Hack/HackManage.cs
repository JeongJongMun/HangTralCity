using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection.Emit;

public class HackManage : MonoBehaviour
{

    public GameObject writePanel, postPanel;

    // 게시물 메인
    public Button writePostBtn; // 게시물 작성 버튼
    public Button post; // 동적으로 추가할 게시물 (버튼)
    public GameObject content; // 게시물 content

    // 게시물 작성
    public TMP_InputField writeTitleInputField, writeDetailInputField; // 게시물 제목, 내용 inputfield
    public Button writeDoneBtn, writeBackBtn; // 게시물 작성 버튼, 뒤로가기 버튼

    // 게시물 보기
    public TMP_Text postTitleTxt, postDetailTxt; // 게시물의 제목, 내용
    public TMP_InputField postCommentInputField; // 게시물 댓글 inputfield
    public Button postCommentBtn, postBackBtn; // 게시물 댓글달기 버튼, 뒤로가기 버튼

    // Start is called before the first frame update
    void Start()
    {
        writePostBtn.onClick.AddListener(ClickWritePostBtn);
        writeBackBtn.onClick.AddListener(ClickBackBtn);
        postBackBtn.onClick.AddListener(ClickBackBtn);
        writeDoneBtn.onClick.AddListener(WritePost);
        // 게시물 불러오기 함수   
    }

    // 게시물 작성 버튼 클릭 시 Panel 활성화
    void ClickWritePostBtn()
    {
        writePanel.SetActive(true);
    }
    void ClickBackBtn()
    {
        writePanel.SetActive(false);
        postPanel.SetActive(false);
    }

    void WritePost()
    {
        string title = writeTitleInputField.text; // 입력한 제목
        string detail = writeDetailInputField.text; // 입력한 내용
        Button _post = Instantiate(post, new Vector2(0, 0), Quaternion.identity, content.transform); // 게시물 생성
        TMP_Text _title = _post.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text _detail = _post.transform.GetChild(1).GetComponent<TMP_Text>();
        _title.text = title; // 제목 적용
        _detail.text = detail; // 내용 적용
        writePanel.SetActive(false); // 게시물 작성 panel 닫기
    }

}
