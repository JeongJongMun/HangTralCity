using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;

public class StartProgram : MonoBehaviour
{
    CognitoAWSCredentials credentials;

    private void Awake() {
        UnityInitializer.AttachToGameObject(this.gameObject);
        credentials = new CognitoAWSCredentials("자신의 자격 증명 풀 ID", RegionEndpoint.APNortheast2);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
