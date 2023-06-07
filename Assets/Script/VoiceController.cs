using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using UnityEngine.SceneManagement;

public class VoiceController : MonoBehaviour
{
    public Recorder recorder;
    Toggle voiceToggle;

    void Start()
    {
        // 마이크 토글 버튼이 있는 강의동과 운동장에서만 작동
        if (SceneManager.GetActiveScene().name == "GangScene" || SceneManager.GetActiveScene().name == "MiniGameScene")
        {
            voiceToggle = GameObject.Find("VoiceToggle").GetComponent<Toggle>();
            voiceToggle.onValueChanged.AddListener(delegate { ToggleMicrophone(); });
        }
    }
    public void ToggleMicrophone()
    {
        if (voiceToggle.isOn) recorder.TransmitEnabled = true; // 마이크 켜기
        else recorder.TransmitEnabled = false; // 마이크 끄기
    }

}
