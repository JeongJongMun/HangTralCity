using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangPlayerMove : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey( KeyCode.LeftArrow)) //좌측방향키
        {
            this.transform.Translate(-0.005f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow)) //우측방향키
        {
            this.transform.Translate(0.005f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.UpArrow)) //위방향키
        {
            this.transform.Translate(0f, 0.005f, 0f);
        }

        if (Input.GetKey(KeyCode.DownArrow)) //아래방향키
        {
            this.transform.Translate(0f, -0.005f, 0f);
        }
    }
}
