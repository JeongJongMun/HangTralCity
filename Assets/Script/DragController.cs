using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private GameObject current_object;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        current_object = gameObject;
        //PlayerInfo.player_info.furniture_pos.Add(current_object.name, current_object.transform.position);
    }

    void OnMouseDrag()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            current_object.transform.Rotate(Vector3.forward * 90);
        }
        else
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }

    }
}
