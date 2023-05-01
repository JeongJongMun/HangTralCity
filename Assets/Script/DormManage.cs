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
        // 씬의 모든 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // 모든 오브젝트를 순환하며 태그를 비교
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "Custom_Dorm")
            {
                // 원하는 태그가 있는 오브젝트의 이름과 포지션 가져오기
                string name = obj.name;
                Vector3 position = obj.transform.position;

                // 기존 PlayerInfo의 기숙사 커스터마이징 정보 삭제
                PlayerInfo.player_info.furniture_pos.Clear();

                // PlayerInfo에 기숙사 커스터마이징 정보 저장
                PlayerInfo.player_info.furniture_pos.Add(name, position);

                // 확인용 출력   
                foreach (var kvp in PlayerInfo.player_info.furniture_pos)
                {
                    Debug.Log("Key = " + kvp.Key + ", Value : " + kvp.Value);
                }
            }
        }
    }
}
