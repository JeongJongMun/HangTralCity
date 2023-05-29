using System.Collections.Generic;
using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

public class CognitoSignUp : MonoBehaviour
{
    private AmazonCognitoIdentityProviderClient cognitoService; // cognitoService ��ü ����

    public TMP_InputField sign_up_email;
    public TMP_InputField sign_up_password;
    public TMP_InputField sign_up_nickname;

    public Button sign_up_sign_up_btn;
    public Button sign_up_back_btn;

    private void Start()
    {

        // Amazon Cognito ���� ���� ���� (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito ������ ��ü�� �ν��Ͻ�ȭ
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);
        
        sign_up_sign_up_btn.onClick.AddListener(SignUp); // ����Ƽ �����ϸ� �ٷ� ȸ������ �����Բ� �س���
        sign_up_back_btn.onClick.AddListener(ClickBackBtn);
    }

    public async void SignUp()
    {
        // ����� ��� ��û ����
        var signUpRequest = new SignUpRequest
        {
            ClientId = "1luokqrq9t4j8gag5kbnphunvu", // Ŭ���̾�Ʈ ID (��� ����ڵ� ����)
            Username = sign_up_nickname.text.ToString(), // ����� �̸�
            Password = sign_up_password.text.ToString(), // ��й�ȣ
            UserAttributes = new List<AttributeType> // aws���� �츮�� ���� ������ �ʼ� �Ӽ�
            {
                new AttributeType { Name = "email", Value = sign_up_email.text.ToString() },
                new AttributeType { Name = "nickname", Value = sign_up_nickname.text.ToString() }
            }
        };


        // ���� ó�� �� ���� �� ���� ó��
        try
        {
            var response = await cognitoService.SignUpAsync(signUpRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                Debug.Log("Sign-Up Successful.");
                var apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/user-singup";
                var httpClient = new HttpClient();
                var requestBody = new
                {
                    data = new
                    {
                        PK = sign_up_nickname.text.ToString(),

                    }
                };
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var apiGatewayResponse = await httpClient.PostAsync(apiGatewayUrl, requestContent);
                if (apiGatewayResponse.StatusCode == HttpStatusCode.OK)
                {
                    Debug.Log("Lambda function invoked successfully.");
                }
                else
                {
                    Debug.Log("Failed to invoke Lambda function. Response: " + apiGatewayResponse.StatusCode);
                }
                SceneManager.LoadScene("SignInScene");
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
    public void ClickBackBtn()
    {
        SceneManager.LoadScene("SignInScene");
    }
}