using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo playerInfo;
    public string nickname;
    public string email;
    public string access_token; // Cognito �α��ν� �޴� ��ū
    public int hatCustom = 8; // ĳ���� Ŀ���͸���¡ hat ���� ����, �⺻�� = �ƹ��͵� ���� none�̹��� 8
    public int eyeCustom = 11; // ĳ���� Ŀ���͸���¡ eye ���� ����, �⺻�� = �ƹ��͵� ���� none�̹��� 11
    public Dictionary<string, List<Vector3>> funiturePos = new Dictionary<string, List<Vector3>>(); // �� Ŀ���͸���¡ ���� ���� �� ��ġ ����
    public int characterType = 0; // ĳ���� Ÿ��
    public float volume = 0.5f; // BGM ����
    
    // PlayerInfo Ŭ������ �̱� ������ ����
    // �� �ϳ��� �ν��Ͻ����� ������ ��𼭵� ���� ����
    // ���ٽ� PlayerInfo.player_info.������ ����
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // player_info�� ������ �ν��Ͻ�
        if (playerInfo == null) playerInfo = this;
        // player_info �ν��Ͻ��� �̰� �ƴ϶��, �ٸ� �ν��Ͻ� ����
        else if (playerInfo != this) Destroy(gameObject);
    }
}
