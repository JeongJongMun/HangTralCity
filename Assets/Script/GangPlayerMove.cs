using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangPlayerMove : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey( KeyCode.LeftArrow)) //��������Ű
        {
            this.transform.Translate(-0.005f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow)) //��������Ű
        {
            this.transform.Translate(0.005f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.UpArrow)) //������Ű
        {
            this.transform.Translate(0f, 0.005f, 0f);
        }

        if (Input.GetKey(KeyCode.DownArrow)) //�Ʒ�����Ű
        {
            this.transform.Translate(0f, -0.005f, 0f);
        }
    }
}
