using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini1BlueTeamAction : MonoBehaviour
{
    public float movingForce = 0.05f;
    private int blueTiles = 0;
    private bool tileOnHand = false;

    private GameObject thePoint;

    private void OnTriggerStay2D(Collider2D collision) //플레이어와 Point가 겹친동안 시행
    {
        if (collision.gameObject.tag == "Point")
        {
            if (blueTiles < 4)
            {
                //TODO
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) this.transform.Translate(-movingForce, 0, 0);
        if (Input.GetKey(KeyCode.RightArrow)) this.transform.Translate(movingForce, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow)) this.transform.Translate(0, movingForce, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) this.transform.Translate(0, -movingForce, 0);

        
    }
}
