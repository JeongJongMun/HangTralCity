using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreateManage : MonoBehaviour
{
    public Button camera_btn, create_btn;
    void Start()
    {
        create_btn.onClick.AddListener(Click_CreateBtn);
        camera_btn.onClick.AddListener(Click_CameraBtn);
    }
    public void Click_CreateBtn() {
        SceneManager.LoadScene("MainScene");
    }
    public void Click_CameraBtn() {
        SceneManager.LoadScene("CameraScene");
    }
    public void Click_LeftBtn() {
        Debug.Log("Clicked LeftBtn");
    }
    public void Click_RightBtn() {
        Debug.Log("Clicked RightBtn");
    }
}
