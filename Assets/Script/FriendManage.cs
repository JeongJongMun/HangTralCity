using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendManage : MonoBehaviour
{
    Dictionary<string, Dictionary<string, List<Vector3>>> friends; // <친구 이름, <가구 이름, <가구 위치들>>>
    public Button addBtn; // 친구 추가 버튼
    public TMP_InputField inputField; // 친구 이름 입력창

    private void Start()
    {
        addBtn.onClick.AddListener(AddFriend);
    }

    // 친구 추가
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
        // 친구 목록 Db에서 불러오기
    }
    void DeleteFriend()
    {
        // 친구 Db에서 삭제
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
