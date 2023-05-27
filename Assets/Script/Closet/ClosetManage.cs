using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class ClosetManage : MonoBehaviour
{
    public GameObject eyeScrollView, hatScrollView; // hat, eye ��ũ�� �� ������Ʈ
    public Button eyeScrollViewBtn, hatScrollViewBtn; // hat, eye ��ũ�� �� ���� ��ư
    public GameObject[] eyeToggle, hatToggle; // hat, eye ī�װ��� Ŀ�� ���� ��۵�
    public GameObject character;

    public class CustomInfoResponse
    {
        public int eyeCustom;
        public int hatCustom;
    }

    public class CustomInfoResponseWrapper
    {
        public int statusCode;
        public string body;
    }

    void Start()
    {
        // ��ũ�Ѻ� ���� ��ư Ŭ����
        eyeScrollViewBtn.onClick.AddListener(ClickEyeCategory);
        hatScrollViewBtn.onClick.AddListener(ClickHatCategory);

        // ��� Ŭ���� �Լ� ȣ��
        for (int i = 0; i < eyeToggle.Length; i++)
        {
            int index = i;
            eyeToggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleEye(index); });
        }
        for (int i = 0; i < hatToggle.Length; i++)
        {
            int index = i;
            hatToggle[i].GetComponent<Toggle>().onValueChanged.AddListener(delegate { ToggleHat(index); });
        }

        StartCoroutine(GetCustomInfo(PlayerInfo.playerInfo.nickname));


    }

    private void ToggleEye(int n)
    {
        if (eyeToggle[n].GetComponent<Toggle>().isOn)
        {
            // �ٸ� ��۵� OFF
            for (int i = 0; i < eyeToggle.Length; i++) if (i != n) eyeToggle[i].GetComponent<Toggle>().isOn = false;
            // ���� ����
            PlayerInfo.playerInfo.eyeCustom = n;
            // ������ ��� �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            // null �̹����� ����
            PlayerInfo.playerInfo.eyeCustom = 11;
            // �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        // DB ������Ʈ ����
        StartCoroutine(UpdateCustomInfo(PlayerInfo.playerInfo.nickname, "eyeCustom", PlayerInfo.playerInfo.eyeCustom));

    }
    private void ToggleHat(int n)
    {
        if (hatToggle[n].GetComponent<Toggle>().isOn)
        {
            // �ٸ� ��۵� OFF
            for (int i = 0; i < hatToggle.Length; i++) if (i != n) hatToggle[i].GetComponent<Toggle>().isOn = false;
            // ���� ����
            PlayerInfo.playerInfo.hatCustom = n;
            // ������ ��� �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            // null �̹����� ����
            PlayerInfo.playerInfo.hatCustom = 8;
            // �̹��� ����
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        // DB ������Ʈ ����
        StartCoroutine(UpdateCustomInfo(PlayerInfo.playerInfo.nickname, "hatCustom", PlayerInfo.playerInfo.hatCustom));


    }
    private IEnumerator UpdateCustomInfo(string nickname, string attribute, int value)
    {
        string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/ch-custom";

        // Create a request object
        var request = new UnityWebRequest(apiGatewayUrl, "PUT");
        request.downloadHandler = new DownloadHandlerBuffer();

        // Create a JSON object for the request body
        string jsonBody = $"{{\"nickname\":\"{nickname}\",\"attribute\":\"{attribute}\",\"value\":{value}}}";
        Debug.LogFormat("jsonBody : {0}", jsonBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Update successful!");
        }
        else
        {
            Debug.Log($"Update failed: {request.error}");
        }
        request.Dispose(); // �޸� ���� ������ ���� �߰�
    }

    private IEnumerator GetCustomInfo(string nickname)
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
            character.GetComponent<PlayerScript>().SetCharacterCustom();
        }
        else
        {
            Debug.Log($"GetCustomInfo failed: {request.error}");
            Debug.Log($"GetCustomInfo Response Code: {(int)request.responseCode}");
            Debug.Log($"GetCustomInfo Response: {request.downloadHandler.text}");
        }
        request.Dispose(); // �޸� ���� ������ ���� �߰�
    }


    private void ClickEyeCategory()
    {
        eyeScrollView.SetActive(true);
        hatScrollView.SetActive(false);
    }
    private void ClickHatCategory()
    {
        eyeScrollView.SetActive(false);
        hatScrollView.SetActive(true);
    }



}
