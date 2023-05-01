using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PenaltyCount : MonoBehaviour
{
    public static PenaltyCount Instance;
    private static int penalty = 0;
    public static TMP_Text penaltyTEXT;

    [SerializeField] 
    private TMP_Text penaltyText;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        penaltyTEXT = penaltyText;
    }

    public static void AddPanalty(int num)
    {
        penalty += num;
        penaltyTEXT.text = penalty.ToString();
    }

    public static int GetPenalty()
    {
        return penalty;
    }
}
