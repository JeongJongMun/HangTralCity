using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public class MiniGameManage : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject exitBtn;
    public GameObject startBtn;

    [Header("Mode")]
    public GameObject lobby;
    public GameObject playing;
    public GameObject ending;

    // lobby, playing, ending
    static public string mode = "lobby";
    [Header("Penalty")]
    public GameObject penaltyTxt;
    public int penaltyScore = 0;

    [Header("ETC")]
    public GameObject timer; 
    
    int time = 60;
    float second = 0;
    PhotonView PV;
    float spawnTimer = 0;


    void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
        startBtn.GetComponent<Button>().onClick.AddListener(ClickStartBtn);

        timer.GetComponent<TMP_Text>().text = "남은시간 : " + time.ToString() + "초";
    }

    void Update()
    {
        if (mode == "lobby") PV.RPC("LobbyMode", RpcTarget.All);
        else if (mode == "playing") PV.RPC("PlayingMode", RpcTarget.All);
        else if (mode == "ending") PV.RPC("EndingMode", RpcTarget.All);

    }
    [PunRPC]
    void LobbyMode()
    {
        lobby.SetActive(true);
        playing.SetActive(false);
        ending.SetActive(false);
    }
    [PunRPC]
    void PlayingMode()
    {
        // TimeOut 시 ending으로 전환
        lobby.SetActive(false);
        playing.SetActive(true);
        ending.SetActive(false);
        penaltyTxt.GetComponent<TMP_Text>().text = penaltyScore.ToString();
        SetTimer();

        // 마스터 클라이언트인 경우 피트 스폰, 시간 흐름 관리
        if (PhotonNetwork.IsMasterClient)
        {
            FeetSpawn();
            TimeFlow();
            PV.RPC("SyncTime", RpcTarget.All, time);
        }
    }
    [PunRPC]
    void EndingMode()
    {
        lobby.SetActive(true);
        playing.SetActive(true);
        ending.SetActive(true);
    }

    void SetTimer()
    {
        timer.GetComponent<TMP_Text>().text = "남은시간 : " + time.ToString() + "초";

        if (0 < time && time < 10)
        {
            if (time % 2 == 0) timer.GetComponent<TMP_Text>().color = Color.red;
            if (time % 2 == 1) timer.GetComponent<TMP_Text>().color = Color.white;
        }
        else if (time == 0)
        {
            mode = "ending";
            ending.SetActive(true);
        }
    }
    void TimeFlow()
    {
        if (second > 1)
        {
            time -= 1;
            second = 0;
        }
        else second += Time.deltaTime;
    }
    [PunRPC]
    void SyncTime(int masterTime)
    {
        // 마스터 클라이언트가 보낸 시간을 동기화
        time = masterTime;
    }
    void FeetSpawn()
    {
        if (mode == "playing")
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > 0.5f)
            {
                GameObject target = PhotonNetwork.Instantiate("Feet", new Vector2(Random.Range(-9, 9), gameObject.transform.position.y), transform.rotation);
                spawnTimer = 0;
                StartCoroutine(DestroyAfter(target, 3f));

            }
        }
    }
    IEnumerator DestroyAfter(GameObject target, float delay)
    {
        // Delay 만큼 대기
        yield return new WaitForSeconds(delay);

        // target이 파괴되지 않았으면 파괴 실행
        if (target != null) PhotonNetwork.Destroy(target);
    }
    public void AddPenaltyScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 패널티 ++;
            penaltyScore++;
            // 변경된 패널티 점수를 모든 플레이어에게 전송
            photonView.RPC("UpdatePenaltyScoreRPC", RpcTarget.All, penaltyScore);
        }
    }
    [PunRPC]
    private void UpdatePenaltyScoreRPC(int updatedScore)
    {
        penaltyScore = updatedScore;
    }

    void ClickStartBtn()
    {
        PV.RPC("SwitchToPlayingMode", RpcTarget.All);
    }
    [PunRPC]
    void SwitchToPlayingMode()
    {
        penaltyScore = 0;
        time = 60;
        mode = "playing";
    }
}
