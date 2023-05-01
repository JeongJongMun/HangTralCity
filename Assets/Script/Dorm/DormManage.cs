using UnityEngine;
using UnityEngine.UI;

public class DormManage : MonoBehaviour
{
    public Vector2 scroll_amount; // ��ũ�� ��
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
                Vector3 position = obj.transform.position;

                // PlayerInfo�� ����� Ŀ���͸���¡ ���� ����
                PlayerInfo.player_info.furniture_pos.Add(name, position);
            }
        }
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
            if (kvp.Key.Replace(to_remove, "") == "Bed") Instantiate(bed, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Carpet1") Instantiate(carpet1, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Carpet2") Instantiate(carpet2, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Desk") Instantiate(desk, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Drawer") Instantiate(drawer, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "FlowerPot") Instantiate(flower_pot, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Sofa") Instantiate(sofa, kvp.Value, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "TrashBin") Instantiate(trash_bin, kvp.Value, Quaternion.identity);
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
