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
    //'�����Ÿ��' : 'blue', '������Ÿ��' : 'red'

 
    public string Activated
    //Mini1BlueTeamAction, Mini1RedTeamAction���� Point�� ���� ���� ������ �ް� ���¸� �����ؾ� �ؼ�
    {
        get { return activated; }
        set
        {
            if (activated == "yellow") //���Ÿ��(��Ÿ��)�̸� ����Ÿ�� �Ķ�Ÿ�� �� �� �� �־
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

            else if (activated == "blue") //�Ķ�Ÿ���̸� �ٷ� ����Ÿ���� �� �� ���
            {
                if (value == "yellow")
                {
                    ChangeToYellow();
                    activated = value;
                }
            }

            else if (activated == "red") //����Ÿ���̸� �ٷ� �Ķ�Ÿ���� �� �� ���
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