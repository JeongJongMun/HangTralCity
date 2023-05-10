using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendManage : MonoBehaviour
{
    Dictionary<string, Dictionary<string, List<Vector3>>> friends; // <친구 이름, <가구 이름, <가구 위치들>>>
    

    void LoadFriend()
    {
        // 친구 목록 Db에서 불러오기
    }
    void SaveFriend()
    {
        // 친구 목록 Db에 저장
    }
    void DeleteFriend()
    {
        // 친구 목록 Db에서 삭제
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
