using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class CameraManage : MonoBehaviour
{
    WebCamTexture camTexture;

    public RawImage cameraViewImage; // 카메라가 보여질 화면

    public Button take_picture_btn, back_btn;
    private void Start()
    {
        take_picture_btn.onClick.AddListener(TakePicture);
        back_btn.onClick.AddListener(BackToCharacterCreateScene);
        CameraOn();
    }

    public void CameraOn() //카메라 켜기
    {
        // 카메라 권한 확인
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // 카메라가 없다면
        if (WebCamTexture.devices.Length == 0) 
        {
            Debug.Log("No Camera!");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices; // 스마트폰의 카메라 정보를 모두 가져옴.
        int selectedCameraIndex = -1;

        //후면 카메라 찾기
        for (int i = 0; i < devices.Length; i++)
        {

            //if (devices[i].isFrontFacing == false) // 후면 카메라
            if (devices[i].isFrontFacing) // 전면 카메라
            {
                selectedCameraIndex = i;
                break;
            }
        }

        //카메라 켜기
        if (selectedCameraIndex >= 0)
        {
            // 선택된 후면 카메라를 가져옴.
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);

            camTexture.requestedFPS = 30; // 카메라 프레임설정

            cameraViewImage.texture = camTexture; // 영상을 raw Image에 할당.

            camTexture.Play(); // 카메라 시작하기
        }

    }

    public void CameraOff() // 카메라 끄기
    {
        if (camTexture != null)
        {
            camTexture.Stop(); // 카메라 정지
            WebCamTexture.Destroy(camTexture); // 카메라 객체반납
            camTexture = null; // 변수 초기화
        }
    }


    public void TakePicture() // 사진 촬영하기
    {
        Texture2D picture = new Texture2D(camTexture.width, camTexture.height);
        picture.SetPixels(camTexture.GetPixels());
        picture.Apply();

        // 이미지를 파일로 저장
        byte[] bytes = picture.EncodeToPNG();
        string fileName = "picture.png";
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // 저장한 파일 경로 출력
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);
    }

    public void BackToCharacterCreateScene()
    {
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }

}