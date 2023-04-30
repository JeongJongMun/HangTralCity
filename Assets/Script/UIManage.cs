using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManage : MonoBehaviour
{
    private TMP_Text nickname;
    private Button setting_btn, customize_btn, home_btn;
    private void Start()
    {
        nickname.text = PlayerInfo.player_info.nickname;

        setting_btn = GameObject.Find("SettingBtn").GetComponent<Button>();
        customize_btn = GameObject.Find("CustomBtn").GetComponent<Button>();
        home_btn = GameObject.Find("HomeBtn").GetComponent<Button>();

        setting_btn.onClick.AddListener(Click_Setting_Btn);
        customize_btn.onClick.AddListener(Click_Customize_Btn);
        home_btn.onClick.AddListener(Click_Home_Btn);
    }

    public void Click_Setting_Btn()
    {
        SceneManager.LoadScene("SignInScene");
    }

    public void Click_Customize_Btn()
    {
        SceneManager.LoadScene("CustomizeScene");
    }
    public void Click_Home_Btn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
