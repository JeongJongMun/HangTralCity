using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

[System.Serializable]
public class PredictedAnimal
{
    public string nickname;
    public int label;
    public int percentage;
}

public class CameraManage : MonoBehaviour
{
    [Header("RawImage")]
    public RawImage cameraViewImage; // ī�޶� ������ ȭ��

    [Header("Button")]
    public Button takePictureBtn; // ���� �Կ� ��ư
    public Button backBtn; // �ڷΰ��� ��ư
    public Button galleryBtn; // ������ ��ư

    [Header("Panel")]
    public GameObject predictionPanel; // ������ ���� �� UI �г�

    // ī�޶�
    WebCamDevice[] devices;
    WebCamTexture frontCameraTexture = null;

    // ������
    PredictedAnimal predictedData;

    private void Start()
    {
        takePictureBtn.onClick.AddListener(TakePicture);
        backBtn.onClick.AddListener(BackToCharacterCreateScene);
        galleryBtn.onClick.AddListener(ClickGalleryBtn);
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
        }
        else
        {
            Debug.Log("Post Nickname to EC2 Successful");
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
                predictedData = JsonUtility.FromJson<PredictedAnimal>(jsonResponse);

                Debug.LogFormat("Get Predicted Character Successful \nNickname:{0}, Label:{1}, Percentage:{2}", predictedData.nickname, predictedData.label, predictedData.percentage);

                // Label�� Character Type ����
                CharacterCreateManage.predictedType = predictedData.label;
                CharacterCreateManage.percentage = predictedData.percentage;
            }
            else
            {
                Debug.Log("GET Predicted Character failed. Error: " + www.error);
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
            Debug.Log("No Camera");
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
        // ������ ������ �ǳ� ON
        predictionPanel.SetActive(true);

        // �Կ��� ������ Texture 2D�� ����
        Texture2D picture = new Texture2D(frontCameraTexture.width, frontCameraTexture.height);
        picture.SetPixels(frontCameraTexture.GetPixels());
        picture.Apply();


        // �̹����� 90�� �ݽð� �������� ȸ���Ͽ� ���Ϸ� ����
        Texture2D rotatedPicture = RotateTexture(picture, -90);
        byte[] bytes = rotatedPicture.EncodeToPNG();

        // �̹����� ���Ϸ� ����
        string fileName = PlayerInfo.playerInfo.nickname;
        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // ������ ���� ��� ���
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);

        // S3 ��Ŷ�� �Կ��� ���� ���ε�
        _ = S3Manage.s3Manage.PostToS3(Application.persistentDataPath + "/" + fileName, PlayerInfo.playerInfo.nickname);

        // EC2 �ν��Ͻ����� ����� Flask �� ������ �г��� ���ε�
        StartCoroutine(PostNicknameToEC2("43.202.19.142", PlayerInfo.playerInfo.nickname));

        // �𵨿��� ������ ���� �г����� S3�� ���� -> S3���� �г���, ���̺�, Ȯ�� ��������
        StartCoroutine(GetPredictedCharacterFromS3("https://predicted-character.s3.ap-northeast-2.amazonaws.com/" + PlayerInfo.playerInfo.nickname));
    }


    private Texture2D RotateTexture(Texture2D texture, float angle)
    {
        Texture2D rotatedTexture = new Texture2D(texture.height, texture.width);
        Color32[] pixels = texture.GetPixels32();
        Color32[] rotatedPixels = new Color32[rotatedTexture.width * rotatedTexture.height];

        int rotatedWidth = rotatedTexture.width;
        int rotatedHeight = rotatedTexture.height;

        for (int x = 0; x < rotatedWidth; x++)
        {
            for (int y = 0; y < rotatedHeight; y++)
            {
                int newIndex = y * rotatedWidth + x;
                int oldIndex = (rotatedWidth - x - 1) * rotatedHeight + y;

                rotatedPixels[newIndex] = pixels[oldIndex];
            }
        }

        rotatedTexture.SetPixels32(rotatedPixels);
        rotatedTexture.Apply();

        return rotatedTexture;
    }
    public void ClickGalleryBtn()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo seleted = new FileInfo(file);

            // �뷮 ���� (����Ʈ ���� -> 50�ް�)
            if (seleted.Length > 50000000) return;

            // ������ �����Ѵٸ� �ҷ�����
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(UploadImageToS3(file));
            }
        });
    }

    IEnumerator UploadImageToS3(string path)
    {
        // ������ ������ �ǳ� ON
        predictionPanel.SetActive(true);

        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0]; // Ȯ���ڸ� �ڸ��� ���� . �������� ����
        string savePath = Application.persistentDataPath + "/Image"; // �ҷ����� ���ο� �����ϱ�

        // ���� �ѹ��� �������� �ʾҴٸ�
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + fileName + ".png", fileData); // png�� ����

        _ = S3Manage.s3Manage.PostToS3(savePath + fileName + ".png", PlayerInfo.playerInfo.nickname); // S3�� ���ε�

        // 0.5�� ���� ��� -> S3�� �̹����� �ö� �ð��� ��
        yield return new WaitForSeconds(0.5f);

        // EC2 �ν��Ͻ����� ����� Flask �� ������ �г��� ���ε�
        StartCoroutine(PostNicknameToEC2("43.202.19.142", PlayerInfo.playerInfo.nickname));

        // �𵨿��� ������ ���� �г����� S3�� ���� -> S3���� �г���, ���̺�, Ȯ�� ��������
        StartCoroutine(GetPredictedCharacterFromS3("https://predicted-character.s3.ap-northeast-2.amazonaws.com/" + PlayerInfo.playerInfo.nickname));
    }

    public void BackToCharacterCreateScene()
    {
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }
}