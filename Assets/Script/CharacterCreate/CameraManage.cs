using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CameraManage : MonoBehaviour
{
    WebCamTexture camTexture;

    public RawImage cameraViewImage; // ī�޶� ������ ȭ��

    public Button takePictureBtn, backBtn;

    private void Start()
    {
        takePictureBtn.onClick.AddListener(TakePicture);
        backBtn.onClick.AddListener(BackToCharacterCreateScene);
        CameraOn();

    }
    //AWS EC2�� �г��� ���ε带 ���� �Լ�
    IEnumerator UploadToEC2(string URL, string nickname)
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Nickname Upload to EC2 Complete!");
        }
        www.Dispose();
    }
    IEnumerator GetData1(string URL)
    {
        //yield return new WaitForSeconds(3f);

        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            int value;
            if (int.TryParse(www.downloadHandler.text, out value))
            {
                Debug.Log("Received value from EC2: " + value);
            }
            else
            {
                Debug.LogError("Failed to parse int value from response: " + www.downloadHandler.text);
            }
        }
        www.Dispose();
    }

    IEnumerator GetData2(string URL)
    {
        //yield return new WaitForSeconds(3f);

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.LogFormat("JsonResponse:{0}", jsonResponse);

                // JSON �����͸� Dictionary ���·� �Ľ�
                Dictionary<string, object> responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);

                // 'nickname' Ű�� ���� nickname ���� ���� (string)
                string nickname = string.Empty;
                if (responseData.ContainsKey("nickname"))
                {
                    nickname = responseData["nickname"].ToString();
                }

                // 'label' Ű�� ���� label ���� ���� (int)
                int label = 0;
                if (responseData.ContainsKey("label"))
                {
                    if (int.TryParse(responseData["label"].ToString(), out int parsedLabel))
                    {
                        label = parsedLabel;
                    }
                }
                Debug.LogFormat("Nickname:{0}, Label:{1}", nickname, label);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }
    IEnumerator GetPredictedCharacterFromS3(string URL)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            // �̹��� �ε� �Ϸ���� ���
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.LogFormat("Predicted Value: {0}", www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Image download failed. Error: " + www.error);
            }
        }
    }


    public void CameraOn() //ī�޶� �ѱ�
    {
        // ī�޶� ���� Ȯ��
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // ī�޶� ���ٸ�
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No Camera!");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices; // ����Ʈ���� ī�޶� ������ ��� ������.
        int selectedCameraIndex = -1;

        //�ĸ� ī�޶� ã��
        for (int i = 0; i < devices.Length; i++)
        {

            //if (devices[i].isFrontFacing == false) // �ĸ� ī�޶�
            if (devices[i].isFrontFacing) // ���� ī�޶�
            {
                selectedCameraIndex = i;
                break;
            }
        }

        //ī�޶� �ѱ�
        if (selectedCameraIndex >= 0)
        {
            // ���õ� ī�޶� ������.
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);

            camTexture.requestedFPS = 30;

            cameraViewImage.texture = camTexture; // ������ raw Image�� �Ҵ�.

            camTexture.Play(); // ī�޶� �����ϱ�
        }

    }

    public void CameraOff() // ī�޶� ����
    {
        if (camTexture != null)
        {
            camTexture.Stop(); // ī�޶� ����
            Destroy(camTexture); // ī�޶� ��ü �ݳ�
            camTexture = null; // ���� �ʱ�ȭ
        }
    }


    public void TakePicture() // ���� �Կ��ϱ�
    {
        Texture2D picture = new Texture2D(camTexture.width, camTexture.height);
        picture.SetPixels(camTexture.GetPixels());
        picture.Apply();

        // ������ �н��� �����ϰ� 224x224 ������� ����
        //Texture2D resizePicture = ScaleTexture(picture, 224, 224);

        // �̹����� ���Ϸ� ����
        byte[] bytes = picture.EncodeToPNG();
        string fileName = "picture.png";
        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // ������ ���� ��� ���
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);

        // S3 ��Ŷ�� �Կ��� ���� ���ε�
        //_ = S3Manage.s3Manage.UploadToS3(Application.persistentDataPath + "/" + fileName, PlayerInfo.playerInfo.nickname);
        _ = S3Manage.s3Manage.UploadToS3(Application.persistentDataPath + "/" + fileName, "JJM" + ".png");

        // EC2 �ν��Ͻ����� ����� Flask �� ������ �г��� ���ε�
        StartCoroutine(UploadToEC2("http://13.124.0.232/", "0.png"));
        StartCoroutine(GetData1("http://13.124.0.232/"));
        //StartCoroutine(GetPredictedCharacterFromS3("https://predicted-character.s3.ap-northeast-2.amazonaws.com/"+ PlayerInfo.playerInfo.nickname));

        //CharacterCreateManage.isPredicted = true;
        //CameraOff();
        //SceneManager.LoadScene("CharacterCreateScene");
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    public void BackToCharacterCreateScene()
    {
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }


}