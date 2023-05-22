using UnityEngine;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using Amazon;

public class CognitoGetUserInfo : MonoBehaviour
{

    AmazonCognitoIdentityProviderClient cognitoService; // cognitoService 객체 선언

    string access_token;

    void Start()
    {
        // Amazon Cognito 인증 정보 설정 (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito 서비스의 객체를 인스턴스화
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);

        access_token = PlayerInfo.playerInfo.access_token; // 로그인 성공 후 반환받은 엑세스 토큰

        GetUserInfo(); // 메인 화면 시작시 자동으로 유저 정보 가져오기


    }

    public async void GetUserInfo()
    {
        Debug.Log("GetUserInfo Called");
        // 사용자 정보 가져오기 요청 생성
        var getUserInfoRequest = new GetUserRequest
        {
            AccessToken = access_token // 엑세스 토큰 전달
        };

        // 예외 처리 및 성공 및 실패 처리
        try
        {
            var response = await cognitoService.GetUserAsync(getUserInfoRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                foreach (var attribute in response.UserAttributes)
                {
                    // UserAttributes 리스트를 순회하며 유저 정보 가져오기
                    if (attribute.Name == "nickname") PlayerInfo.playerInfo.nickname = attribute.Value;
                    if (attribute.Name == "email") PlayerInfo.playerInfo.email = attribute.Value;
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
