using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnectionInPlayground : MonoBehaviourPunCallbacks
{
    [Header("User In")]
    public GameObject UserInPanel;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Screen.SetResolution(1080, 2340, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            UserInPanel.SetActive(true);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.playerInfo.nickname;
        PhotonNetwork.JoinOrCreateRoom("PlaygroundRoom", new RoomOptions { MaxPlayers = 5 }, null); 
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity);
        Debug.LogFormat("{0}님이 방에 참가하였습니다.", PlayerInfo.playerInfo.nickname);
        Invoke("UserIn", 1f);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainScene");
    }

    private void UserIn()
    {
        UserInPanel.SetActive(false);
    }
}
