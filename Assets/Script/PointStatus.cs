using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointStatus : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite yellowTile;
    public Sprite redTile;
    public Sprite blueTile;

    void ChangeToBlue()
    {
        spriteRenderer.sprite = blueTile;
    }
    void ChangeToRed()
    {
        spriteRenderer.sprite = redTile;
    }
    void ChangeToYellow()
    {
        spriteRenderer.sprite = yellowTile;
    }

    private string activated = "yellow";
    //default : 'yellow'
    //'블루팀타일' : 'blue', '레드팀타일' : 'red'

 
    public string Activated
    //Mini1BlueTeamAction, Mini1RedTeamAction에서 Point의 색깔에 대한 정보를 받고 상태를 수정해야 해서
    {
        get { return activated; }
        set
        {
            if (activated == "yellow") //노란타일(빈타일)이면 빨간타일 파란타일 다 올 수 있어서
            {
                if (value == "blue")
                {
                    ChangeToBlue();
                    activated = value;
                }
                else if (value == "red")
                {
                    ChangeToRed();
                    activated = value;
                }
            }

            else if (activated == "blue") //파란타일이면 바로 빨간타일은 올 수 없어서
            {
                if (value == "yellow")
                {
                    ChangeToYellow();
                    activated = value;
                }
            }

            else if (activated == "red") //빨간타일이면 바로 파란타일은 올 수 없어서
            {
                if (value == "yellow")
                {
                    ChangeToYellow();
                    activated = value;
                }
            }
        }
    }
}