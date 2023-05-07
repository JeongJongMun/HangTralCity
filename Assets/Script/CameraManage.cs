using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class CameraManage : MonoBehaviour
{
    WebCamTexture camTexture;

    public RawImage cameraViewImage; // ī�޶� ������ ȭ��

    public Button take_picture_btn, back_btn;

    private void Start()
    {
        take_picture_btn.onClick.AddListener(TakePicture);
        back_btn.onClick.AddListener(BackToCharacterCreateScene);
        CameraOn();

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
            Destroy(camTexture); // ī�޶� ��ü�ݳ�
            camTexture = null; // ���� �ʱ�ȭ
        }
    }


    public void TakePicture() // ���� �Կ��ϱ�
    {
        Texture2D picture = new Texture2D(camTexture.width, camTexture.height);
        picture.SetPixels(camTexture.GetPixels());
        picture.Apply();

        // ������ �н��� �����ϰ� 224x224 ������� ����
        Texture2D resizePicture = ScaleTexture(picture, 224, 224);

        Debug.LogFormat("ResizedPhoto width:{0}, height{1}", resizePicture.width, resizePicture.height);

        // �̹����� ���Ϸ� ����
        byte[] bytes = resizePicture.EncodeToPNG();
        string fileName = "picture.png";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // ������ ���� ��� ���
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);

        // S3 ��Ŷ�� �Կ��� ���� ���ε�
        S3Manage.s3Manage.UploadToS3(Application.persistentDataPath + "/" + fileName, PlayerInfo.playerInfo.nickname);

        CharacterCreateManage.isPredicted = true;
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
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