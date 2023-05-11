using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;


public class VoiceController : MonoBehaviour
{
    public Recorder recorder;
    public Toggle voiceToggle;

    void Start()
    {
        voiceToggle.onValueChanged.AddListener(delegate { ToggleMicrophone(); });
    }
    public void ToggleMicrophone()
    {
        if (voiceToggle.isOn) recorder.TransmitEnabled = true; // 마이크 켜기
        else recorder.TransmitEnabled = false; // 마이크 끄기
    }

}
