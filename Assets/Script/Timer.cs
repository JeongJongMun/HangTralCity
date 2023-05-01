using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text finalPenalty;

    public Button lobby;
    public Button ranking;

    public GameObject text;
    public GameObject penaltyText;
    public GameObject timeRemainingText;
    public GameObject timeoverText;
    public GameObject finalPenaltyText;

    public GameObject lobbyBtn;
    public GameObject rankingBtn;

    public int timer = 60;
    private float count = 0;

    bool isWhite = true;

    public static bool finishedFlag = false;

    void Start()
    {
        timerText.text = "남은시간 : " + timer.ToString() + "초";
    }

    void Update()
    {
        if (count > 1 && timer > 11)
        {
            timer -= 1;
            timerText.text = "남은시간 : " + timer.ToString() + "초";
            count = 0;
        }
        else if (count > 1 && timer < 12 && timer > 0)
        {
            timer -= 1;
            timerText.text = "남은시간 : " + timer.ToString() + "초";
            count = 0;
            if (isWhite == true)
            {
                timerText.color = Color.red;
                isWhite = false;
            }
            else if (isWhite == false)
            {
                timerText.color = Color.white;
                isWhite = true;
            }
        }
        else
        {
            count += Time.deltaTime;
        }

        if (timer == 0)
        {
            finishedFlag = true;
            finalPenalty.text = "penalty : " + PenaltyCount.GetPenalty();

            text.SetActive(false);
            penaltyText.SetActive(false);
            timeRemainingText.SetActive(false);
            timeoverText.SetActive(true);
            finalPenaltyText.SetActive(true);
            lobbyBtn.SetActive(true);
            rankingBtn.SetActive(true);
        }
    }

    public static bool GetFinishedFlag()
    {
        return finishedFlag;
    }
}
