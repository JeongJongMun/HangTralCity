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
    private string _clientId = "1luokqrq9t4j8gag5kbnphunvu"; // Ŭ���̾�Ʈ ID

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

        // �� ���� �� PlayerPrefs���� ���������� �α��� �� ����� ID�� ��й�ȣ ��������
        lastLoggedInUserId = PlayerPrefs.GetString("LastLoggedInUserId");
        lastLoggedInPassword = PlayerPrefs.GetString("LastLoggedInPassword");

        // ���������� �α����� ����ڰ� �ִٸ� �ڵ� �α��� ����
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
                // ���� ������ Access Token ��ȯ, �г��� ��ȯ
                PlayerInfo.playerInfo.access_token = authResponse.AuthenticationResult.AccessToken;
                PlayerInfo.playerInfo.nickname = sign_in_nickname.text;

                // �α��� ���� �� PlayerPrefs�� ����� ID�� ��й�ȣ ����
                PlayerPrefs.SetString("LastLoggedInUserId", sign_in_nickname.text);
                PlayerPrefs.SetString("LastLoggedInPassword", sign_in_password.text);
                PlayerPrefs.Save();

                //if (ĳ���͸� ������ ���� �ִٸ�) {
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

    // �α��� �Լ��� �񵿱������ ����
    private IEnumerator SignInCoroutine()
    {
        yield return SignInAsync(sign_in_nickname.text, sign_in_password.text);
        
        yield break;
    }
    // �ڵ� �α��� �� ���� �Լ�
    private IEnumerator AutoSignInCoroutine()
    {
        yield return SignInAsync(lastLoggedInUserId, lastLoggedInPassword);

        yield break;
    }

    // �α��� ��ư Ŭ�� �� �ڷ�ƾ ����
    public void ClickSignInBtn()
    {
        StartCoroutine(SignInCoroutine());
    }
    // ȸ������ ��ư Ŭ�� ��
    public void ClickSignUpBtn()
    {
        SceneManager.LoadScene("SignUpScene");
    }
}