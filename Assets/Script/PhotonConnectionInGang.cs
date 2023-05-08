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
    private Text ChatText;
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

            UserOutPanel.SetActive(false);
            UserInPanel.SetActive(true);
        }
    }

    private void Start()
    {
        dooropenEffect = GetComponent<AudioSource>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster\n");
        
        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.playerInfo.nickname; // 닉네임 가져오기
        

        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 10 }, null);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3((float)-9.9, 6, -1), Quaternion.identity);
        Debug.LogFormat("{0}님이 방에 참가하였습니다.", PlayerInfo.playerInfo.nickname);
        UserInPanel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainScene");
        Debug.Log("Move\n");
    }

    /*public void Send()
    {
        string msg = ChatInput.text;
        PV.RPC("ChatRPC", RpcTarget.All, ChatInput.text);
        
        ChatInput.text = ""; 
    }


    [PunRPC]
    void ChatRPC(string msg)
    {
        ChatText = GameObject.Find("ChatText").GetComponent<Text>();

        if(ChatText.text == "")
        {
            ChatText.text = msg;
        }
        else
        {
            ChatText.text = "";
            ChatText.text = msg;
        }*/
        
     
        /*bool isInput = false;
            if (ChatText == "")
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
        
    }*/
    
}
