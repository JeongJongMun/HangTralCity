using Cinemachine;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public PhotonView PV;

    AudioSource dooropenEffect;

    Vector3 curPos;

    private float playerScale = 0.1f; // �÷��̾� ũ�� ����
    private float speed = 10f; // �÷��̾� �̵� �ӵ�

    // ĳ���� Ÿ�� ���� ����
    private SpriteRenderer SR;
    private Animator AN;

    private List<string> animators = new List<string> {"puppy", "cat", "bear", "dino", "rabbit" }; 
    private List<string> sprites = new List<string> {"puppy_ridle", "cat_ridle", "bear_ridle", "dino_ridle", "rabbit_ridle" }; 

    // ĳ���� Ŀ���� ���� ����
    public GameObject hatPoint;
    public GameObject eyePoint;

    // ĳ���� Ŀ���� ���� ����Ʈ
    public Sprite[] hatSprites;
    public Sprite[] eyeSprites;

    // ĳ���� �г��� ���� ����
    public GameObject nickNameTxt;
    public GameObject nickNamePoint;

    // ������ ��ư, ������ ��ư
    private GameObject exitBtn, galleryBtn;


    public GameObject ChatPoint;
    public GameObject ChatText;
    private Button sendBtn; // ä�� ������ ��ư UI
    private InputField inputfield; // ä�� �Է�â UI 

    //public GameObject chatBoxPrefab; // ��ǳ�� ������
    //public Transform chatTr; // ��ǳ�� ��ġ

    void Awake()
    {
        if(SceneManager.GetActiveScene().name != "ClosetScene")
        {
            nickNameTxt.GetComponent<TMP_Text>().text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
            nickNameTxt.GetComponent<TMP_Text>().color = PV.IsMine ? Color.green : Color.blue;
        }

        ChatText.GetComponent<TMP_Text>().text = "";

        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        AN = GetComponent<Animator>();

        // ĳ���� Ŀ���� ������ �۵�
        if (PV.IsMine && SceneManager.GetActiveScene().name == "ClosetScene")
        {
            SetCharacterType();
            SetCharacterCustom();
        }

        // ���ǵ����� �۵�
        if (PV.IsMine && SceneManager.GetActiveScene().name == "GangScene")
        {
            //������ ��ư
            exitBtn = GameObject.Find("ExitBtn");
            exitBtn.GetComponent<Button>().onClick.AddListener(Exit);
            exitBtn.SetActive(false);

            galleryBtn = GameObject.Find("GalleryBtn");
            galleryBtn.SetActive(false);

            //ī�޶�
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;

            //ĳ���� ����
            PV.RPC("SetCharacterCustom", RpcTarget.All);
            PV.RPC("SetCharacterType", RpcTarget.AllBuffered);

            //ä��
            Chat();
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
        Invoke("Delay", 5); // 2�ʵ� 0.3���ֱ�� LaunchProjectile�Լ� �ݺ� ȣ��
    }

    void Delay()
    {
        ChatText.GetComponent<TMP_Text>().text = "";
    }

    private void Move() {
        // if (���� ������ �� ĳ���� OR ���� ���� ���� �����)
        if (PV.IsMine && (SceneManager.GetActiveScene().name == "GangScene" || SceneManager.GetActiveScene().name == "DormScene"))
        {
            // ** �����̱� **
            // Ű���� �Է�
            float axis_x = Input.GetAxisRaw("Horizontal");
            float axis_y = Input.GetAxisRaw("Vertical");
            
            // ��ġ �Է�
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                axis_x = (touchPos.x > transform.position.x) ? 1 : -1;
                axis_y = (touchPos.y > transform.position.y) ? 1 : -1;
            }
            RB.velocity = new Vector2(speed * axis_x, speed * axis_y);
            RB.AddForce(RB.velocity * Time.deltaTime, ForceMode2D.Impulse);
            // ** �����̱� **

            // �ȱ� �ִϸ��̼� ����
            if (axis_x != 0 || axis_y != 0) AN.SetBool("isWalking", true);
            else AN.SetBool("isWalking", false);

            // x�� ����
            if (axis_x < 0)
            {
                PV.RPC("FlipXxRPC", RpcTarget.AllBuffered, axis_x);
                // �г����� x�� ���� X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(-1, 1, 1);
                //ä���� x�� ���� X
                Transform rr = ChatPoint.transform;
                rr.localScale = new Vector3(-1, 1, 1);
            }
            else if (axis_x > 0)
            {
                PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_x);
                // �г����� x�� ���� X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(1, 1, 1);
                //ä���� x�� ���� X
                Transform rr = ChatPoint.transform;
                rr.localScale = new Vector3(1, 1, 1);
            }


        }
        //ismine�� �ƴѰ�� ��ġ����ȭ (�ٸ� ����� �����̴°� ���� �� �� �ְ�)
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // �ָ� �������ٸ�
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // ��ó���
    }

    // ���� ������� ��� ������ ��ư Ȱ��ȭ
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.tag == "Door") exitBtn.SetActive(true);
            if (collision.tag == "MainDesk") galleryBtn.SetActive(true);
        }
        
    }
    // ���� ������ ��� ������ ��ư ��Ȱ��ȭ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.tag == "Door") exitBtn.SetActive(false);
            if (collision.tag == "MainDesk") galleryBtn.SetActive(false);
        }

    }
    // ������ ��ư Ŭ����
    public void Exit()
    {
        SceneManager.LoadScene("MainScene");
        PhotonNetwork.Disconnect();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // ��������ȭ
    {
        if (stream.IsWriting) stream.SendNext(transform.position);
        else curPos = (Vector3)stream.ReceiveNext();
    }

    [PunRPC]
    public void SetCharacterCustom()
    {
        Debug.LogFormat("�÷��̾� Ŀ���� ����:{0}, ��:{1}", PlayerInfo.playerInfo.hatCustom, PlayerInfo.playerInfo.eyeCustom);
        hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[PlayerInfo.playerInfo.hatCustom];
        eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[PlayerInfo.playerInfo.eyeCustom];
    }

    void setting()
    {

    }

    [PunRPC]
    void SetCharacterType()
    {
        AN.SetInteger("type", PlayerInfo.playerInfo.characterType);
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
    }
    [PunRPC]
    void FlipXxRPC(float axis)
    {
        transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
    }
}

