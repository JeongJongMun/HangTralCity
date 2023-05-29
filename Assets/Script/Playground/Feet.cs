using UnityEngine;

public class Feet : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도 조절을 위한 변수

    void Update()
    {
        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") GameObject.Find("MiniGameManager").GetComponent<MiniGameManage>().penalty++;

    }
}
