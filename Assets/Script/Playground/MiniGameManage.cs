using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManage : MonoBehaviour
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

    [Header("SpawnObject")]
    public GameObject feet;

    [Header("ETC")]
    public GameObject timer; 
    public int time = 60;
    float second = 0;


    bool isWhite = true;

    float spawnTimer = 0;


    void Start()
    {
        startBtn.GetComponent<Button>().onClick.AddListener(ClickStartBtn);

        timer.GetComponent<TMP_Text>().text = "남은시간 : " + time.ToString() + "초";
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
            penaltyTxt.GetComponent<TMP_Text>().text = penalty.ToString();
            FeetSpawn();
            SetTimer();
        }
        else if (mode == "ending")
        {
            lobby.SetActive(true);
            playing.SetActive(false);
            ending.SetActive(true);
        }

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

            if (spawnTimer > 0.3f)
            {
                GameObject newTile = Instantiate(feet);
                newTile.transform.position = new Vector2(Random.Range(-15f, 10f), 21);
                spawnTimer = 0;
                Destroy(newTile, 5.0f);
            }
        }
    }
    void ClickStartBtn()
    {
        penalty = 0;
        time = 60;
        mode = "playing";
    }
}
