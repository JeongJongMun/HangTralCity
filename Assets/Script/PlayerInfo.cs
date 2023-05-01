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
    public int custom_hat = -1; // 캐릭터 커스터마이징 hat 에셋 저장
    public int custom_eye = -1; // 캐릭터 커스터마이징 eye 에셋 저장
    public Dictionary<string, Vector3> furniture_pos = new Dictionary<string, Vector3>(); // 방 커스터마이징 가구 종류 및 위치 저장
    public int character_type;
    
    // PlayerInfo 클래스를 싱글 톤으로 생성
    // 단 하나의 인스턴스만을 가지며 어디서든 접근 가능
    // 접근시 PlayerInfo.player_info.변수로 접근
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // player_info가 유일한 인스턴스
        if (player_info == null) player_info = this;
        // player_info 인스턴스가 이게 아니라면, 다른 인스턴스 삭제
        else if (player_info != this) Destroy(gameObject);
    }
}
