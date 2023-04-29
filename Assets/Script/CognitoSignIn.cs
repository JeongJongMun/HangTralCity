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
using Unity.VisualScripting;

public class CognitoSignIn : MonoBehaviour
{
    private string _clientId = "1luokqrq9t4j8gag5kbnphunvu"; // 클라이언트 ID
    private string _userPoolId = "ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292"; // 유저 풀 ID

    public TMP_InputField sign_in_nickname;
    public TMP_InputField sign_in_password;
    public Button sign_in_sign_in_btn;
    public Button sign_in_sign_up_btn;


    public async Task<string> SignInAsync(string username, string password)
    {
        using var provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.APNortheast2);
        try
        {
            Debug.Log("SignInAsync F Called");
            var authRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string> { { "USERNAME", username }, { "PASSWORD", password } }
            };
            var authResponse = await provider.InitiateAuthAsync(authRequest);

            if (authResponse.AuthenticationResult != null)
            {
                // 인증 성공시 Access Token 반환
                PlayerInfo.player_info.access_token = authResponse.AuthenticationResult.AccessToken;
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
    private IEnumerator MyCoroutine()
    {
        yield return SignInAsync(sign_in_nickname.text, sign_in_password.text);
        
        yield break;
    }

    // 로그인 버튼 클릭 시 코루틴 시작
    public void SignInBtnClicked()
    {
        StartCoroutine(MyCoroutine());
    }
}