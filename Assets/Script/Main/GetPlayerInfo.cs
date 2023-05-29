using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using Amazon;
using static ClosetManage;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class GetPlayerInfo : MonoBehaviour
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

        GetUserInformation(); // ���� ȭ�� ���۽� �ڵ����� ���� ���� ��������
    }

    async void GetUserInformation()
    {
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
                StartCoroutine(GetCustomInfo(PlayerInfo.playerInfo.nickname)); // DB���� ĳ���� Ŀ���͸���¡ ���� ��������
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
    IEnumerator GetCustomInfo(string nickname)
    {
        string apiGatewayUrl2 = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/ch-custom";

        var request = new UnityWebRequest(apiGatewayUrl2, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"nickname\":\"{nickname}\"}}";
        Debug.LogFormat("jsonBody : {0}", jsonBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GetCustomInfo successful!");

            Debug.Log("GetCustomInfo Response: " + request.downloadHandler.text);


            // Parse the outer JSON
            var wrapper = JsonUtility.FromJson<CustomInfoResponseWrapper>(request.downloadHandler.text);
            // Then parse the inner JSON
            var response = JsonUtility.FromJson<CustomInfoResponse>(wrapper.body);

            // Update the customization in your game
            PlayerInfo.playerInfo.eyeCustom = response.eyeCustom;
            PlayerInfo.playerInfo.hatCustom = response.hatCustom;
            //character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            Debug.Log($"GetCustomInfo failed: {request.error}");
            Debug.Log($"GetCustomInfo Response Code: {(int)request.responseCode}");
            Debug.Log($"GetCustomInfo Response: {request.downloadHandler.text}");
        }
        request.Dispose(); // �޸� ���� ������ ���� �߰�
    }

}
