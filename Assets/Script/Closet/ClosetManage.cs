using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;

public class ClosetManage : MonoBehaviour
{
    public GameObject eye_scroll_view, hat_scroll_view; // hat, eye ��ũ�� �� ������Ʈ
    public Button eye_scroll_view_btn, hat_scroll_view_btn; // hat, eye ��ũ�� �� ���� ��ư
    public GameObject[] eye_toggle, hat_toggle; // hat, eye ī�װ��� Ŀ�� ���� ��۵�
    public GameObject hat_point; // player�� hat ��ġ
    public GameObject eye_point; // player�� eye ��ġ

    void Start()
    {
        // ��ũ�Ѻ� ���� ��ư Ŭ����
        eye_scroll_view_btn.onClick.AddListener(ClickEyeCategory);
        hat_scroll_view_btn.onClick.AddListener(ClickHatCategory);

        // ��� Ŭ���� �Լ� ȣ��
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
            // �ٸ� ��۵� OFF
            for (int i = 0; i < eye_toggle.Length; i++) if (i != n) eye_toggle[i].GetComponent<Toggle>().isOn = false;
            // ������ ��� �̹��� ����
            eye_point.GetComponent<SpriteRenderer>().sprite = eye_toggle[n].transform.GetChild(3).GetComponent<Image>().sprite;

        }
        else eye_point.GetComponent<SpriteRenderer>().sprite = null;
    }
    private void ToggleHat(int n)
    {
        if (hat_toggle[n].GetComponent<Toggle>().isOn)
        {
            // �ٸ� ��۵� OFF
            for (int i = 0; i < hat_toggle.Length; i++) if (i != n) hat_toggle[i].GetComponent<Toggle>().isOn = false;
            // ������ ��� �̹��� ����
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
