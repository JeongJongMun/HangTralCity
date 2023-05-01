using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingManage : MonoBehaviour
{
    public Button lobby;

    public TMP_Text[] ranking;

    private void Start()
    {
        lobby.GetComponent<Button>().onClick.AddListener(EnterLobby);        
    }

    public void EnterLobby()
    {
        SceneManager.LoadScene("MainScene");
    }

    //TODO Dynamo DB에서 랭킹정보를 받아서 순위에 삽입
}
