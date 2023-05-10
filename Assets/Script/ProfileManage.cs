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
        // �г���, �̸��� �ҷ�����
        nickname.text = PlayerInfo.playerInfo.nickname;
        email.text = PlayerInfo.playerInfo.email;
        // ĳ���� ���� ��ư Ŭ�� ��
        characterChangeBtn.onClick.AddListener(ClickCharacterChangeBtn);
    }

    void ClickCharacterChangeBtn()
    {
        SceneManager.LoadScene("CharacterCreateScene");
    }
}
