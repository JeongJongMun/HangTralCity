using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManage : MonoBehaviour
{
    // Canvas의 TopBar, BottomBar UI 관리
    private TMP_Text nickname;
    private Button home_btn, closet_btn, setting_btn, logout_btn, chat_btn;
    private GameObject setting_content;
    void Start()
    {
        //nickname.text = PlayerInfo.player_info.nickname;

        home_btn = GameObject.Find("HomeBtn").GetComponent<Button>();
        home_btn.onClick.AddListener(ClickHomeBtn);
        Debug.Log("STARATWQTQTQ");

        closet_btn = GameObject.Find("ClosetBtn").GetComponent<Button>();
        closet_btn.onClick.AddListener(ClickClosetBtn);

        setting_btn = GameObject.Find("SettingBtn").GetComponent<Button>();
        setting_btn.onClick.AddListener(ClickSettingBtn);

        chat_btn = GameObject.Find("ChatBtn").GetComponent<Button>();
        chat_btn.onClick.AddListener(ClickChatBtn);


        logout_btn = GameObject.Find("LogOutBtn").GetComponent<Button>();
        logout_btn.onClick.AddListener(ClickLogOutBtn);

        setting_content = GameObject.Find("SettingContent");
        setting_content.SetActive(false);

    }
    private void ClickSettingBtn()
    {
        Debug.Log("SettingBtn Clicked");

        if (setting_content.activeSelf) setting_content.SetActive(false);
        else setting_content.SetActive(true);
    }
    private void ClickLogOutBtn()
    {
        Debug.Log("LogoutBtn Clicked");
        // 로그아웃시 자동 로그인된 정보를 삭제
        PlayerPrefs.DeleteKey("LastLoggedInUserId");
        PlayerPrefs.DeleteKey("LastLoggedInPassword");
        SceneManager.LoadScene("SignInScene");
    }

    private void ClickClosetBtn()
    {
        Debug.Log("ClosetBtn Clicked");
        SceneManager.LoadScene("ClosetScene");
    }
    private void ClickHomeBtn()
    {
        Debug.Log("HomeBtn Clicked");
        SceneManager.LoadScene("MainScene");
    }

    private void ClickChatBtn()
    {
        Debug.Log("ChatBtn Clicked");
        if (SceneManager.GetActiveScene().name != "MainChatScene")
        {
            SceneManager.LoadScene("MainChatScene");
        }
        
        
    }
}
