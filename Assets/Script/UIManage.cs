using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class UIManage : MonoBehaviour
{
    // Canvas의 TopBar, BottomBar UI 관리
    private Button home_btn, closet_btn, setting_btn, logout_btn, chat_btn, friendBtn, profileBtn;
    private GameObject setting_content;
    void Start()
    {

        home_btn = GameObject.Find("HomeBtn").GetComponent<Button>();
        home_btn.onClick.AddListener(ClickHomeBtn);

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

        friendBtn = GameObject.Find("FriendBtn").GetComponent<Button>();
        friendBtn.onClick.AddListener(ClickFriendBtn);

        profileBtn = GameObject.Find("ProfileBtn").GetComponent<Button>();
        profileBtn.onClick.AddListener(ClickProfileBtn);

    }
    private void ClickSettingBtn()
    {
        if (setting_content.activeSelf) setting_content.SetActive(false);
        else setting_content.SetActive(true);
    }
    private void ClickLogOutBtn()
    {
        // 로그아웃시 자동 로그인된 정보를 삭제
        PlayerPrefs.DeleteKey("LastLoggedInUserId");
        PlayerPrefs.DeleteKey("LastLoggedInPassword");
        SceneManager.LoadScene("SignInScene");
    }

    private void ClickClosetBtn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("ClosetScene");
    }
    private void ClickHomeBtn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainScene");
    }

    private void ClickChatBtn()
    {
        if (SceneManager.GetActiveScene().name != "MainChatScene") SceneManager.LoadScene("MainChatScene");
    }
    private void ClickFriendBtn()
    {
        SceneManager.LoadScene("FriendScene");
    }
    private void ClickProfileBtn()
    {
        SceneManager.LoadScene("ProfileScene");
    }
}
