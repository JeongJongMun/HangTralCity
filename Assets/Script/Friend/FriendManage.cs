using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FriendManage : MonoBehaviour
{
    public Button addBtn; // 친구 추가 버튼
    public TMP_InputField inputField; // 친구 이름 입력창
    public GameObject content; // 동적으로 추가할 친구 프리팹의 부모 오브젝트
    public GameObject friend; // 동적으로 추가 할 친구 프리팹
    Button[] deleteButtons; // 삭제 버튼들
    private void Start()
    {
        addBtn.onClick.AddListener(AddFriend);
        SetDeleteBtnListener();

    }
    void SetDeleteBtnListener()
    {
        // 이름으로 모든 삭제 버튼을 찾음
        deleteButtons = FindObjectsOfType<Button>().Where(button => button.name == "DeleteBtn").ToArray();

        // 찾은 버튼들에 대해 동일한 삭제 함수를 연결
        foreach (Button button in deleteButtons)
        {
            button.onClick.AddListener(() => DeleteFriend(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text, button.transform.parent.gameObject));
        }
    }

    // 친구 추가
    void AddFriend()
    {
        string friendName;
        if (inputField.text != null) 
        {
            friendName = inputField.text;
            inputField.text = null;
            // 친구 프리팹 추가
            GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
            // 이름 적용
            _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
            // 추가된 친구 프리팹의 삭제 버튼에 삭제 리스너 부착
            SetDeleteBtnListener();

            Debug.LogFormat("FriendName : {0}", friendName);
        }
    }
    void DeleteFriend(string friendName, GameObject parentObject)
    {
        // 친구 Db에서 삭제
        Destroy(parentObject);
        Debug.LogFormat("{0} Deleted", friendName);
        
    }

    void LoadFriend()
    {
        // 친구 목록 Db에서 불러오기
    }

    void GoFriendDorm()
    {
        // 친구 기숙사 정보 Db에서 받아오고 놀러가기
    }
    void FriendChat()
    {
        // 친구와 1대1 채팅
    }
}
