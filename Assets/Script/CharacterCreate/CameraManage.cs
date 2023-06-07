using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;

public class CameraManage : MonoBehaviour
{
    [Header("RawImage")]
    public RawImage cameraViewImage; // ī�޶� ������ ȭ��

    [Header("Button")]
    public Button takePictureBtn;
    public Button backBtn;

    [Header("Panel")]
    public GameObject predictionPanel;

    public TMP_Text debug;
    // ī�޶�
    WebCamDevice[] devices;
    WebCamTexture frontCameraTexture = null;

    private void Start()
    {
        takePictureBtn.onClick.AddListener(TakePicture);
        backBtn.onClick.AddListener(BackToCharacterCreateScene);
        CameraOn();

    }

    // AWS EC2 �������� ���� S3���� ������ ���� �̸�(�г���) ���ε带 ���� �Լ�
    IEnumerator PostNicknameToEC2(string URL, string nickname)
    {
        // S3�� ������ ���ε� �� �ð� 1�� ���� ���
        yield return new WaitForSeconds(1f);

        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Post Nickname To EC2 Failed : " + www.error);
            debug.text += "Post Nickname To EC2 Failed : " + www.error;
        }
        else
        {
            Debug.Log("Post Nickname to EC2 Successful");
            debug.text += "Post Nickname to EC2 Successful";
        }
        www.Dispose();
    }
    // ���� ������ ������ �����͸� S3�� ���� -> S3���� ������ ��������
    IEnumerator GetPredictedCharacterFromS3(string URL)
    {
        // ���� ���ư� �ð� 3�� ���� ���
        yield return new WaitForSeconds(3f);

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            // �̹��� �ε� �Ϸ���� ���
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;

                // JSON �����͸� Dictionary ���·� �Ľ�
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);

                // �г��� ����
                string nickname = string.Empty;
                if (responseData.ContainsKey("nickname"))
                {
                    nickname = responseData["nickname"].ToString();
                }

                // Ȯ�� �� ����
                int percentage = 0;
                if (responseData.ContainsKey("percentage"))
                {
                    if (int.TryParse(responseData["percentage"].ToString(), out int parsedPercentage))
                    {
                        percentage = parsedPercentage;
                    }
                }

                // ���̺� ����
                int label = 0;
                if (responseData.ContainsKey("label"))
                {
                    if (int.TryParse(responseData["label"].ToString(), out int parsedLabel))
                    {
                        label = parsedLabel;
                    }
                }
                Debug.LogFormat("Nickname:{0}, Label:{1}, Percentage:{2}", nickname, label, percentage);
                debug.text += "Nickname:" + nickname + ", Label:" + label + ", Percentage:" + percentage;
                // Label�� Character Type ����
                CharacterCreateManage.predictedType = label;
                CharacterCreateManage.percentage = percentage;
            }
            else
            {
                Debug.Log("GET Predicted Character failed. Error: " + www.error);
                debug.text += "GET Predicted Character failed. Error: " + www.error;
            }
        }

        CharacterCreateManage.isPredicted = true;
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }


    public void CameraOn() // ī�޶� �ѱ�
    {
        // ī�޶� ���� Ȯ��
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) Permission.RequestUserPermission(Permission.Camera);

        // ī�޶� ���ٸ�
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No Camera!");
            return;
        }

        // ����Ʈ���� ī�޶� ������ ��� ������.
        devices = WebCamTexture.devices;

        // ���� ī�޶� ã��
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                // ���� ī�޶� ������
                frontCameraTexture = new WebCamTexture(devices[i].name);
                frontCameraTexture.requestedFPS = 60;

                // ���� ī�޶� ������ raw Image�� �Ҵ�.
                cameraViewImage.texture = frontCameraTexture;

                // ī�޶� �����ϱ�
                frontCameraTexture.Play();
            }
        }
    }

    public void CameraOff() // ī�޶� ����
    {
        if (frontCameraTexture != null)
        {
            frontCameraTexture.Stop(); // ī�޶� ����
            Destroy(frontCameraTexture); // ī�޶� ��ü �ݳ�
            frontCameraTexture = null; // ���� �ʱ�ȭ
        }
    }

    public void TakePicture() // ���� �Կ��ϱ�
    {
        // ������ ������.. �ǳ� ON
        predictionPanel.SetActive(true);

        Texture2D picture = new Texture2D(frontCameraTexture.width, frontCameraTexture.height);
        picture.SetPixels(frontCameraTexture.GetPixels());
        picture.Apply();

        // �̹����� ���Ϸ� ����
        byte[] bytes = picture.EncodeToPNG();
        string fileName = PlayerInfo.playerInfo.nickname;
        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // ������ ���� ��� ���
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);

        // S3 ��Ŷ�� �Կ��� ���� ���ε�
        _ = S3Manage.s3Manage.PostPictureToS3(Application.persistentDataPath + "/" + fileName, PlayerInfo.playerInfo.nickname);

        // EC2 �ν��Ͻ����� ����� Flask �� ������ �г��� ���ε�
        StartCoroutine(PostNicknameToEC2("http://43.202.19.142", PlayerInfo.playerInfo.nickname));
        // �𵨿��� ������ ���� �г����� S3�� ���� -> S3���� �г���, ���̺�, Ȯ�� ��������
        StartCoroutine(GetPredictedCharacterFromS3("https://predicted-character.s3.ap-northeast-2.amazonaws.com/" + PlayerInfo.playerInfo.nickname));
    }

    public void BackToCharacterCreateScene()
    {
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }
}