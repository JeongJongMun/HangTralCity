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

    //TODO Dynamo DB���� ��ŷ������ �޾Ƽ� ������ ����
}
