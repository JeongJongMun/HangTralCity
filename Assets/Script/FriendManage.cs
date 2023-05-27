using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendManage : MonoBehaviour
{
    Dictionary<string, Dictionary<string, List<Vector3>>> friends; // <ģ�� �̸�, <���� �̸�, <���� ��ġ��>>>
    public Button addBtn; // ģ�� �߰� ��ư
    public TMP_InputField inputField; // ģ�� �̸� �Է�â

    private void Start()
    {
        addBtn.onClick.AddListener(AddFriend);
    }

    // ģ�� �߰�
    void AddFriend()
    {
        string friendName;
        if (inputField.text != null) 
        {
            friendName = inputField.text;
            inputField.text = null;
            Debug.LogFormat("FriendName : {0}", friendName);
        }
    }

    void LoadFriend()
    {
        // ģ�� ��� Db���� �ҷ�����
    }
    void DeleteFriend()
    {
        // ģ�� Db���� ����
    }
    void GoFriendDorm()
    {
        // ģ�� ����� ���� Db���� �޾ƿ��� �����
    }
    void FriendChat()
    {
        // ģ���� 1��1 ä��
    }
}
