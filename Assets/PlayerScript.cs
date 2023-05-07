using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEditor.Animations;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;

    AudioSource dooropenEffect;

    Vector3 curPos;

    private float playerScale = 0.1f; // �÷��̾� ũ�� ����
    private float speed = 10f; // �÷��̾� �̵� �ӵ�

    // ĳ���� Ÿ�� ���� ����
    public AnimatorController[] animators;
    public Sprite[] sprites;
    // ĳ���� Ŀ���� ���� ����
    public GameObject hatPoint;
    public GameObject eyePoint;
    // ĳ���� Ŀ���� ���� ����Ʈ
    public Sprite[] hatSprites;
    public Sprite[] eyeSprites;
    // ĳ���� �г��� ���� ����
    public GameObject nickNameTxt;
    public GameObject nickNamePoint;
    // ������ ��ư
    public GameObject exitBtn;

    void Awake()
    {
        //nameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickNameTxt.GetComponent<TMP_Text>().color = PV.IsMine ? Color.green : Color.red;
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        
        // ĳ���� ������ Ÿ��/Ŀ����/�г��� ����
        SetCharacterType();
        SetCharacterCustom();
        SetCharacterName();
        // ������ ��ư (���ǵ��� ��쿡�� ã��)
        if (SceneManager.GetActiveScene().name == "GangScene")
        {
            exitBtn = GameObject.Find("ExitBtn");
            exitBtn.GetComponent<Button>().onClick.AddListener(Exit);
            exitBtn.SetActive(false);
        }

        // if (���� ������ �� ĳ���� AND ���� ���� ���� ����簡 �ƴ�)
        if (PV.IsMine && SceneManager.GetActiveScene().name != "DormScene" && SceneManager.GetActiveScene().name != "ClosetScene")
        {
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move() {
        // if (���� ������ �� ĳ���� OR ���� ���� ���� �����)
        if (PV.IsMine || SceneManager.GetActiveScene().name == "DormScene")
        {
            //�����̱�
            float axis_x = Input.GetAxisRaw("Horizontal");
            float axis_y = Input.GetAxisRaw("Vertical");
            RB.velocity = new Vector2(speed * axis_x, speed * axis_y);
            RB.AddForce(RB.velocity * Time.deltaTime, ForceMode2D.Impulse);

            //�ȱ� �ִϸ��̼� ����
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) AN.SetBool("isWalking", true);
            else AN.SetBool("isWalking", false);

            //x�� ����
            if (axis_x < 0)
            {
                transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
                // �г����� x�� ���� X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(-1, 1, 1);
            }
            else if (axis_x > 0)
            {
                transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
                // �г����� x�� ���� X
                Transform rt = nickNamePoint.transform;
                rt.localScale = new Vector3(1, 1, 1);
            }

        }
        //ismine�� �ƴѰ�� ��ġ����ȭ (�ٸ� ����� �����̴°� ���� �� �� �ְ�)
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // �ָ� �������ٸ�
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // ��ó���
    }

    // ���� ������� ��� ������ ��ư Ȱ��ȭ
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Door") exitBtn.SetActive(true);
    }
    // ���� ������ ��� ������ ��ư ��Ȱ��ȭ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door") exitBtn.SetActive(false);
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
    private void SetCharacterType()
    {
        Debug.LogFormat("�÷��̾� Ÿ�� : {0}", PlayerInfo.playerInfo.characterType);
        SR.sprite = sprites[PlayerInfo.playerInfo.characterType];
        AN.runtimeAnimatorController = animators[PlayerInfo.playerInfo.characterType];
        Debug.Log(SR.sprite);
        Debug.Log(AN.runtimeAnimatorController);
    }
    public void SetCharacterCustom()
    {
        Debug.LogFormat("�÷��̾� Ŀ���� ����:{0}, ��:{1}", PlayerInfo.playerInfo.hatCustom, PlayerInfo.playerInfo.eyeCustom);
        hatPoint.GetComponent<SpriteRenderer>().sprite = hatSprites[PlayerInfo.playerInfo.hatCustom];
        eyePoint.GetComponent<SpriteRenderer>().sprite = eyeSprites[PlayerInfo.playerInfo.eyeCustom];
    }
    private void SetCharacterName()
    {
        Debug.LogFormat("�÷��̾� �г���:{0}", PlayerInfo.playerInfo.nickname);
        nickNameTxt.GetComponent<TMP_Text>().text = PlayerInfo.playerInfo.nickname;
    }
}

