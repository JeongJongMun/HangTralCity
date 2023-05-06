using Amazon.Runtime.Internal.Transform;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DormManage : MonoBehaviour
{
    private Vector2 scroll_amount = new Vector2(0.1f, 0); // ��ũ�� ��
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
        Load(); // �� ����� ����� �� Ŀ���͸���¡ ���� �ҷ�����
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
    private void Save() // �� Ŀ���͸���¡ ���� ����
    {
        // ���� ��� ������Ʈ ��������
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // ���� PlayerInfo�� ����� Ŀ���͸���¡ ���� ����
        PlayerInfo.player_info.furniture_pos.Clear();

        // ��� ������Ʈ�� ��ȯ�ϸ� �±׸� ��
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "Custom_Dorm")
            {
                // ���ϴ� �±װ� �ִ� ������Ʈ�� �̸��� ������ ��������
                string name = obj.name;
                Vector3 pos = obj.transform.position;

                // PlayerInfo�� ����� Ŀ���͸���¡ ���� ����
                // ���� key�� �ִٸ� list�� �߰�
                if (PlayerInfo.player_info.furniture_pos.ContainsKey(name)) PlayerInfo.player_info.furniture_pos[name].Add(pos);
                // key�� ���ٸ� list�� ���� �߰�
                else
                {
                    List<Vector3> list = new List<Vector3> { pos };
                    PlayerInfo.player_info.furniture_pos.Add(name, list);
                }
            }
        }

        S3Manage.s3Manage.UploadToS3(PlayerInfo.player_info.furniture_pos, PlayerInfo.player_info.nickname);

        // Ȯ�ο� ���   
        foreach (var kvp in PlayerInfo.player_info.furniture_pos)
            Debug.Log("Key = " + kvp.Key + ", Value : " + kvp.Value);
    }
    private void Load() // �� Ŀ���͸���¡ ���� �ҷ�����
    {
        // ���� ��� ������Ʈ ��������
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // ��� ������Ʈ�� ��ȯ�ϸ� �±׸� ���Ͽ� ���� �� Ŀ���͸���¡ ���� ����
        foreach (GameObject obj in allObjects) if (obj.tag == "Custom_Dorm") Destroy(obj);

        // PlayerInfo�� ����� �� Ŀ���͸���¡ ���� �ҷ�����
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
    private void ClickLeftBtn() // ��ũ�� �� �������� �̵�
    {
        furniture_list.GetComponent<ScrollRect>().normalizedPosition -= scroll_amount;
    }
    private void ClickRightBtn() // ��ũ�� �� ���������� �̵�
    {
        furniture_list.GetComponent<ScrollRect>().normalizedPosition += scroll_amount;

    }
}
