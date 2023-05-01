using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanaltyCount : MonoBehaviour
{
    public static PanaltyCount Instance;
    private static int panalty = 0;
    public static TMP_Text panaltyTEXT;

    [SerializeField] 
    private TMP_Text panaltyText;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        panaltyTEXT = panaltyText;
    }

    public static void AddPanalty(int num)
    {
        panalty += num;
        panaltyTEXT.text = panalty.ToString();
    }

    public static int GetPanalty()
    {
        return panalty;
    }
}
