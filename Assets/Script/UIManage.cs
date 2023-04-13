using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManage : MonoBehaviour
{
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
}
