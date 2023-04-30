using Amazon.S3.Transfer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed; // �̵� �ӵ�
    
    private float playerScale = 0.1f; // �÷��̾� ũ�� ����

    Rigidbody2D rigid;
    Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // �̵�
        rigid.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime, ForceMode2D.Impulse );
        
        // x�� ����
        if (Input.GetAxisRaw("Horizontal") < 0) transform.localScale = new Vector3(-1 * playerScale, playerScale, playerScale);
        else if(Input.GetAxisRaw("Horizontal") > 0) transform.localScale = new Vector3(1 * playerScale, playerScale, playerScale);

        // �ȱ� �ִϸ��̼� ����
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);
    }
}
