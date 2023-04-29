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

public class CognitoSignUp : MonoBehaviour
{
    private AmazonCognitoIdentityProviderClient cognitoService; // cognitoService ��ü ����

    public InputField signUpEMail;
    public InputField signUpPassWord;
    public InputField signUpNickName;

    public Button signUpSignUp;
    public Button signUpBackToSignIn;

    private string email;
    private string password;
    private string nickname;

    private void Awake()
    {
        if (signUpNickName.GetComponent<InputField>().text == null)
        {
            nickname = " ";
        }
        else
        {
            nickname = signUpNickName.GetComponent<InputField>().text;
        }

        if (signUpEMail.GetComponent<InputField>().text == null)
        {
            email = " ";
        }
        else
        {
            email = signUpEMail.GetComponent<InputField>().text;

        }

        if (signUpPassWord.GetComponent<InputField>().text == null)
        {
            password = " ";
        }
        else
        {
            password = signUpPassWord.GetComponent<InputField>().text;

        }
    }

    void Start()
    {
        // Amazon Cognito ���� ���� ���� (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito ������ ��ü�� �ν��Ͻ�ȭ
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);
        
        signUpSignUp.onClick.AddListener(SignUp); // ����Ƽ �����ϸ� �ٷ� ȸ������ �����Բ� �س���
        signUpBackToSignIn.onClick.AddListener(changeSignInScene);
    }

    void changeSignInScene()
    {
        SceneManager.LoadScene("SignInScene");
    }

    public async void SignUp()
    {
        // ����� ��� ��û ����
        var signUpRequest = new SignUpRequest
        {
            ClientId = "1luokqrq9t4j8gag5kbnphunvu", // Ŭ���̾�Ʈ ID (��� ����ڵ� ����)
            Username = signUpNickName.ToString(), // ����� �̸�
            Password = signUpPassWord.ToString(), // ��й�ȣ
            UserAttributes = new List<AttributeType> // aws���� �츮�� ���� ������ �ʼ� �Ӽ�
            {
                new AttributeType { Name = "email", Value = signUpEMail.ToString() },
                new AttributeType { Name = "nickname", Value = signUpNickName.ToString() }
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