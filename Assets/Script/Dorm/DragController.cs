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
                // 마우스 드래그 중일 때는 Rigidbody2D의 타입을 Dynamic으로 유지
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                // 현재 위치로부터 이동 벡터 계산
                Vector3 moveVector = curPosition - transform.position;

                // Rigidbody2D를 사용하여 힘을 가해서 오브젝트 이동
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(moveVector, ForceMode2D.Impulse);
            }
        }
    }
    void OnMouseExit()
    {
        // 마우스 드래그 중이 아닐 때는 Rigidbody2D의 타입을 Kinematic으로 변경
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        // 현재 이동 중인 경우에는 이동을 멈춤
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }




    // 오른쪽 마우스 버튼 클릭시 가구 삭제
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
