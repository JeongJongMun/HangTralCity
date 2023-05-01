using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;

public class ClosetManage : MonoBehaviour
{
    public GameObject eye_scroll_view, hat_scroll_view; // hat, eye 스크롤 뷰 오브젝트
    public Button eye_scroll_view_btn, hat_scroll_view_btn; // hat, eye 스크롤 뷰 선택 버튼
    public GameObject[] eye_toggle, hat_toggle; // hat, eye 카테고리의 커마 에셋 토글들
    public GameObject hat_point; // player의 hat 위치
    public GameObject eye_point; // player의 eye 위치

    void Start()
    {
        // 스크롤뷰 선택 버튼 클릭시
        eye_scroll_view_btn.onClick.AddListener(ClickEyeCategory);
        hat_scroll_view_btn.onClick.AddListener(ClickHatCategory);

        // 토글 클릭시 함수 호출
        for (int i = 0; i < eye_toggle.Length; i++)
        {
            int index = i;
            eye_toggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleEye(index); });
        }
        for (int i = 0; i < hat_toggle.Length; i++)
        {
            int index = i;
            hat_toggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleHat(index); });
        }
    }

    private void ToggleEye(int n)
    {
        if (eye_toggle[n].GetComponent<Toggle>().isOn)
        {
            // 다른 토글들 OFF
            for (int i = 0; i < eye_toggle.Length; i++) if (i != n) eye_toggle[i].GetComponent<Toggle>().isOn = false;
            // 선택한 토글 이미지 적용
            eye_point.GetComponent<SpriteRenderer>().sprite = eye_toggle[n].transform.GetChild(3).GetComponent<Image>().sprite;

        }
        else eye_point.GetComponent<SpriteRenderer>().sprite = null;
    }
    private void ToggleHat(int n)
    {
        if (hat_toggle[n].GetComponent<Toggle>().isOn)
        {
            // 다른 토글들 OFF
            for (int i = 0; i < hat_toggle.Length; i++) if (i != n) hat_toggle[i].GetComponent<Toggle>().isOn = false;
            // 선택한 토글 이미지 적용
            hat_point.GetComponent<SpriteRenderer>().sprite = hat_toggle[n].transform.GetChild(3).GetComponent<Image>().sprite;
        }
        else hat_point.GetComponent<SpriteRenderer>().sprite = null;
    }
    private void ClickEyeCategory()
    {
        eye_scroll_view.SetActive(true);
        hat_scroll_view.SetActive(false);
    }
    private void ClickHatCategory()
    {
        eye_scroll_view.SetActive(false);
        hat_scroll_view.SetActive(true);
    }
}
