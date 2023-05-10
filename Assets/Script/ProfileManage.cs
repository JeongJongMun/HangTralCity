using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProfileManage : MonoBehaviour
{
    public TMP_Text nickname, email;
    public Button characterChangeBtn;

    void Start()
    {
        // 닉네임, 이메일 불러오기
        nickname.text = PlayerInfo.playerInfo.nickname;
        email.text = PlayerInfo.playerInfo.email;
        // 캐릭터 변경 버튼 클릭 시
        characterChangeBtn.onClick.AddListener(ClickCharacterChangeBtn);
    }

    void ClickCharacterChangeBtn()
    {
        SceneManager.LoadScene("CharacterCreateScene");
    }
}
