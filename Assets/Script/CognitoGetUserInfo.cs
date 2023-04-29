using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using Amazon;

public class CognitoGetUserInfo : MonoBehaviour
{

    private AmazonCognitoIdentityProviderClient cognitoService; // cognitoService ��ü ����

    private string access_token;

    private void Start()
    {
        // Amazon Cognito ���� ���� ���� (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito ������ ��ü�� �ν��Ͻ�ȭ
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);

        access_token = PlayerInfo.player_info.access_token; // �α��� ���� �� ��ȯ���� ������ ��ū
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
                Debug.Log("User Info: " + response.UserAttributes);
                foreach (var attribute in response.UserAttributes)
                {
                    // UserAttributes ����Ʈ�� ��ȸ�ϸ� ���� ���� ��������
                    if (attribute.Name == "nickname") PlayerInfo.player_info.nickname = attribute.Value;
                    if (attribute.Name == "email") PlayerInfo.player_info.email = attribute.Value;
                    Debug.LogFormat("{0} : {1}", attribute.Name, attribute.Value);
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
