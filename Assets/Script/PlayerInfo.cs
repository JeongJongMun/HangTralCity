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

        // player_info�� ������ �ν��Ͻ�
        if (player_info == null) player_info = this;
        // player_info �ν��Ͻ��� �̰� �ƴ϶��, �ٸ� �ν��Ͻ� ����
        else if (player_info != this) Destroy(gameObject);
    }
}
