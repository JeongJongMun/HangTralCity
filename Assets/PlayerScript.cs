using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEditor.Animations;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;

    public GameObject ExitBtn;
    public Button btn_exit;

    AudioSource dooropenEffect;

    Vector3 curPos;

    private float playerScale = 0.1f; // �÷��̾� ũ�� ����

    // ĳ���� Ÿ�� ���� ����
    public AnimatorController[] animators;
    public Sprite[] sprites;

    void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        SetCharacterType();
        if (PV.IsMine)
        {
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }

        ExitBtn = GameObject.Find("ExitBtn");
        ExitBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            //�����̱�
            float axis_x = Input.GetAxisRaw("Horizontal");
            float axis_y = Input.GetAxisRaw("Vertical");
            RB.velocity = new Vector2(10 * axis_x, 10 * axis_y);
            RB.AddForce(RB.velocity * Time.deltaTime, ForceMode2D.Impulse);

            //rigid.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime, ForceMode2D.Impulse);

            //�ȱ� �ִϸ��̼� ����
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                AN.SetBool("isWalking", true);
                //PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_x);
            }
            else AN.SetBool("isWalking", false);

            //x�� ����
            if (axis_x < 0) transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
            else if (axis_x > 0) transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
        }

        //ismine�� �ƴѰ�� ��ġ����ȭ (�ٸ� ����� �����̴°� ���� �� �� �ְ�
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // �ָ� �������ٸ�
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // ��ó���




        float distance1 = Vector3.Distance(new Vector3(-14, (float)7.5, -1), transform.position);
        float distance2= Vector3.Distance(new Vector3(14, (float)7.5, -1), transform.position);

        if (distance1 <= 2.5f || distance2 <= 2.5f)
        {
            ExitBtn.SetActive(true);
        }
        else
        {
            ExitBtn.SetActive(false);
        }


    }

    public void Exit()
    {
        btn_exit.GetComponent<Button>().onClick.AddListener(EnterMain);
    }
    void EnterMain()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("call");
    }

    //[PunRPC]
    //void FlipXRPC(float axis) => SR.flipX = axis == -1;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // ��������ȭ
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
    private void SetCharacterType()
    {
        Debug.LogFormat("�÷��̾� Ÿ�� : {0}", PlayerInfo.player_info.character_type);
        SR.sprite = sprites[PlayerInfo.player_info.character_type];
        AN.runtimeAnimatorController = animators[PlayerInfo.player_info.character_type];
        Debug.Log(SR.sprite);
        Debug.Log(AN.runtimeAnimatorController);
    }
}
