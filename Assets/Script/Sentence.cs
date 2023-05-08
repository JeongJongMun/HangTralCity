using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sentence : MonoBehaviourPunCallbacks
{
    /*private string[] sentences; // 채팅 텍스트
    public Transform chatTr; // 말풍선 위치
    public GameObject chatBoxPrefab; // 말풍선 프리팹
    private Button sendBtn; // 채팅 보내기 버튼 UI
    private InputField inputfield; // 채팅 입력창 UI*/

    private void Start()
    {
       /*if (SceneManager.GetActiveScene().name == "GangScene")
        {
            inputfield = GameObject.Find("ChatInputfield").GetComponent<InputField>();
            sendBtn = GameObject.Find("ChatSendBtn").GetComponent<Button>();
            sendBtn.onClick.AddListener(Talk);
        }*/
    }

    private void Talk()
    {
        /*if (inputfield.text != null)
        {
            sentences = new string[] { inputfield.text };
            inputfield.text = "";
            GameObject go = Instantiate(chatBoxPrefab);
            go.GetComponent<ChatSystem>().Ondialogue(sentences, chatTr);
        }*/
    }
}