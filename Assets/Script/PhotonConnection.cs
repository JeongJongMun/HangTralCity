using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public TMP_Text for_test;
    public Button EnterGang;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Screen.SetResolution(1080, 2340, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
            Debug.Log("Awake\n");
        }
    }
    /*void Start()
    {
        Screen.SetResolution(1080, 2340, false);

        Debug.Log("접속로그\n");
        Connect();
    }*/

    public void OnClickEnterGang()
    {
        Debug.Log("enter\n");
        PhotonNetwork.LoadLevel("GangScene");
        updatePlayer();
        Debug.LogFormat("{0}님이 방에 참가하였습니다.2", PlayerInfo.player_info.nickname);
    }

    /*public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster\n");
*//*        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;*//*

        //PhotonNetwork.LocalPlayer.NickName = PlayerInfo.player_info.nickname;
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 10 }, null);
    }*/

    /*public override void OnJoinedRoom() //JoinOrCreateRoom callbackfunction
    {
        Debug.Log("enter\n");
        PhotonNetwork.LoadLevel("GangScene");
        updatePlayer();
        Debug.LogFormat("{0}님이 방에 참가하였습니다.2", PlayerInfo.player_info.nickname);
    }*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        updatePlayer();
        Debug.LogFormat("{0}님이 입장하였습니다.",newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updatePlayer();
        Debug.LogFormat("{0}님이 퇴장하였습니다.", otherPlayer.NickName);
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connect Function\n");
    }
    

    void updatePlayer()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.LogFormat("현재 접속자 : {0}", PhotonNetwork.PlayerList[i].NickName);
            for_test.text += PhotonNetwork.PlayerList[i].NickName;
        }
    }

}