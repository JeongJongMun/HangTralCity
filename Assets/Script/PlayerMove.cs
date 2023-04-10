using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float force = 0.05f;
    void Update()
    {
        if (Input.GetKey( KeyCode.LeftArrow)) transform.Translate(-force, 0f, 0f);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(force, 0f, 0f);
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(0f, force, 0f);
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(0f, -force, 0f);

    }
}
