using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.UpArrow)) transform.position += Vector3.up / 50;
        //else if (Input.GetKey(KeyCode.LeftArrow)) transform.position += Vector3.left / 50;
        //else if (Input.GetKey(KeyCode.RightArrow)) transform.position += Vector3.right / 50;
        //else if (Input.GetKey(KeyCode.DownArrow)) transform.position += Vector3.down / 50;
        Vector3 tempVec = player.transform.position;
        tempVec.z = -10;
        transform.position = tempVec;
    
    }
}
