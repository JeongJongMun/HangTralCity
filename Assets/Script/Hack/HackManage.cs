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

    void Start()
    {
        writePostBtn.onClick.AddListener(ClickWritePostBtn);
        writeBackBtn.onClick.AddListener(ClickBackBtn);
        postBackBtn.onClick.AddListener(ClickBackBtn);
        doneBtn.onClick.AddListener(WritePost);
        commentBtn.onClick.AddListener(WriteComment);
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
            Button _post = Instantiate(post, new Vector2(0, 0), Quaternion.identity, content.transform); // �Խù� ����
            _post.transform.GetChild(0).GetComponent<TMP_Text>().text = title; // ���� ����
            _post.transform.GetChild(1).GetComponent<TMP_Text>().text = detail; // ���� ����
            writePanel.SetActive(false); // �Խù� �ۼ� panel �ݱ�
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
        }

    }

}
