using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sentence : MonoBehaviourPunCallbacks
{
    /*private string[] sentences; // ä�� �ؽ�Ʈ
    public Transform chatTr; // ��ǳ�� ��ġ
    public GameObject chatBoxPrefab; // ��ǳ�� ������
    private Button sendBtn; // ä�� ������ ��ư UI
    private InputField inputfield; // ä�� �Է�â UI*/

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