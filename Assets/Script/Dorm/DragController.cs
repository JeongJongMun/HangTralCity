using UnityEngine;
using UnityEngine.UI;

public class DragController : MonoBehaviour
{
    Vector3 screenPoint;
    Vector3 offset;
    GameObject current_object;
    Toggle editToggle;

    void Start()
    {
        editToggle = GameObject.Find("EditToggle").GetComponent<Toggle>();
    }
    void Update()
    {
        Delete();
    }
    void OnMouseDown()
    {
        if (editToggle.isOn)
        {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            current_object = gameObject;
        }
    }

    void OnMouseDrag()
    {
        if (editToggle.isOn)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                current_object.transform.Rotate(Vector3.forward * 90);
            }
            else
            {
                // ���콺 �巡�� ���� ���� Rigidbody2D�� Ÿ���� Dynamic���� ����
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                // ���� ��ġ�κ��� �̵� ���� ���
                Vector3 moveVector = curPosition - transform.position;

                // Rigidbody2D�� ����Ͽ� ���� ���ؼ� ������Ʈ �̵�
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(moveVector, ForceMode2D.Impulse);
            }
        }
    }
    void OnMouseExit()
    {
        // ���콺 �巡�� ���� �ƴ� ���� Rigidbody2D�� Ÿ���� Kinematic���� ����
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        // ���� �̵� ���� ��쿡�� �̵��� ����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }




    // ������ ���콺 ��ư Ŭ���� ���� ����
    void Delete()
    {
        if (Input.GetMouseButtonDown(1) && editToggle.isOn)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                string objectName = hit.collider.gameObject.name;
                string objectTag = hit.collider.gameObject.tag;
                if (objectTag == "Custom_Dorm" && objectName == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
