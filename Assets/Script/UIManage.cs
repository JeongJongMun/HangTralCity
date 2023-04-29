using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManage : MonoBehaviour
{
    public TMP_Text nickname;
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
    private void Start()
    {
        nickname.text = PlayerInfo.player_info.nickname;
    }
}
