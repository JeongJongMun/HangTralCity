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
        Debug.Log("OnConnectedToMaster\n");

        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.playerInfo.nickname; // �г��� ��������
        PhotonNetwork.JoinOrCreateRoom("PlaygroundRoom", new RoomOptions { MaxPlayers = 5 }, null); // �濡 �����ϰų�, ���� ���ٸ� ���� �� ����
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity);
        Debug.LogFormat("{0}���� �濡 �����Ͽ����ϴ�.", PlayerInfo.playerInfo.nickname);
        Invoke("UserIn", 1f);
    }

    private void UserIn()
    {
        UserInPanel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
}
