using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo player_info;
    public string nickname;
    public string email;
    public string access_token;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // player_info가 유일한 인스턴스
        if (player_info == null) player_info = this;
        // player_info 인스턴스가 이게 아니라면, 다른 인스턴스 삭제
        else if (player_info != this) Destroy(gameObject);
    }
}
