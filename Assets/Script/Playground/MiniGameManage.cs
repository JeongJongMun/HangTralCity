using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    public int penalty = 0;

    [Header("ETC")]
    public GameObject timer; 
    public int time = 60;
    float second = 0;
    PhotonView PV;


    bool isWhite = true;

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
        penaltyTxt.GetComponent<TMP_Text>().text = penalty.ToString();
        FeetSpawn();
        SetTimer();
    }
    [PunRPC]
    void EndingMode()
    {
        lobby.SetActive(true);
        playing.SetActive(false);
        ending.SetActive(true);
    }

    void SetTimer()
    {
        if (second > 1)
        {
            time -= 1;
            timer.GetComponent<TMP_Text>().text = "남은시간 : " + time.ToString() + "초";
            second = 0;

            if (time < 12 && time > 0)
            {
                timer.GetComponent<TMP_Text>().color = isWhite ? Color.red : Color.white;
                isWhite = !isWhite;
            }
        }
        else second += Time.deltaTime;


        if (time == 0)
        {
            mode = "ending";
            ending.SetActive(true);
        }
    }
    void FeetSpawn()
    {
        if (mode == "playing")
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > 0.5f)
            {
                GameObject newFeet = PhotonNetwork.Instantiate("Feet", new Vector2(Random.Range(-15f, 10f), 21), transform.rotation);
                spawnTimer = 0;
                Destroy(newFeet, 3.0f);
            }
        }
    }
    void ClickStartBtn()
    {
        PV.RPC("SwitchToPlayingMode", RpcTarget.All);
    }
    [PunRPC]
    void SwitchToPlayingMode()
    {
        penalty = 0;
        time = 60;
        mode = "playing";
    }
}
