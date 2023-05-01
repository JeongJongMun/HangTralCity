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
    public int custom_hat = -1; // ĳ���� Ŀ���͸���¡ hat ���� ����
    public int custom_eye = -1; // ĳ���� Ŀ���͸���¡ eye ���� ����
    public Dictionary<string, Vector3> furniture_pos = new Dictionary<string, Vector3>(); // �� Ŀ���͸���¡ ���� ���� �� ��ġ ����
    public int character_type;
    
    // PlayerInfo Ŭ������ �̱� ������ ����
    // �� �ϳ��� �ν��Ͻ����� ������ ��𼭵� ���� ����
    // ���ٽ� PlayerInfo.player_info.������ ����
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // player_info�� ������ �ν��Ͻ�
        if (player_info == null) player_info = this;
        // player_info �ν��Ͻ��� �̰� �ƴ϶��, �ٸ� �ν��Ͻ� ����
        else if (player_info != this) Destroy(gameObject);
    }
}
