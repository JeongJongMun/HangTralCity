using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class CameraManage : MonoBehaviour
{
    WebCamTexture camTexture;

    public RawImage cameraViewImage; // 카메라가 보여질 화면

    public Button takePictureBtn, backBtn;

    private void Start()
    {
        takePictureBtn.onClick.AddListener(TakePicture);
        backBtn.onClick.AddListener(BackToCharacterCreateScene);
        CameraOn();

    }
    //AWS EC2에 닉네임 업로드를 위한 함수
    IEnumerator UploadToEC2(string URL, string nickname)
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Nickname Upload to EC2 Complete!");
        }
        www.Dispose();
    }
    IEnumerator GetFromEC2(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            int value;
            if (int.TryParse(www.downloadHandler.text, out value))
            {
                UnityEngine.Debug.Log("Received value from EC2: " + value);
            }
            else
            {
                UnityEngine.Debug.LogError("Failed to parse int value from response: " + www.downloadHandler.text);
            }
        }
        www.Dispose();
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
            UnityEngine.Debug.Log("No Camera!");
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
            // 선택된 카메라를 가져옴.
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);

            camTexture.requestedFPS = 30;

            cameraViewImage.texture = camTexture; // 영상을 raw Image에 할당.

            camTexture.Play(); // 카메라 시작하기
        }

    }

    public void CameraOff() // 카메라 끄기
    {
        if (camTexture != null)
        {
            camTexture.Stop(); // 카메라 정지
            Destroy(camTexture); // 카메라 객체반납
            camTexture = null; // 변수 초기화
        }
    }


    public void TakePicture() // 사진 촬영하기
    {
        Texture2D picture = new Texture2D(camTexture.width, camTexture.height);
        picture.SetPixels(camTexture.GetPixels());
        picture.Apply();

        // 동물상 학습에 적합하게 224x224 사이즈로 조정
        //Texture2D resizePicture = ScaleTexture(picture, 224, 224);

        // 이미지를 파일로 저장
        byte[] bytes = picture.EncodeToPNG();
        string fileName = "picture.png";
        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // 저장한 파일 경로 출력
        UnityEngine.Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);

        // S3 버킷에 촬영한 사진 업로드
        _ = S3Manage.s3Manage.UploadToS3(Application.persistentDataPath + "/" + fileName, PlayerInfo.playerInfo.nickname);

        // EC2 인스턴스에서 실행된 Flask 웹 서버에 닉네임 업로드
        StartCoroutine(UploadToEC2("http://13.124.0.232:5000/", "JJM"));

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