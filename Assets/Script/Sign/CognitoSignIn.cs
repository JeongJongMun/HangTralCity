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
    private string _clientId = "1luokqrq9t4j8gag5kbnphunvu"; // Ŭ���̾�Ʈ ID

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
                PlayerInfo.playerInfo.nickname = username;

                // �α��� ���� �� PlayerPrefs�� ����� ID�� ��й�ȣ ����
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

    // �α��� �Լ��� �񵿱������ ����
    private IEnumerator SignInCoroutine()
    {
        yield return SignInAsync(nicknameInputField.text, passwordInputField.text);
        
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