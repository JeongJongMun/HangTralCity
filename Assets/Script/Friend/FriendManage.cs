using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FriendManage : MonoBehaviour
{
    public Button addBtn; // ģ�� �߰� ��ư
    public TMP_InputField inputField; // ģ�� �̸� �Է�â
    public GameObject content; // �������� �߰��� ģ�� �������� �θ� ������Ʈ
    public GameObject friend; // �������� �߰� �� ģ�� ������
    Button[] deleteButtons; // ���� ��ư��
    private void Start()
    {
        addBtn.onClick.AddListener(AddFriend);
        SetDeleteBtnListener();

    }
    void SetDeleteBtnListener()
    {
        // �̸����� ��� ���� ��ư�� ã��
        deleteButtons = FindObjectsOfType<Button>().Where(button => button.name == "DeleteBtn").ToArray();

        // ã�� ��ư�鿡 ���� ������ ���� �Լ��� ����
        foreach (Button button in deleteButtons)
        {
            button.onClick.AddListener(() => DeleteFriend(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text, button.transform.parent.gameObject));
        }
    }

    // ģ�� �߰�
    void AddFriend()
    {
        string friendName;
        if (inputField.text != null) 
        {
            friendName = inputField.text;
            inputField.text = null;
            // ģ�� ������ �߰�
            GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
            // �̸� ����
            _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
            // �߰��� ģ�� �������� ���� ��ư�� ���� ������ ����
            SetDeleteBtnListener();

            Debug.LogFormat("FriendName : {0}", friendName);
        }
    }
    void DeleteFriend(string friendName, GameObject parentObject)
    {
        // ģ�� Db���� ����
        Destroy(parentObject);
        Debug.LogFormat("{0} Deleted", friendName);
        
    }

    void LoadFriend()
    {
        // ģ�� ��� Db���� �ҷ�����
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
