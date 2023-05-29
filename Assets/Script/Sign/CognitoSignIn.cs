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
using System.Diagnostics.Eventing.Reader;

public class CognitoSignIn : MonoBehaviour
{
    private string _clientId = "1luokqrq9t4j8gag5kbnphunvu"; // 클라이언트 ID

    public TMP_InputField sign_in_nickname;
    public TMP_InputField sign_in_password;
    public Button sign_in_sign_in_btn;
    public Button sign_in_sign_up_btn;

    static public string lastLoggedInUserId;
    static public string lastLoggedInPassword;
    private void Start()
    {
        sign_in_sign_in_btn.onClick.AddListener(ClickSignInBtn);
        sign_in_sign_up_btn.onClick.AddListener(ClickSignUpBtn);

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
            Debug.Log("SignInAsync Function Called");
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
                PlayerInfo.playerInfo.nickname = sign_in_nickname.text;

                // 로그인 성공 후 PlayerPrefs에 사용자 ID와 비밀번호 저장
                PlayerPrefs.SetString("LastLoggedInUserId", sign_in_nickname.text);
                PlayerPrefs.SetString("LastLoggedInPassword", sign_in_password.text);
                PlayerPrefs.Save();

                //if (캐릭터를 생성한 적이 있다면) {
                //    SceneManager.LoadScene("MainScene");
                //}
                //else
                //{
                //    SceneManager.LoadScene("CharacterCreateScene");
                //}

                SceneManager.LoadScene("CharacterCreateScene");
                return authResponse.AuthenticationResult.AccessToken;
            }
            else
            {
                throw new Exception("Authentication failed.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    // 로그인 함수를 비동기식으로 실행
    private IEnumerator SignInCoroutine()
    {
        yield return SignInAsync(sign_in_nickname.text, sign_in_password.text);
        
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