using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManage : MonoBehaviour
{

    public GameObject menuBar;
    private bool isMenuBtnClicked;
    private void FixedUpdate()
    {
        if (isMenuBtnClicked)
        {
            Vector3 speed = Vector3.zero;
            menuBar.transform.position = Vector3.SmoothDamp(menuBar.transform.position, new Vector3(540 + 190, 2340/2, 0), ref speed, 0.1f);
        }
        else if (isMenuBtnClicked == false)
        {
            Vector3 speed = Vector3.zero;
            menuBar.transform.position = Vector3.SmoothDamp(menuBar.transform.position, new Vector3(1080 + 540 - 190, 2340 / 2, 0), ref speed, 0.1f);
        }
        if (Input.GetMouseButtonUp(0)) isMenuBtnClicked = false;
    }
    public void Click_Setting_Btn() {
        SceneManager.LoadScene("LogInScene");
    }
    public void Click_Menu_Btn()
    {
        isMenuBtnClicked = true;
    }
}
