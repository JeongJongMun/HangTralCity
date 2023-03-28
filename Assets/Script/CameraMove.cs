using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) transform.position += Vector3.up / 100;
        else if (Input.GetKey(KeyCode.DownArrow)) transform.position += Vector3.down / 100;
        else if (Input.GetKey(KeyCode.LeftArrow)) transform.position += Vector3.left / 100;
        else if (Input.GetKey(KeyCode.RightArrow)) transform.position += Vector3.right / 100;
    
    }
}
