using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTriggerCount : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && MiniGameManage.GetFinishedFlag() == false)
        {
            PenaltyCount.AddPanalty(1); //���Ÿ���� �÷��̾�� ������ �г�Ƽ +1
        }
    }
}
