using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManage : MonoBehaviour
{
    public Button ranking;
    public Button main;

    public Button setting;

    public Button settingMain, settingSound;
    public GameObject settingMAIN, settingSOUND;

    bool settingButtonActive = false;

    void Start()
    {
        ranking.GetComponent<Button>().onClick.AddListener(EnterRanking);
        main.GetComponent<Button>().onClick.AddListener(EnterMain);

        settingMain.GetComponent<Button>().onClick.AddListener(EnterMain);
        settingSound.GetComponent<Button>().onClick.AddListener(EnterSoundSetting);

        if (settingButtonActive == false)
        {
            setting.GetComponent<Button>().onClick.AddListener(SettingActiveTrue);
            settingButtonActive = true;
        }
        else
        {
            setting.GetComponent<Button>().onClick.AddListener(SettingActiveTrue);
            settingButtonActive = false;
        }

    }

    public void EnterRanking()
    {
        SceneManager.LoadScene("RankingScene");
    }

    public void EnterMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void EnterSoundSetting()
    {
        SceneManager.LoadScene("SoundSettingScene");
    }

    public void SettingActiveTrue()
    {
        settingMAIN.SetActive(true);
        settingSOUND.SetActive(true);
    }

    public void SettingActiveFalse()
    {
        settingMAIN.SetActive(false);
        settingSOUND.SetActive(false);
    }
}
