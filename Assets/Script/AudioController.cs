using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioClip[] clips; // Dorm, Hack, Main, MiniGame
    public AudioSource audioSource;
    private Slider volumeSlider;

    // 시작 시 씬 이름에 따라 BGM 설정
    List<string> mainBGMList = new List<string> {"CharacterCreateScene", "MainScene", "ClosetScene", "MainChatScene", "ProfileScene", "FriendScene" };
    List<string> dormBGMList = new List<string> {"DormScene"};
    List<string> miniGameBGMList = new List<string> {"MiniGameScene"};
    void Awake()
    {
        // 시작 시 VolumeSlider 찾기
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
    }
    void Start()
    {
        // 시작 시 씬 이름에 따라 BGM 설정
        if (mainBGMList.Contains(SceneManager.GetActiveScene().name)) audioSource.clip = clips[2];
        else if (dormBGMList.Contains(SceneManager.GetActiveScene().name)) audioSource.clip = clips[0];
        else if (miniGameBGMList.Contains(SceneManager.GetActiveScene().name)) audioSource.clip = clips[3];
        else audioSource.clip = null;

        // 씬 시작시 저장된 볼륨 값을 가져오기
        audioSource.volume = PlayerInfo.playerInfo.volume;
        volumeSlider.value = PlayerInfo.playerInfo.volume;

        // 슬라이더의 값에 따라 배경음악의 볼륨을 조정하는 이벤트를 추가
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float volume)
    {
        // 슬라이더의 값(0-1)을 배경음악의 볼륨 범위(0-1)에 맞게 조정
        audioSource.volume = volume;
        // 볼륨 값 저장
        PlayerInfo.playerInfo.volume = volume;
    }
}
