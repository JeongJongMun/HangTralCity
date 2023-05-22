using Cinemachine;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public PhotonView PV;

    Vector3 curPos;
    int currentHatCustom, currentEyeCustom;

    private float playerScale = 0.1f; // 플레이어 크기 비율
    private float speed = 10f; // 플레이어 이동 속도

    // 캐릭터 타입 변경 변수
    private Animator AN;

    // 캐릭터 커스텀 변경 변수
    public GameObject hatPoint;
    public GameObject eyePoint;

    // 캐릭터 커스텀 에셋 리스트
    public Sprite[] hatSprites;
    public Sprite[] eyeSprites;

    // 캐릭터 닉네임 변경 변수
    public GameObject nickNameTxt;
    public GameObject nickNamePoint;

    // 나가기 버튼, 갤러리 버튼
    private GameObject exitBtn, galleryBtn;


    public GameObject ChatPoint;
    public GameObject ChatText;
    private Button sendBtn; // 채팅 보내기 버튼 UI
    private InputField inputfield; // 채팅 입력창 UI 


    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "ClosetScene") // closet scene일때
        {
            nickNameTxt.GetComponent<TMP_Text>().text = "";
            if (PV.IsMine)
            {
                transform.localPosition = new Vector3(0, 2, -1);
                SetCharacterType();
            }
        }
        else // 그 외의 모든 씬에서 nickname 표시
        {
            nickNameTxt.GetComponent<TMP_Text>().text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
            nickNameTxt.GetComponent<TMP_Text>().color = PV.IsMine ? Color.green : Color.blue;

            if (PV.IsMine && SceneManager.GetActiveScene().name == "GangScene") // Gang scene일때
            {
                //나가기 버튼
                exitBtn = GameObject.Find("ExitBtn");
                exitBtn.GetComponent<Button>().onClick.AddListener(Exit);
                exitBtn.SetActive(false);

                galleryBtn = GameObject.Find("GalleryBtn");
                galleryBtn.SetActive(false);

                //카메라
                var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
                CM.Follow = transform;
                CM.LookAt = transform;

                // 캐릭터 세팅
                currentHatCustom = PlayerInfo.playerInfo.hatCustom; // 초기값 설정
                currentEyeCustom = PlayerInfo.playerInfo.eyeCustom;
                PV.RPC("SetCharacterCustom", RpcTarget.AllBuffered); // 다른 플레이어에게 내 커스텀 정보 및 캐릭터 타입 전달
                PV.RPC("SetCharacterType", RpcTarget.AllBuffered);

                //채팅
                Chat();
            }
        }
    }
    void Update()
    {
        Move();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // 변수동기화
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            // 다른 플레이어에게 내 현재 커스텀 데이터 전달
            stream.SendNext(currentHatCustom);
            stream.SendNext(currentEyeCustom);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            // 다른 플레이어에게 커스텀 데이터 받기
            currentHatCustom = (int)stream.ReceiveNext();
            currentEyeCustom = (int)stream.ReceiveNext();
            // 다른 플레이어의 커스텀 스프라이트 변경
            hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[currentHatCustom];
            eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[currentEyeCustom];
        }
    }
    [PunRPC]
    public void SetCharacterCustom()
    {
        // 플레이어의 자신 캐릭터 커스텀 변경
        if (PV.IsMine)
        {
            hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[PlayerInfo.playerInfo.hatCustom];
            eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[PlayerInfo.playerInfo.eyeCustom];
        }
    }

    [PunRPC]
    void SetCharacterType()
    {
        AN.SetInteger("type", PlayerInfo.playerInfo.characterType);
    }

    private void Chat()
    {
        inputfield = GameObject.Find("ChatInputfield").GetComponent<InputField>();
        sendBtn = GameObject.Find("ChatSendBtn").GetComponent<Button>();
        sendBtn.onClick.AddListener(Send);
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
        if (SceneManager.GetActiveScene().name == "GangScene" || SceneManager.GetActiveScene().name == "DormScene")
        {
            if (PV.IsMine)
            {
                // ** 움직이기 **
                // 키보드 입력
                float axis_x = Input.GetAxisRaw("Horizontal");
                float axis_y = Input.GetAxisRaw("Vertical");

                // 터치 입력
                if (Input.GetMouseButton(0))
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    axis_x = (touchPos.x > transform.position.x) ? 1 : -1;
                    axis_y = (touchPos.y > transform.position.y) ? 1 : -1;
                }
                RB.velocity = new Vector2(speed * axis_x, speed * axis_y);
                RB.AddForce(RB.velocity * Time.deltaTime, ForceMode2D.Impulse);
                // ** 움직이기 **

                // 걷기 애니메이션 설정
                if (axis_x != 0 || axis_y != 0) AN.SetBool("isWalking", true);
                else AN.SetBool("isWalking", false);

                // x축 반전
                if (axis_x < 0)
                {
                    PV.RPC("FlipXxRPC", RpcTarget.AllBuffered, axis_x);

                }
                else if (axis_x > 0)
                {
                    PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_x);

                }


            }
            //ismine이 아닌경우 위치동기화 (다른 사람이 움직이는걸 내가 볼 수 있게)
            else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // 멀리 떨어졌다면
            else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // 근처라면
        }
    }

    // 문에 닿아있을 경우 나가기 버튼 활성화
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.tag == "Door") exitBtn.SetActive(true);
            if (collision.tag == "MainDesk") galleryBtn.SetActive(true);
        }
        
    }
    // 문과 떨어질 경우 나가기 버튼 비활성화
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.tag == "Door") exitBtn.SetActive(false);
            if (collision.tag == "MainDesk") galleryBtn.SetActive(false);
        }

    }
    // 나가기 버튼 클릭시
    public void Exit()
    {
        SceneManager.LoadScene("MainScene");
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
        // 닉네임은 x축 반전 X
        Transform rt = nickNamePoint.transform;
        rt.localScale = new Vector3(1, 1, 1);
        //채팅은 x축 반전 X
        Transform rr = ChatPoint.transform;
        rr.localScale = new Vector3(1, 1, 1);
    }
    [PunRPC]
    void FlipXxRPC(float axis)
    {
        transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
        // 닉네임은 x축 반전 X
        Transform rt = nickNamePoint.transform;
        rt.localScale = new Vector3(-1, 1, 1);
        //채팅은 x축 반전 X
        Transform rr = ChatPoint.transform;
        rr.localScale = new Vector3(-1, 1, 1);
    }
}

