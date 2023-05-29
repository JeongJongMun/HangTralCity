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

    AmazonCognitoIdentityProviderClient cognitoService; // cognitoService 객체 선언

    string access_token;

    void Start()
    {
        // Amazon Cognito 인증 정보 설정 (IdentityPool, Region)
        var credentials = new CognitoAWSCredentials("ap-northeast-2:49b91ce6-d3db-47f2-af63-2a9db71ca292", RegionEndpoint.APNortheast2);

        // Amazon Cognito 서비스의 객체를 인스턴스화
        cognitoService = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.APNortheast2);

        access_token = PlayerInfo.playerInfo.access_token; // 로그인 성공 후 반환받은 엑세스 토큰

        GetUserInformation(); // 메인 화면 시작시 자동으로 유저 정보 가져오기
    }

    async void GetUserInformation()
    {
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
                StartCoroutine(GetCustomInfo(PlayerInfo.playerInfo.nickname)); // DB에서 캐릭터 커스터마이징 정보 가져오기
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
        request.Dispose(); // 메모리 누수 방지를 위해 추가
    }

}
