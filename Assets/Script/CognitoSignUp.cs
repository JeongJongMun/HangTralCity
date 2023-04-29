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
    private AmazonCognitoIdentityProviderClient cognitoService; // cognitoService 객체 선언

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
        // Amazon Cognito 인증 정보 설정 (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito 서비스의 객체를 인스턴스화
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);
        
        signUpSignUp.onClick.AddListener(SignUp); // 유니티 실행하면 바로 회원가입 보내게끔 해놨음
        signUpBackToSignIn.onClick.AddListener(changeSignInScene);
    }

    void changeSignInScene()
    {
        SceneManager.LoadScene("SignInScene");
    }

    public async void SignUp()
    {
        // 사용자 등록 요청 생성
        var signUpRequest = new SignUpRequest
        {
            ClientId = "1luokqrq9t4j8gag5kbnphunvu", // 클라이언트 ID (모든 사용자들 공통)
            Username = signUpNickName.ToString(), // 사용자 이름
            Password = signUpPassWord.ToString(), // 비밀번호
            UserAttributes = new List<AttributeType> // aws에서 우리가 직접 설정한 필수 속성
            {
                new AttributeType { Name = "email", Value = signUpEMail.ToString() },
                new AttributeType { Name = "nickname", Value = signUpNickName.ToString() }
            }
        };


        // 예외 처리 및 성공 및 실패 처리
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