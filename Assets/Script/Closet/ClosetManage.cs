using UnityEngine;
using UnityEngine.UI;

public class ClosetManage : MonoBehaviour
{
    public GameObject eyeScrollView, hatScrollView; // hat, eye ��ũ�� �� ������Ʈ
    public Button eyeScrollViewBtn, hatScrollViewBtn; // hat, eye ��ũ�� �� ���� ��ư
    public GameObject[] eyeToggle, hatToggle; // hat, eye ī�װ��� Ŀ�� ���� ��۵�
    public GameObject character;

    void Start()
    {
        // ��ũ�Ѻ� ���� ��ư Ŭ����
        eyeScrollViewBtn.onClick.AddListener(ClickEyeCategory);
        hatScrollViewBtn.onClick.AddListener(ClickHatCategory);

        // ��� Ŭ���� �Լ� ȣ��
        for (int i = 0; i < eyeToggle.Length; i++)
        {
            int index = i;
            eyeToggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleEye(index); });
        }
        for (int i = 0; i < hatToggle.Length; i++)
        {
            int index = i;
            hatToggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleHat(index); });
        }
    }

    private void ToggleEye(int n)
    {
        if (eyeToggle[n].GetComponent<Toggle>().isOn)
        {
            // �ٸ� ��۵� OFF
            for (int i = 0; i < eyeToggle.Length; i++) if (i != n) eyeToggle[i].GetComponent<Toggle>().isOn = false;
            // ���� ����
            PlayerInfo.playerInfo.eyeCustom = n;
            // ������ ��� �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            // null �̹����� ����
            PlayerInfo.playerInfo.eyeCustom = 11;
            // �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
    }
    private void ToggleHat(int n)
    {
        if (hatToggle[n].GetComponent<Toggle>().isOn)
        {
            // �ٸ� ��۵� OFF
            for (int i = 0; i < hatToggle.Length; i++) if (i != n) hatToggle[i].GetComponent<Toggle>().isOn = false;
            // ���� ����
            PlayerInfo.playerInfo.hatCustom = n;
            // ������ ��� �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            // null �̹����� ����
            PlayerInfo.playerInfo.hatCustom = 8;
            // �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
    }
    private void ClickEyeCategory()
    {
        eyeScrollView.SetActive(true);
        hatScrollView.SetActive(false);
    }
    private void ClickHatCategory()
    {
        eyeScrollView.SetActive(false);
        hatScrollView.SetActive(true);
    }
}
