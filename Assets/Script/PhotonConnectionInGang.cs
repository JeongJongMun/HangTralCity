using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonConnectionInGang : MonoBehaviourPunCallbacks
{
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
    }

    public void OnClickGoMain()
    {
        if (PhotonNetwork.IsConnected)
        {
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
}
