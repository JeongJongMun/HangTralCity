using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonConnectionInPlayground : MonoBehaviourPunCallbacks
{
    [Header("User In and Out")]
    public GameObject UserInPanel;
    public GameObject UserOutPanel;

    //[Header("ChatPanel")]
    //private Text ChatText;
    //public InputField ChatInput;

    //[Header("ETC")]
    //public PhotonView PV;
    //public Animator AN;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Screen.SetResolution(1080, 2340, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            UserInPanel.SetActive(true);
            UserOutPanel.SetActive(false);

            //PV.RPC("SetCharacter", RpcTarget.All);
        }
    }

    //[PunRPC]
    //private void SetCharacter()
    //{
    //    AN.SetInteger("type", PlayerInfo.playerInfo.characterType);
    //}

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster\n");

        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.playerInfo.nickname; // 닉네임 가져오기


        PhotonNetwork.JoinOrCreateRoom("MiniGameRoom", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        Debug.LogFormat("{0}님이 방에 참가하였습니다.", PlayerInfo.playerInfo.nickname);
        Invoke("Panel_", 1f);
    }

    private void Panel_()
    {
        UserInPanel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
}
