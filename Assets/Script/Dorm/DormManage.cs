using Amazon.Runtime.Internal.Transform;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DormManage : MonoBehaviour
{
    private Vector2 scroll_amount = new Vector2(0.1f, 0); // 스크롤 양
    public Toggle edit;
    public GameObject save_btn, reset_btn, furniture_list, left_btn, right_btn;
    public GameObject bed, carpet1, carpet2, desk, drawer, flower_pot, sofa, trash_bin;
    private void Start()
    {
        edit.onValueChanged.AddListener(delegate { Edit(); });
        save_btn.GetComponent<Button>().onClick.AddListener(Save);
        left_btn.GetComponent<Button>().onClick.AddListener(ClickLeftBtn);
        right_btn.GetComponent<Button>().onClick.AddListener(ClickRightBtn);
        reset_btn.GetComponent<Button>().onClick.AddListener(Load);
        Load(); // 씬 입장시 저장된 방 커스터마이징 정보 불러오기
    }

    private void Edit()
    {
        if (edit.isOn)
        {
            save_btn.SetActive(true);
            reset_btn.SetActive(true);
            furniture_list.SetActive(true);
            left_btn.SetActive(true);
            right_btn.SetActive(true);
        }
        else
        {
            save_btn.SetActive(false);
            reset_btn.SetActive(false);
            furniture_list.SetActive(false);
            left_btn.SetActive(false);
            right_btn.SetActive(false);
        }
        
    }
    private void Save() // 방 커스터마이징 정보 저장
    {
        // 씬의 모든 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // 기존 PlayerInfo의 기숙사 커스터마이징 정보 삭제
        PlayerInfo.player_info.furniture_pos.Clear();

        // 모든 오브젝트를 순환하며 태그를 비교
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "Custom_Dorm")
            {
                // 원하는 태그가 있는 오브젝트의 이름과 포지션 가져오기
                string name = obj.name;
                Vector3 pos = obj.transform.position;

                // PlayerInfo에 기숙사 커스터마이징 정보 저장
                // 기존 key가 있다면 list에 추가
                if (PlayerInfo.player_info.furniture_pos.ContainsKey(name)) PlayerInfo.player_info.furniture_pos[name].Add(pos);
                // key가 없다면 list를 만들어서 추가
                else
                {
                    List<Vector3> list = new List<Vector3> { pos };
                    PlayerInfo.player_info.furniture_pos.Add(name, list);
                }
            }
        }

        S3Manage.s3Manage.UploadToS3(PlayerInfo.player_info.furniture_pos, PlayerInfo.player_info.nickname);

        // 확인용 출력   
        foreach (var kvp in PlayerInfo.player_info.furniture_pos)
            Debug.Log("Key = " + kvp.Key + ", Value : " + kvp.Value);
    }
    private void Load() // 방 커스터마이징 정보 불러오기
    {
        // 씬의 모든 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // 모든 오브젝트를 순환하며 태그를 비교하여 기존 방 커스터마이징 정보 삭제
        foreach (GameObject obj in allObjects) if (obj.tag == "Custom_Dorm") Destroy(obj);

        // PlayerInfo에 저장된 방 커스터마이징 정보 불러오기
        string to_remove = "(Clone)";
        foreach (var kvp in PlayerInfo.player_info.furniture_pos)
        {
            if (kvp.Key.Replace(to_remove, "") == "Bed") foreach (Vector3 pos in kvp.Value) Instantiate(bed, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Carpet1") foreach (Vector3 pos in kvp.Value) Instantiate(carpet1, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Carpet2") foreach (Vector3 pos in kvp.Value) Instantiate(carpet2, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Desk") foreach (Vector3 pos in kvp.Value) Instantiate(desk, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Drawer") foreach (Vector3 pos in kvp.Value) Instantiate(drawer, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "FlowerPot") foreach (Vector3 pos in kvp.Value) Instantiate(flower_pot, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Sofa") foreach (Vector3 pos in kvp.Value) Instantiate(sofa, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "TrashBin") foreach (Vector3 pos in kvp.Value) Instantiate(trash_bin, pos, Quaternion.identity);
            Debug.Log("Key = " + kvp.Key + ", Value : " + kvp.Value);
        }
    }
    private void ClickLeftBtn() // 스크롤 바 왼쪽으로 이동
    {
        furniture_list.GetComponent<ScrollRect>().normalizedPosition -= scroll_amount;
    }
    private void ClickRightBtn() // 스크롤 바 오른쪽으로 이동
    {
        furniture_list.GetComponent<ScrollRect>().normalizedPosition += scroll_amount;

    }
}
