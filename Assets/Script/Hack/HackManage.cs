using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection.Emit;

public class HackManage : MonoBehaviour
{

    public GameObject writePanel, postPanel;

    // �Խù� ����
    public Button writePostBtn; // �Խù� �ۼ� ��ư
    public Button post; // �������� �߰��� �Խù� (��ư)
    public GameObject content; // �Խù� content

    // �Խù� �ۼ�
    public TMP_InputField writeTitleInputField, writeDetailInputField; // �Խù� ����, ���� inputfield
    public Button writeDoneBtn, writeBackBtn; // �Խù� �ۼ� ��ư, �ڷΰ��� ��ư

    // �Խù� ����
    public TMP_Text postTitleTxt, postDetailTxt; // �Խù��� ����, ����
    public TMP_InputField postCommentInputField; // �Խù� ��� inputfield
    public Button postCommentBtn, postBackBtn; // �Խù� ��۴ޱ� ��ư, �ڷΰ��� ��ư

    // Start is called before the first frame update
    void Start()
    {
        writePostBtn.onClick.AddListener(ClickWritePostBtn);
        writeBackBtn.onClick.AddListener(ClickBackBtn);
        postBackBtn.onClick.AddListener(ClickBackBtn);
        writeDoneBtn.onClick.AddListener(WritePost);
        // �Խù� �ҷ����� �Լ�   
    }

    // �Խù� �ۼ� ��ư Ŭ�� �� Panel Ȱ��ȭ
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
        string title = writeTitleInputField.text; // �Է��� ����
        string detail = writeDetailInputField.text; // �Է��� ����
        Button _post = Instantiate(post, new Vector2(0, 0), Quaternion.identity, content.transform); // �Խù� ����
        TMP_Text _title = _post.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text _detail = _post.transform.GetChild(1).GetComponent<TMP_Text>();
        _title.text = title; // ���� ����
        _detail.text = detail; // ���� ����
        writePanel.SetActive(false); // �Խù� �ۼ� panel �ݱ�
    }

}
