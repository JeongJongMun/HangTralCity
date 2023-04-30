using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonConnectionInGang : MonoBehaviourPunCallbacks
{
    public Button GoMain;

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

    public void OnClickGoMain()
    {
        if (PhotonNetwork.IsConnected)
        {
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
