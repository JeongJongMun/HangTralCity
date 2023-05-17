using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class UIManage : MonoBehaviour
{
    // Canvas의 TopBar, BottomBar UI 관리
    private Button homeBtn, closetBtn, settingBtn, logoutBtn, chatBtn, friendBtn, profileBtn, settingCloseBtn, arBtn;
    private GameObject settingPanel;
    void Start()
    {

        homeBtn = GameObject.Find("HomeBtn").GetComponent<Button>();
        homeBtn.onClick.AddListener(ClickHomeBtn);

        closetBtn = GameObject.Find("ClosetBtn").GetComponent<Button>();
        closetBtn.onClick.AddListener(ClickClosetBtn);

        settingBtn = GameObject.Find("SettingBtn").GetComponent<Button>();
        settingBtn.onClick.AddListener(ClickSettingBtn);

        chatBtn = GameObject.Find("ChatBtn").GetComponent<Button>();
        chatBtn.onClick.AddListener(ClickChatBtn);


        logoutBtn = GameObject.Find("LogOutBtn").GetComponent<Button>();
        logoutBtn.onClick.AddListener(ClickLogOutBtn);

        settingPanel = GameObject.Find("SettingPanel");
        settingCloseBtn = GameObject.Find("SettingCloseBtn").GetComponent<Button>();
        settingCloseBtn.onClick.AddListener(ClickSettingBtn);
        settingPanel.SetActive(false);

        friendBtn = GameObject.Find("FriendBtn").GetComponent<Button>();
        friendBtn.onClick.AddListener(ClickFriendBtn);

        profileBtn = GameObject.Find("ProfileBtn").GetComponent<Button>();
        profileBtn.onClick.AddListener(ClickProfileBtn);

        arBtn = GameObject.Find("ARBtn").GetComponent<Button>();
        arBtn.onClick.AddListener(ClickARBtn);


    }
    void ClickSettingBtn()
    {
        if (settingPanel.activeSelf) settingPanel.SetActive(false);
        else settingPanel.SetActive(true);
    }
    void ClickLogOutBtn()
    {
        // 로그아웃시 자동 로그인된 정보를 삭제
        PlayerPrefs.DeleteKey("LastLoggedInUserId");
        PlayerPrefs.DeleteKey("LastLoggedInPassword");
        SceneManager.LoadScene("SignInScene");
    }

    void ClickClosetBtn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("ClosetScene");
    }
    void ClickHomeBtn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainScene");
    }

    void ClickChatBtn()
    {
        if (SceneManager.GetActiveScene().name != "MainChatScene") SceneManager.LoadScene("MainChatScene");
    }
    void ClickFriendBtn()
    {
        SceneManager.LoadScene("FriendScene");
    }
    void ClickProfileBtn()
    {
        SceneManager.LoadScene("ProfileScene");
    }
    void ClickARBtn()
    {
        SceneManager.LoadScene("ARScene");
    }
}
