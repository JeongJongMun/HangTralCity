using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerLocation;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float smoothRotate = 5.0f;

    private Transform CamLocation;
    // Start is called before the first frame update
    void Start()
    {
        CamLocation = GetComponent<Transform>();        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float currYAngle = Mathf.LerpAngle(CamLocation.eulerAngles.y, 
            PlayerLocation.eulerAngles.y, smoothRotate * Time.deltaTime);
        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

        CamLocation.position = PlayerLocation.position - 
            (rot * Vector3.forward * distance) + (Vector3.up * height);

        CamLocation.LookAt(PlayerLocation);
    }
}
