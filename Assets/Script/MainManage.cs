using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManage : MonoBehaviour
{

    // 메인 화면 건물과 입장하기 버튼 누르면 이벤트 관리
    public Button building_gang, building_hack, building_dorm, building_playground;
    public Button enter_btn_gang, enter_btn_hack, enter_btn_dorm, enter_btn_playground;

    void Start()
    {
        building_gang.GetComponent<Button>().onClick.AddListener(EnterGang);
        enter_btn_gang.GetComponent<Button>().onClick.AddListener(EnterGang);

        building_dorm.GetComponent<Button>().onClick.AddListener(EnterDorm);
        enter_btn_dorm.GetComponent<Button>().onClick.AddListener(EnterDorm);

        building_hack.GetComponent<Button>().onClick.AddListener(EnterHack);
        enter_btn_hack.GetComponent<Button>().onClick.AddListener(EnterHack);

        building_playground.GetComponent<Button>().onClick.AddListener(EnterPlayground);
        enter_btn_playground.GetComponent<Button>().onClick.AddListener(EnterPlayground);
    }

    public void EnterGang()
    {
        SceneManager.LoadScene("GangScene");
    }
    public void EnterDorm()
    {
        SceneManager.LoadScene("DormScene");
    }
    public void EnterHack()
    {

    }

    public void EnterPlayground()
    {

    }
}
