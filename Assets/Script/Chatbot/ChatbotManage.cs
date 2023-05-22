using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatbotManage : MonoBehaviour
{
    public TMP_InputField requestInputField; // 요청 입력창
    public Button sendBtn;
    public GameObject requestGroup, responseGroup; // 동적으로 추가할 요청, 응답 말풍선
    public GameObject content; // 대화창 content



    // Start is called before the first frame update
    void Start()
    {
        sendBtn.onClick.AddListener(Request);
    }

    void Request()
    {
        string sentence = requestInputField.text;
        GameObject _requestGroup = Instantiate(requestGroup, new Vector2(0, 0), Quaternion.identity, content.transform); // 요청 말풍선 생성
        TMP_Text _sentence = _requestGroup.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        _sentence.text = sentence; // 내용 적용
        requestInputField.text = "";
    }

}
