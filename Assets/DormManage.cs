using Amazon.EC2;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DormManage : MonoBehaviour
{
    public Toggle edit;
    public GameObject save_btn, reset_btn, pull_btn;
    private void Start()
    {
        edit.onValueChanged.AddListener(delegate { Edit(); });
        save_btn.GetComponent<Button>().onClick.AddListener(Save);
    }

    private void Edit()
    {
        if (edit.isOn)
        {
            save_btn.SetActive(true);
            reset_btn.SetActive(true);
        }
        else
        {
            save_btn.SetActive(false);
            reset_btn.SetActive(false);
        }
        
    }
    private void Save()
    {
        // ���� ��� ������Ʈ ��������
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // ��� ������Ʈ�� ��ȯ�ϸ� �±׸� ��
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "Custom_Dorm")
            {
                // ���ϴ� �±װ� �ִ� ������Ʈ�� �̸��� ������ ��������
                string name = obj.name;
                Vector3 position = obj.transform.position;

                // ���� PlayerInfo�� ����� Ŀ���͸���¡ ���� ����
                PlayerInfo.player_info.furniture_pos.Clear();

                // PlayerInfo�� ����� Ŀ���͸���¡ ���� ����
                PlayerInfo.player_info.furniture_pos.Add(name, position);

                // Ȯ�ο� ���   
                foreach (var kvp in PlayerInfo.player_info.furniture_pos)
                {
                    Debug.Log("Key = " + kvp.Key + ", Value : " + kvp.Value);
                }
            }
        }
    }
}
