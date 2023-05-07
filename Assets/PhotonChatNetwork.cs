using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonChatNetwork : MonoBehaviourPunCallbacks
{

    [Header("ChatScene")]
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public PhotonView PV;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Screen.SetResolution(1080, 2340, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
            Debug.Log("Awake\n");
        }
        else
        {
            Debug.Log("already in server");
        }
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) Debug.Log(ChatText[i].text);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster\n");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 20;

        //PhotonNetwork.LocalPlayer.NickName = PlayerInfo.player_info.nickname;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("{0}님이 채팅에 참가하였습니다.", PlayerInfo.playerInfo.nickname);
        PV.RPC("ChatRPC", RpcTarget.All, "<size=60><color=#ffa93a>" + PlayerInfo.playerInfo.nickname + " 님이 채팅에 참가하였습니다</color></size>");
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainChatScene")
        {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect Chat\n");
    }

    public void Send()
    {
        string msg = PlayerInfo.playerInfo.nickname + " : " + ChatInput.text;
        if (PV.IsMine)
        {
            PV.RPC("ChatRPC", RpcTarget.All, "<color=#82a571>" + PlayerInfo.playerInfo.nickname + "</color>" + " : " + ChatInput.text);
        }
        else
        {
            PV.RPC("ChatRPC", RpcTarget.All, "<color=#7d9dcd>" + PlayerInfo.playerInfo.nickname + "</color>" + " : " + ChatInput.text);
        }
        
        ChatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
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