using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonConnectionInGang : MonoBehaviourPunCallbacks
{
    [Header("User In and Out")]
    public GameObject UserInPanel;
    public GameObject UserOutPanel;

    [Header("ChatPanel")]
    public GameObject ChatPanel;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public PhotonView PV;
    AudioSource dooropenEffect;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Screen.SetResolution(1080, 2340, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
            Debug.Log("Awake\n");
            UserOutPanel.SetActive(false);
            UserInPanel.SetActive(true);
            ChatPanel.SetActive(false);
        }
    }

    private void Start()
    {
        dooropenEffect = GetComponent<AudioSource>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster\n");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;

        //PhotonNetwork.LocalPlayer.NickName = PlayerInfo.player_info.nickname;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(-11, 6, -1), Quaternion.identity);
        Debug.LogFormat("{0}님이 방에 참가하였습니다.", PlayerInfo.player_info.nickname);
        UserInPanel.SetActive(false);
    }

    public void OnClickChat ()
    {
        ChatPanel.SetActive(true);
        //OnJoinedRoom();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }
    public void OnClickGoMain()
    {
        if (PhotonNetwork.IsConnected)
        {
            UserOutPanel.SetActive(true);
            dooropenEffect.Play();
            PhotonNetwork.Disconnect();
            Debug.Log("Disconnect\n");
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainScene");
        Debug.Log("Move\n");
    }

    public void Input(Text text)
    {
        text.text = ChatInput.text;
    }

    public void OnClickExitChat()
    {
        ChatPanel.SetActive(false);
    }

    public void Send()
    {
        string msg = PlayerInfo.player_info.nickname + ":" + ChatInput.text;
        PV.RPC("ChatRPC", RpcTarget.All, PlayerInfo.player_info.nickname + ":" + ChatInput.text);
        ChatInput.text = ""; 
    }


    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for(int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput)
        {
            for (int j = 1; j < ChatText.Length; j++) ChatText[j - 1].text = ChatText[j].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
        
    }
}
