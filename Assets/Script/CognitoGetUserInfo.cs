using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using Amazon;

public class CognitoGetUserInfo : MonoBehaviour
{

    AmazonCognitoIdentityProviderClient cognitoService; // cognitoService ��ü ����

    string access_token;

    void Start()
    {
        // Amazon Cognito ���� ���� ���� (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito ������ ��ü�� �ν��Ͻ�ȭ
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);

        access_token = PlayerInfo.playerInfo.access_token; // �α��� ���� �� ��ȯ���� ������ ��ū

        GetUserInfo(); // ���� ȭ�� ���۽� �ڵ����� ���� ���� ��������


    }

    public async void GetUserInfo()
    {
        Debug.Log("GetUserInfo Called");
        // ����� ���� �������� ��û ����
        var getUserInfoRequest = new GetUserRequest
        {
            AccessToken = access_token // ������ ��ū ����
        };

        // ���� ó�� �� ���� �� ���� ó��
        try
        {
            var response = await cognitoService.GetUserAsync(getUserInfoRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                foreach (var attribute in response.UserAttributes)
                {
                    // UserAttributes ����Ʈ�� ��ȸ�ϸ� ���� ���� ��������
                    if (attribute.Name == "nickname") PlayerInfo.playerInfo.nickname = attribute.Value;
                    if (attribute.Name == "email") PlayerInfo.playerInfo.email = attribute.Value;
                }
            }
            else
            {
                Debug.Log("Get User Info Failed. Response: " + response.HttpStatusCode);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Get User Info Failed: " + e.Message);
        }
    }

}
