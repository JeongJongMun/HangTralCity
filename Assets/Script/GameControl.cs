using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControl : MonoBehaviour
{
    [SerializeField] GameObject GangPanel;
    [SerializeField] Text GangPanelText;


    // Start is called before the first frame update
    void Start()
    {
        GangPanel.SetActive(false);
        GangPanelText.text = "����";
    }

    public void panel()
    {
        if (GangPanel.activeSelf)
        {
            GangPanel.SetActive(false);
            GangPanelText.text = "����";
        }
        else
        {
            GangPanel.SetActive(true);
            GangPanelText.text = "�ݱ�";
        }
    }
}
