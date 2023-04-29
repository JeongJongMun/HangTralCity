using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManage : MonoBehaviour
{
    public TMP_Text nickname;
    public Button setting_btn, customize_btn, home_btn, gang_btn, dorm_btn;
    private void Start()
    {
        nickname.text = PlayerInfo.player_info.nickname;

        setting_btn.GetComponent<Button>().onClick.AddListener(Click_Setting_Btn);
        customize_btn.GetComponent<Button>().onClick.AddListener(Click_Customize_Btn);
        home_btn.GetComponent<Button>().onClick.AddListener(Click_Home_Btn);
        gang_btn.GetComponent<Button>().onClick.AddListener(Click_Gang_Btn);
        dorm_btn.GetComponent<Button>().onClick.AddListener(Click_Dorm_Btn);
    }

    public void Click_Setting_Btn()
    {
        SceneManager.LoadScene("LogInScene");
    }

    public void Click_Customize_Btn()
    {
        SceneManager.LoadScene("CustomizeScene");
    }
    public void Click_Home_Btn()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Click_Gang_Btn()
    {
        SceneManager.LoadScene("GangScene");
    }
    public void Click_Dorm_Btn()
    {
        SceneManager.LoadScene("DormScene");
    }
}
