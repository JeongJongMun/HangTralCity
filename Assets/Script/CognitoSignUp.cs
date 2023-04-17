using System.Collections.Generic;
using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using Amazon;

public class CognitoSignUp : MonoBehaviour
{
    private AmazonCognitoIdentityProviderClient cognitoService; // cognitoService ��ü ����

    void Start()
    {
        // Amazon Cognito ���� ���� ���� (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito ������ ��ü�� �ν��Ͻ�ȭ
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);
        SignUp(); // ����Ƽ �����ϸ� �ٷ� ȸ������ �����Բ� �س���
    }

    public async void SignUp()
    {
        // ����� ��� ��û ����
        var signUpRequest = new SignUpRequest
        {
            ClientId = "1luokqrq9t4j8gag5kbnphunvu", // Ŭ���̾�Ʈ ID (��� ����ڵ� ����)
            Username = "SimJaeHyeok", // ����� �̸�
            Password = "12345678", // ��й�ȣ
            UserAttributes = new List<AttributeType> // aws���� �츮�� ���� ������ �ʼ� �Ӽ�
            {
                new AttributeType { Name = "email", Value = "simson0524@naver.com" },
                new AttributeType { Name = "nickname", Value = "sjh" }
            }
        };

        // ���� ó�� �� ���� �� ���� ó��
        try
        {
            var response = await cognitoService.SignUpAsync(signUpRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                Debug.Log("Sign-Up Successful.");
            }
            else
            {
                Debug.Log("Sign-Up Failed. Response: " + response.HttpStatusCode);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Sign-Up Failed: " + e.Message);
        }
    }
}