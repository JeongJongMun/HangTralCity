using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{
    public GameObject yellowTile;

    float timer = 0;
    private void Update()
    {
        if (Timer.GetFinishedFlag() == false)
        {
            timer += Time.deltaTime;

            if (timer > 0.1f)
            {
                GameObject newTile = Instantiate(yellowTile);
                newTile.transform.position = new Vector2(Random.Range(-15f, 10f), 21);
                timer = 0;
                Destroy(newTile, 5.0f);
            }
        }
    }
}
