using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatbotManage : MonoBehaviour
{
    public TMP_InputField requestInputField; // ��û �Է�â
    public Button sendBtn;
    public GameObject requestGroup, responseGroup; // �������� �߰��� ��û, ���� ��ǳ��
    public GameObject content; // ��ȭâ content



    // Start is called before the first frame update
    void Start()
    {
        sendBtn.onClick.AddListener(Request);
    }

    void Request()
    {
        string sentence = requestInputField.text;
        GameObject _requestGroup = Instantiate(requestGroup, new Vector2(0, 0), Quaternion.identity, content.transform); // ��û ��ǳ�� ����
        TMP_Text _sentence = _requestGroup.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        _sentence.text = sentence; // ���� ����
        requestInputField.text = "";
    }

}
