using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeManage : MonoBehaviour
{
    public GameObject[] toggle;
    public GameObject hat_point;
    public GameObject eye_point;

    void Start()
    {
        for (int i = 0; i < toggle.Length; i++)
        {
            int index = i;
            toggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleEye(index); });
        }
    }

    private void ToggleEye(int n)
    {
        Debug.Log(n);
        if (toggle[n].GetComponent<Toggle>().isOn) eye_point.GetComponent<SpriteRenderer>().sprite = toggle[n].transform.GetChild(3).GetComponent<Image>().sprite
                ;
        else eye_point.GetComponent<SpriteRenderer>().sprite = null;
    }
}
