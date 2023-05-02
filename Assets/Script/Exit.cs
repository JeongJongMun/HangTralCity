using Amazon.EC2.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public GameObject exit_btn;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("GangDoor"))
        {
            exit_btn.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GangDoor"))
        {
            exit_btn.SetActive(false);
        }
    }
    public void ExitBtnClicked()
    {
        //SceneManager.LoadScene("MainScene");
    }
}
