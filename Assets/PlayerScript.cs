using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public PhotonView PV;

    AudioSource dooropenEffect;

    Vector3 curPos;

    private float playerScale = 0.1f; // 플레이어 크기 비율
    private float speed = 10f; // 플레이어 이동 속도

    // 캐릭터 타입 변경 변수
    private SpriteRenderer SR;
    private Animator AN;

    private List<string> animators = new List<string> {"puppy", "cat", "bear", "dino", "rabbit" }; 
    private List<string> sprites = new List<string> {"puppy_ridle", "cat_ridle", "bear_ridle", "dino_ridle", "rabbit_ridle" }; 

    // 캐릭터 커스텀 변경 변수
    public GameObject hatPoint;
    public GameObject eyePoint;

    // 캐릭터 커스텀 에셋 리스트
    public Sprite[] hatSprites;
    public Sprite[] eyeSprites;

    // 캐릭터 닉네임 변경 변수
    public GameObject nickNameTxt;
    public GameObject nickNamePoint;

    // 나가기 버튼
    private GameObject exitBtn;


    public GameObject ChatPoint;
    public GameObject ChatText;
    private Button sendBtn; // 채팅 보내기 버튼 UI
    private InputField inputfield; // 채팅 입력창 UI


    //public GameObject chatBoxPrefab; // 말풍선 프리팹
    //public Transform chatTr; // 말풍선 위치

    void Awake()
    {
        //nameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickNameTxt.GetComponent<TMP_Text>().text = PV.IsMine? PhotonNetwork.NickName : PV.Owner.NickName;
        nickNameTxt.GetComponent<TMP_Text>().color = PV.IsMine ? Color.green : Color.blue;

        ChatText.GetComponent<TMP_Text>().text = "";

        RB = GetComponent<Rigidbody2D>();

        

        // 나가기 버튼 (강의동일 경우에만 찾음)
        if (SceneManager.GetActiveScene().name == "GangScene")
        {
            exitBtn = GameObject.Find("ExitBtn");
            exitBtn.GetComponent<Button>().onClick.AddListener(Exit);
            exitBtn.SetActive(false);
        }

        // if (서버 내에서 내 캐릭터 AND 현재 씬이 기숙사&&옷장&&프로필이 아님)
        if (PV.IsMine && SceneManager.GetActiveScene().name != "DormScene" && SceneManager.GetActiveScene().name != "ClosetScene" && SceneManager.GetActiveScene().name != "ProfileScene")
        {
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
        if (PV.IsMine)
        {
            AN = GetComponent<Animator>();
            SR = GetComponent<SpriteRenderer>();

            // 캐릭터 생성시 타입/커스텀/닉네임 설정
            AN.SetInteger("type", PlayerInfo.playerInfo.characterType);
            SetCharacterCustom();
            Chat();
            //SetCharacterName();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Chat()
    {
        if (SceneManager.GetActiveScene().name == "GangScene")
        {
            inputfield = GameObject.Find("ChatInputfield").GetComponent<InputField>();
            sendBtn = GameObject.Find("ChatSendBtn").GetComponent<Button>();
            sendBtn.onClick.AddListener(Send);
        }
    }

    public void Send()
    {
        string msg = GameObject.Find("ChatInputfield").GetComponent<InputField>().ToString();
        PV.RPC("ChatRPC", RpcTarget.All, inputfield.text);
        inputfield.text = "";
    }


    [PunRPC]
    void ChatRPC(string msg)
    {
        if (ChatText.GetComponent<TMP_Text>().text == "")
        {
            ChatText.GetComponent<TMP_Text>().text = msg;
        }
        else
        {
            ChatText.GetComponent<TMP_Text>().text = "";
            ChatText.GetComponent<TMP_Text>().text = msg;
        }
        Invoke("Delay", 5); // 2초뒤 0.3초주기로 LaunchProjectile함수 반복 호출
    }

    void Delay()
    {
        ChatText.GetComponent<TMP_Text>().text = "";
    }

    private void Move() {
        // if (서버 내에서 내 캐릭터 OR 현재 씬이 로컬 기숙사)
        if (PV.IsMine || SceneManager.GetActiveScene().name == "DormScene")
        {
            //움직이기
            float axis_x = Input.GetAxisRaw("Horizontal");
            float axis_y = Input.GetAxisRaw("Vertical");
            RB.velocity = new Vector2(speed * axis_x, speed * axis_y);
            RB.AddForce(RB.velocity * Time.deltaTime, ForceMode2D.Impulse);

            //걷기 애니메이션 설정
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) AN.SetBool("isWalking", true);
            else AN.SetBool("isWalking", false);

            //x축 반전
            if (axis_x < 0)
            {
                transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
                // 닉네임은 x축 반전 X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(-1, 1, 1);
                //채팅은 x축 반전 X
                Transform rr = ChatPoint.transform;
                rr.localScale = new Vector3(-1, 1, 1);
            }
            else if (axis_x > 0)
            {
                transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
                // 닉네임은 x축 반전 X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(1, 1, 1);
                //채팅은 x축 반전 X
                Transform rr = ChatPoint.transform;
                rr.localScale = new Vector3(1, 1, 1);
            }

        }
        //ismine이 아닌경우 위치동기화 (다른 사람이 움직이는걸 내가 볼 수 있게)
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // 멀리 떨어졌다면
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // 근처라면
    }

    // 문에 닿아있을 경우 나가기 버튼 활성화
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Door") exitBtn.SetActive(true);
    }
    // 문과 떨어질 경우 나가기 버튼 비활성화
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door") exitBtn.SetActive(false);
    }
    // 나가기 버튼 클릭시
    public void Exit()
    {
        SceneManager.LoadScene("MainScene");
        PhotonNetwork.Disconnect();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // 변수동기화
    {
        if (stream.IsWriting) stream.SendNext(transform.position);
        else curPos = (Vector3)stream.ReceiveNext();
    }

    public void SetCharacterCustom()
    {
        if (PV.IsMine)
        {
            Debug.LogFormat("플레이어 커스텀 모자:{0}, 눈:{1}", PlayerInfo.playerInfo.hatCustom, PlayerInfo.playerInfo.eyeCustom);
            hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[PlayerInfo.playerInfo.hatCustom];
            eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[PlayerInfo.playerInfo.eyeCustom];
        }
        else
        {
            Debug.Log("Not Is Mine");
            hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[PlayerInfo.playerInfo.hatCustom];
            eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[PlayerInfo.playerInfo.eyeCustom];
        }
        
    }
    private void SetCharacterName()
    {
        /*if (PV.IsMine)
        {
            Debug.LogFormat("플레이어 닉네임:{0}", PlayerInfo.playerInfo.nickname);
            nickNameTxt.GetComponent<TMP_Text>().text = PlayerInfo.playerInfo.nickname;
        }
        else
        {
            nickNameTxt.GetComponent<TMP_Text>().text = PlayerInfo.playerInfo.nickname;
        }*/

    }
}

