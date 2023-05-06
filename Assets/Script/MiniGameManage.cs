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

    void Start()
    {
        setting.onClick.AddListener(ActiveSetting);
       
        ranking.onClick.AddListener(EnterRanking);
        main.onClick.AddListener(EnterMain);

        settingMain.onClick.AddListener(EnterMain);
        settingSound.onClick.AddListener(EnterSoundSetting);

        settingMAIN.SetActive(false);
        settingSOUND.SetActive(false);
    }



    private void EnterRanking()
    {
        SceneManager.LoadScene("RankingScene");
    }

    private void EnterMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void EnterSoundSetting()
    {
        SceneManager.LoadScene("SoundSettingScene");
    }

    private void ActiveSetting()
    {
        if (!settingMAIN.activeSelf)
        {
            settingMAIN.SetActive(true);
            settingSOUND.SetActive(true);
        }
        else
        {
            settingMAIN.SetActive(false);
            settingSOUND.SetActive(false);
        }
    }
}
