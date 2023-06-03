using Photon.Pun;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도 조절을 위한 변수
    PhotonView photonView;

    void Update()
    {
        photonView = GetComponent<PhotonView>();
        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("MiniGameManager").GetComponent<MiniGameManage>().AddPenaltyScore();

            // 삭제를 MasterClient에게 위임
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("DestroyGameObjectRPC", RpcTarget.MasterClient, gameObject.GetPhotonView().ViewID);
            }
            else
            {
                // MasterClient가 삭제 작업 수행
                DestroyGameObject(gameObject.GetPhotonView().ViewID);
            }
        }
    }
    [PunRPC]
    private void DestroyGameObjectRPC(int viewID)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView != null && photonView.IsMine)
        {
            DestroyGameObject(viewID);
        }
    }

    private void DestroyGameObject(int viewID)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        if (photonView != null && photonView.IsMine)
        {
            PhotonNetwork.Destroy(photonView.gameObject);
        }
    }
}
