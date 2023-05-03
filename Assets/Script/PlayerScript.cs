using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEditor.Animations;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;

    Vector3 curPos;

    private float playerScale = 0.1f; // 플레이어 크기 비율

    // 캐릭터 타입 변경 변수
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
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            //움직이기
            float axis_x = Input.GetAxisRaw("Horizontal");
            float axis_y = Input.GetAxisRaw("Vertical");
            RB.velocity = new Vector2(10 * axis_x, 10 * axis_y);
            RB.AddForce(RB.velocity * Time.deltaTime, ForceMode2D.Impulse);

            //rigid.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime, ForceMode2D.Impulse);

            //걷기 애니메이션 설정
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                AN.SetBool("isWalking", true);
                //PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_x);
            }
            else AN.SetBool("isWalking", false);

            //x축 반전
            if (axis_x < 0) transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
            else if (axis_x > 0) transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);
        }

        //ismine이 아닌경우 위치동기화 (다른 사람이 움직이는걸 내가 볼 수 있게
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos; // 멀리 떨어졌다면
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10); // 근처라면
    }

    //[PunRPC]
    //void FlipXRPC(float axis) => SR.flipX = axis == -1;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // 변수동기화
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
        Debug.LogFormat("플레이어 타입 : {0}",PlayerInfo.player_info.character_type);
        SR.sprite = sprites[PlayerInfo.player_info.character_type];
        AN.runtimeAnimatorController = animators[PlayerInfo.player_info.character_type];
        Debug.Log(SR.sprite);
        Debug.Log(AN.runtimeAnimatorController);
    }
}
