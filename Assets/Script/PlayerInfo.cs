using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo playerInfo;
    public string nickname;
    public string email;
    public string access_token; // Cognito 로그인시 받는 토큰
    public int hatCustom = 8; // 캐릭터 커스터마이징 hat 에셋 저장, 기본값 = 아무것도 없는 none이미지 8
    public int eyeCustom = 11; // 캐릭터 커스터마이징 eye 에셋 저장, 기본값 = 아무것도 없는 none이미지 11
    public Dictionary<string, List<Vector3>> funiturePos = new Dictionary<string, List<Vector3>>(); // 방 커스터마이징 가구 종류 및 위치 저장
    public int characterType = 0; // 캐릭터 타입
    public float volume = 0.5f; // BGM 볼륨
    
    // PlayerInfo 클래스를 싱글 톤으로 생성
    // 단 하나의 인스턴스만을 가지며 어디서든 접근 가능
    // 접근시 PlayerInfo.player_info.변수로 접근
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // player_info가 유일한 인스턴스
        if (playerInfo == null) playerInfo = this;
        // player_info 인스턴스가 이게 아니라면, 다른 인스턴스 삭제
        else if (playerInfo != this) Destroy(gameObject);
    }
}
