using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CognitoSignIn : MonoBehaviour
{
    private string _clientId = "1luokqrq9t4j8gag5kbnphunvu"; // 클라이언트 ID

    [Header("InputField")]
    public TMP_InputField nicknameInputField;
    public TMP_InputField passwordInputField;

    [Header("Button")]
    public Button signInBtn;
    public Button signUpBtn;

    [Header("AutoLogin")]
    static public string lastLoggedInUserId;
    static public string lastLoggedInPassword;

    [Header("PopUp")]
    public PopUpManage popUpManage;

    private void Start()
    {
        signInBtn.onClick.AddListener(ClickSignInBtn);
        signUpBtn.onClick.AddListener(ClickSignUpBtn);

        // 앱 시작 시 PlayerPrefs에서 마지막으로 로그인 한 사용자 ID와 비밀번호 가져오기
        lastLoggedInUserId = PlayerPrefs.GetString("LastLoggedInUserId");
        lastLoggedInPassword = PlayerPrefs.GetString("LastLoggedInPassword");

        // 마지막으로 로그인한 사용자가 있다면 자동 로그인 실행
        if (!string.IsNullOrEmpty(lastLoggedInUserId) && !string.IsNullOrEmpty(lastLoggedInPassword))
        {
            StartCoroutine(AutoSignInCoroutine());
        }

    }


    public async Task<string> SignInAsync(string username, string password)
    {
        using var provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APNortheast2);
        try
        {
            var authRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string> { { "USERNAME", username }, { "PASSWORD", password } }
            };
            var authResponse = await provider.InitiateAuthAsync(authRequest);

            if (authResponse.AuthenticationResult != null)
            {
                // 인증 성공시 Access Token 반환, 닉네임 반환
                PlayerInfo.playerInfo.access_token = authResponse.AuthenticationResult.AccessToken;
                PlayerInfo.playerInfo.nickname = username;

                // 로그인 성공 후 PlayerPrefs에 사용자 ID와 비밀번호 저장
                PlayerPrefs.SetString("LastLoggedInUserId", nicknameInputField.text);
                PlayerPrefs.SetString("LastLoggedInPassword", passwordInputField.text);
                PlayerPrefs.Save();

                Debug.Log("SignIn Successful");

                SceneManager.LoadScene("CharacterCreateScene");
                return authResponse.AuthenticationResult.AccessToken;
            }
            else
            {
                throw new Exception("SignIn Failed.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            popUpManage.ShowPopup(e.Message);
            return null;
        }
    }

    // 로그인 함수를 비동기식으로 실행
    private IEnumerator SignInCoroutine()
    {
        yield return SignInAsync(nicknameInputField.text, passwordInputField.text);
        
        yield break;
    }
    // 자동 로그인 시 실행 함수
    private IEnumerator AutoSignInCoroutine()
    {
        yield return SignInAsync(lastLoggedInUserId, lastLoggedInPassword);

        yield break;
    }

    // 로그인 버튼 클릭 시 코루틴 시작
    public void ClickSignInBtn()
    {
        StartCoroutine(SignInCoroutine());
    }
    // 회원가입 버튼 클릭 시
    public void ClickSignUpBtn()
    {
        SceneManager.LoadScene("SignUpScene");
    }
}