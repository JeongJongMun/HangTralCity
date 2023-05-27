using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManage : MonoBehaviour
{
    // UI
    public GameObject settingBtn, exitBtn, _settingBtn, lobbyBtn, rankingBtn, startBtn, settingPanel;
    // UI

    // lobby, playing, ending
    static public string mode = "lobby";

    public GameObject lobby, playing, ending;

    public GameObject penalty, penaltyScore, finalPenaltyScore, time; 
    public int timer = 60;
    private float count = 0;

    bool isWhite = true;

    public static bool isTimeOut = false;

    public GameObject yellowTile;
    float spawnTimer = 0;


    void Start()
    {
        // UI
        settingBtn.GetComponent<Button>().onClick.AddListener(ClickSettingBtn);
        exitBtn.GetComponent<Button>().onClick.AddListener(ClickExitBtn);
        _settingBtn.GetComponent<Button>().onClick.AddListener(ClickSoundBtn);
        lobbyBtn.GetComponent<Button>().onClick.AddListener(ClickExitBtn);
        rankingBtn.GetComponent<Button>().onClick.AddListener(ClickRankBtn);
        startBtn.GetComponent<Button>().onClick.AddListener(ClickStartBtn);
        exitBtn.SetActive(false);
        _settingBtn.SetActive(false);
        settingPanel.SetActive(false);
        // UI

        time.GetComponent<TMP_Text>().text = "남은시간 : " + timer.ToString() + "초";
    }

    void Update()
    {
        if (mode == "lobby")
        {
            // 시작하기 버튼 클릭시 playing으로 전환
            lobby.SetActive(true);
            playing.SetActive(false);
            ending.SetActive(false);
        }
        else if (mode == "playing")
        {
            // TimeOut 시 ending으로 전환
            lobby.SetActive(false);
            playing.SetActive(true);
            ending.SetActive(false);
            TileSpawn();
            SetTimer();
        }
        else if (mode == "ending")
        {
            lobby.SetActive(false);
            playing.SetActive(false);
            ending.SetActive(true);
        }

    }
    void TimeOut()
    {
        mode = "ending";
        isTimeOut = true;
        finalPenaltyScore.GetComponent<TMP_Text>().text = "Penalty : " + PenaltyCount.GetPenalty();

        ending.SetActive(true);
    }

    public static bool GetFinishedFlag()
    {
        return isTimeOut;
    }
    void SetTimer()
    {
        if (count > 1 && timer > 11)
        {
            timer -= 1;
            time.GetComponent<TMP_Text>().text = "남은시간 : " + timer.ToString() + "초";
            count = 0;
        }
        else if (count > 1 && timer < 12 && timer > 0)
        {
            timer -= 1;
            time.GetComponent<TMP_Text>().text = "남은시간 : " + timer.ToString() + "초";
            count = 0;
            if (isWhite == true)
            {
                time.GetComponent<TMP_Text>().color = Color.red;
                isWhite = false;
            }
            else if (isWhite == false)
            {
                time.GetComponent<TMP_Text>().color = Color.white;
                isWhite = true;
            }
        }
        else
        {
            count += Time.deltaTime;
        }

        if (timer == 0) TimeOut();
    }
    void TileSpawn()
    {
        if (GetFinishedFlag() == false)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > 0.1f)
            {
                GameObject newTile = Instantiate(yellowTile);
                newTile.transform.position = new Vector2(Random.Range(-15f, 10f), 21);
                spawnTimer = 0;
                Destroy(newTile, 5.0f);
            }
        }
    }

    // UI
    void ClickStartBtn()
    {
        mode = "playing";
    }

    void ClickExitBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    void ClickSoundBtn()
    {
        if (settingPanel.activeSelf) settingPanel.SetActive(false);
        else settingPanel.SetActive(true);
    }
    void ClickRankBtn()
    {
        // 랭킹 버튼 클릭시
    }

    // 설정 버튼 클릭시
    void ClickSettingBtn()
    {
        if (!exitBtn.activeSelf && !_settingBtn.activeSelf)
        {
            exitBtn.SetActive(true);
            _settingBtn.SetActive(true);
        }
        else
        {
            exitBtn.SetActive(false);
            _settingBtn.SetActive(false);
        }
    }
    // UI
}
