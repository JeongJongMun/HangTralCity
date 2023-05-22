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

    private float playerScale = 0.1f; // �÷��̾� ũ�� ����
    private float speed = 10f; // �÷��̾� �̵� �ӵ�

    // ĳ���� Ÿ�� ���� ����
    private Animator AN;

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


    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "ClosetScene") // closet scene�϶�
        {
            nickNameTxt.GetComponent<TMP_Text>().text = "";
            if (PV.IsMine)
            {
                transform.localPosition = new Vector3(0, 2, -1);
                SetCharacterType();
            }
        }
        else // �� ���� ��� ������ nickname ǥ��
        {
            nickNameTxt.GetComponent<TMP_Text>().text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
            nickNameTxt.GetComponent<TMP_Text>().color = PV.IsMine ? Color.green : Color.blue;

            if (PV.IsMine && SceneManager.GetActiveScene().name == "GangScene") // Gang scene�϶�
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

                // ĳ���� ����
                currentHatCustom = PlayerInfo.playerInfo.hatCustom; // �ʱⰪ ����
                currentEyeCustom = PlayerInfo.playerInfo.eyeCustom;
                PV.RPC("SetCharacterCustom", RpcTarget.AllBuffered); // �ٸ� �÷��̾�� �� Ŀ���� ���� �� ĳ���� Ÿ�� ����
                PV.RPC("SetCharacterType", RpcTarget.AllBuffered);

                //ä��
                Chat();
            }
        }
    }
    void Update()
    {
        Move();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // ��������ȭ
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            // �ٸ� �÷��̾�� �� ���� Ŀ���� ������ ����
            stream.SendNext(currentHatCustom);
            stream.SendNext(currentEyeCustom);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            // �ٸ� �÷��̾�� Ŀ���� ������ �ޱ�
            currentHatCustom = (int)stream.ReceiveNext();
            currentEyeCustom = (int)stream.ReceiveNext();
            // �ٸ� �÷��̾��� Ŀ���� ��������Ʈ ����
            hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[currentHatCustom];
            eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[currentEyeCustom];
        }
    }
    [PunRPC]
    public void SetCharacterCustom()
    {
        // �÷��̾��� �ڽ� ĳ���� Ŀ���� ����
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
        Invoke("Delay", 5); // 2�ʵ� 0.3���ֱ�� LaunchProjectile�Լ� �ݺ� ȣ��
    }

    void Delay()
    {
        ChatText.GetComponent<TMP_Text>().text = "";
    }

    private void Move() {
        // if (���� ������ �� ĳ���� OR ���� ���� ���� �����)
        if (SceneManager.GetActiveScene().name == "GangScene" || SceneManager.GetActiveScene().name == "DormScene")
        {
            if (PV.IsMine)
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

                }
                else if (axis_x > 0)
                {
                    PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_x);

                }


            }
            //ismine�� �ƴѰ�� ��ġ����ȭ (�ٸ� ����� �����̴°� ���� �� �� �ְ�)
            else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // �ָ� �������ٸ�
            else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // ��ó���
        }
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

    [PunRPC]
    void FlipXRPC(float axis)
    {
        transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
        // �г����� x�� ���� X
        Transform rt = nickNamePoint.transform;
        rt.localScale = new Vector3(1, 1, 1);
        //ä���� x�� ���� X
        Transform rr = ChatPoint.transform;
        rr.localScale = new Vector3(1, 1, 1);
    }
    [PunRPC]
    void FlipXxRPC(float axis)
    {
        transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
        // �г����� x�� ���� X
        Transform rt = nickNamePoint.transform;
        rt.localScale = new Vector3(-1, 1, 1);
        //ä���� x�� ���� X
        Transform rr = ChatPoint.transform;
        rr.localScale = new Vector3(-1, 1, 1);
    }
}

