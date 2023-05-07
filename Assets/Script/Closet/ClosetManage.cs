using UnityEngine;
using UnityEngine.UI;

public class ClosetManage : MonoBehaviour
{
    public GameObject eyeScrollView, hatScrollView; // hat, eye 스크롤 뷰 오브젝트
    public Button eyeScrollViewBtn, hatScrollViewBtn; // hat, eye 스크롤 뷰 선택 버튼
    public GameObject[] eyeToggle, hatToggle; // hat, eye 카테고리의 커마 에셋 토글들
    public GameObject character;

    void Start()
    {
        // 스크롤뷰 선택 버튼 클릭시
        eyeScrollViewBtn.onClick.AddListener(ClickEyeCategory);
        hatScrollViewBtn.onClick.AddListener(ClickHatCategory);

        // 토글 클릭시 함수 호출
        for (int i = 0; i < eyeToggle.Length; i++)
        {
            int index = i;
            eyeToggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleEye(index); });
        }
        for (int i = 0; i < hatToggle.Length; i++)
        {
            int index = i;
            hatToggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleHat(index); });
        }
    }

    private void ToggleEye(int n)
    {
        if (eyeToggle[n].GetComponent<Toggle>().isOn)
        {
            // 다른 토글들 OFF
            for (int i = 0; i < eyeToggle.Length; i++) if (i != n) eyeToggle[i].GetComponent<Toggle>().isOn = false;
            // 정보 저장
            PlayerInfo.playerInfo.eyeCustom = n;
            // 선택한 토글 이미지 적용
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            // null 이미지로 설정
            PlayerInfo.playerInfo.eyeCustom = 11;
            // 이미지 적용
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
    }
    private void ToggleHat(int n)
    {
        if (hatToggle[n].GetComponent<Toggle>().isOn)
        {
            // 다른 토글들 OFF
            for (int i = 0; i < hatToggle.Length; i++) if (i != n) hatToggle[i].GetComponent<Toggle>().isOn = false;
            // 정보 저장
            PlayerInfo.playerInfo.hatCustom = n;
            // 선택한 토글 이미지 적용
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            // null 이미지로 설정
            PlayerInfo.playerInfo.hatCustom = 8;
            // 이미지 적용
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
    }
    private void ClickEyeCategory()
    {
        eyeScrollView.SetActive(true);
        hatScrollView.SetActive(false);
    }
    private void ClickHatCategory()
    {
        eyeScrollView.SetActive(false);
        hatScrollView.SetActive(true);
    }
}
