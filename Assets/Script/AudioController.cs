using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioClip[] clips; // Dorm, Hack, Main, MiniGame
    public AudioSource audioSource;
    private Slider volumeSlider;

    // ���� �� �� �̸��� ���� BGM ����
    List<string> mainBGMList = new List<string> {"CharacterCreateScene", "MainScene", "ClosetScene", "MainChatScene", "ProfileScene", "FriendScene" };
    List<string> dormBGMList = new List<string> {"DormScene"};
    List<string> miniGameBGMList = new List<string> {"MiniGameScene"};
    void Awake()
    {
        // ���� �� VolumeSlider ã��
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
    }
    void Start()
    {
        // ���� �� �� �̸��� ���� BGM ����
        if (mainBGMList.Contains(SceneManager.GetActiveScene().name)) audioSource.clip = clips[2];
        else if (dormBGMList.Contains(SceneManager.GetActiveScene().name)) audioSource.clip = clips[0];
        else if (miniGameBGMList.Contains(SceneManager.GetActiveScene().name)) audioSource.clip = clips[3];
        else audioSource.clip = null;

        // �� ���۽� ����� ���� ���� ��������
        audioSource.volume = PlayerInfo.playerInfo.volume;
        volumeSlider.value = PlayerInfo.playerInfo.volume;

        // �����̴��� ���� ���� ��������� ������ �����ϴ� �̺�Ʈ�� �߰�
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float volume)
    {
        // �����̴��� ��(0-1)�� ��������� ���� ����(0-1)�� �°� ����
        audioSource.volume = volume;
        // ���� �� ����
        PlayerInfo.playerInfo.volume = volume;
    }
}
