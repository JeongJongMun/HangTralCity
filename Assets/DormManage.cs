using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DormManage : MonoBehaviour
{
    public Toggle edit;
    public GameObject save_btn, reset_btn;
    private void Start()
    {
        edit.onValueChanged.AddListener(delegate { Edit(); });
    }

    private void Edit()
    {
        if (edit.isOn)
        {
            save_btn.SetActive(true);
            reset_btn.SetActive(true);
        }
        else
        {
            save_btn.SetActive(false);
            reset_btn.SetActive(false);
        }
        
    }
}
