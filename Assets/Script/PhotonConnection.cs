using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection photon_connection;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // player_info가 유일한 인스턴스
        if (photon_connection == null) photon_connection = this;
        // player_info 인스턴스가 이게 아니라면, 다른 인스턴스 삭제
        else if (photon_connection != this) Destroy(gameObject);
    }
    public TMP_Text for_test;
    void Start()
    {
        Screen.SetResolution(1080, 2340, false);
        Debug.Log("접속로그\n");
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;

        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.player_info.nickname;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);

    }
    public override void OnJoinedRoom()
    {
        updatePlayer();
        Debug.LogFormat("{0}님이 방에 참가하였습니다.", PlayerInfo.player_info.nickname);
    }

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