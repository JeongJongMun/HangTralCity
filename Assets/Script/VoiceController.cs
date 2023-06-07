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
        // ����ũ ��� ��ư�� �ִ� ���ǵ��� ��忡���� �۵�
        if (SceneManager.GetActiveScene().name == "GangScene" || SceneManager.GetActiveScene().name == "MiniGameScene")
        {
            voiceToggle = GameObject.Find("VoiceToggle").GetComponent<Toggle>();
            voiceToggle.onValueChanged.AddListener(delegate { ToggleMicrophone(); });
        }
    }
    public void ToggleMicrophone()
    {
        if (voiceToggle.isOn) recorder.TransmitEnabled = true; // ����ũ �ѱ�
        else recorder.TransmitEnabled = false; // ����ũ ����
    }

}
