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
    //public AnimatorController[] animators;
    private List<string> animators = new List<string> {"puppy", "cat", "bear", "dino", "rabbit" }; 
    private List<string> sprites = new List<string> {"puppy_ridle", "cat_ridle", "bear_ridle", "dino_ridle", "rabbit_ridle" }; 
    //public Sprite[] characterSprites;
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

    void Awake()
    {
        //nameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickNameTxt.GetComponent<TMP_Text>().color = PV.IsMine ? Color.green : Color.blue;
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        

        // 나가기 버튼 (강의동일 경우에만 찾음)
        if (SceneManager.GetActiveScene().name == "GangScene")
        {
            exitBtn = GameObject.Find("ExitBtn");
            exitBtn.GetComponent<Button>().onClick.AddListener(Exit);
            exitBtn.SetActive(false);
        }

        // if (서버 내에서 내 캐릭터 AND 현재 씬이 로컬 기숙사가 아님)
        if (PV.IsMine && SceneManager.GetActiveScene().name != "DormScene" && SceneManager.GetActiveScene().name != "ClosetScene")
        {
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
    }
    void Start()
    {
        // 캐릭터 생성시 타입/커스텀/닉네임 설정
        SetCharacterType();
        SetCharacterCustom();
        SetCharacterName();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
            }
            else if (axis_x > 0)
            {
                transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
                // 닉네임은 x축 반전 X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(1, 1, 1);
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
    private void SetCharacterType()
    {
        SR.sprite = Resources.Load<Sprite>(sprites[PlayerInfo.playerInfo.characterType]);
        //SR.sprite = characterSprites[PlayerInfo.playerInfo.characterType];
        //AN.runtimeAnimatorController = animators[PlayerInfo.playerInfo.characterType];
        AN.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animators[PlayerInfo.playerInfo.characterType]);
        Debug.LogFormat("스프라이트:{0}, 컨트롤러:{1}, 캐릭터타입인덱스:{2}", SR.sprite.name, AN.runtimeAnimatorController.name, PlayerInfo.playerInfo.characterType);
        //Debug.LogFormat("지정된 스프라이트:{0}", characterSprites[PlayerInfo.playerInfo.characterType].name);
    }
    public void SetCharacterCustom()
    {
        Debug.LogFormat("플레이어 커스텀 모자:{0}, 눈:{1}", PlayerInfo.playerInfo.hatCustom, PlayerInfo.playerInfo.eyeCustom);
        hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[PlayerInfo.playerInfo.hatCustom];
        eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[PlayerInfo.playerInfo.eyeCustom];
    }
    private void SetCharacterName()
    {
        Debug.LogFormat("플레이어 닉네임:{0}", PlayerInfo.playerInfo.nickname);
        nickNameTxt.GetComponent<TMP_Text>().text = PlayerInfo.playerInfo.nickname;
    }
}

