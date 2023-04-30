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
            // ���õ� �ĸ� ī�޶� ������.
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);

            camTexture.requestedFPS = 30; // ī�޶� �����Ӽ���

            cameraViewImage.texture = camTexture; // ������ raw Image�� �Ҵ�.

            camTexture.Play(); // ī�޶� �����ϱ�
        }

    }

    public void CameraOff() // ī�޶� ����
    {
        if (camTexture != null)
        {
            camTexture.Stop(); // ī�޶� ����
            WebCamTexture.Destroy(camTexture); // ī�޶� ��ü�ݳ�
            camTexture = null; // ���� �ʱ�ȭ
        }
    }


    public void TakePicture() // ���� �Կ��ϱ�
    {
        Texture2D picture = new Texture2D(camTexture.width, camTexture.height);
        picture.SetPixels(camTexture.GetPixels());
        picture.Apply();

        // �̹����� ���Ϸ� ����
        byte[] bytes = picture.EncodeToPNG();
        string fileName = "picture.png";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // ������ ���� ��� ���
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);
    }

    public void BackToCharacterCreateScene()
    {
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }

}