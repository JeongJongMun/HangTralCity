using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    // ī�޶� �÷��̾� ����ٴϴ� ��ũ��Ʈ
    // ���ǵ� ��� ���
    // ���ο��� ��� X
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        //Vector3 tempVec = player.transform.position;
        //tempVec.z = -10;
        //transform.position = tempVec;
    
    }
    
}
