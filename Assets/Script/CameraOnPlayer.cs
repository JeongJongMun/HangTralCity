using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    // 카메라가 플레이어 따라다니는 스크립트
    // 강의동 등에서 사용
    // 메인에서 사용 X
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
