using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManage : MonoBehaviour
{
    public Button ranking;
    public Button main;

    void Start()
    {
        ranking.GetComponent<Button>().onClick.AddListener(EnterRanking);
        main.GetComponent<Button>().onClick.AddListener(EnterMain);
    }

    public void EnterRanking()
    {
        SceneManager.LoadScene("RankingScene");
    }

    public void EnterMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}
