/*using Amazon.EC2.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Exit : MonoBehaviour
{
    public GameObject ExitBtn;
    public GameObject Canvas;

    void Awake()
    {
        ExitBtn = FindObjectWithTag("GangDoor");
        ExitBtn = Canvas.transform.Find("Exit_Btn").gameObject;
        ExitBtn.SetActive(false);
        Debug.Log("In");
        //ExitBtn.GetComponent<GameObject>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GangDoor"))
        {
            ExitBtn.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GangDoor"))
        {
            ExitBtn.SetActive(false);
        }
    }
 
}
*/