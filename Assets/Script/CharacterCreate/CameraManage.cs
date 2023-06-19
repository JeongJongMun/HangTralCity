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
    public RawImage cameraViewImage; // 카메라가 보여질 화면

    [Header("Button")]
    public Button takePictureBtn; // 사진 촬영 버튼
    public Button backBtn; // 뒤로가기 버튼
    public Button galleryBtn; // 갤러리 버튼

    [Header("Panel")]
    public GameObject predictionPanel; // 동물상 예측 중 UI 패널

    // 카메라
    WebCamDevice[] devices;
    WebCamTexture frontCameraTexture = null;

    // 예측값
    PredictedAnimal predictedData;

    private void Start()
    {
        takePictureBtn.onClick.AddListener(TakePicture);
        backBtn.onClick.AddListener(BackToCharacterCreateScene);
        galleryBtn.onClick.AddListener(ClickGalleryBtn);
        CameraOn();

    }

    // AWS EC2 웹서버에 모델이 S3에서 가져올 사진 이름(닉네임) 업로드를 위한 함수
    IEnumerator PostNicknameToEC2(string URL, string nickname)
    {
        // S3에 사진이 업로드 될 시간 1초 정도 대기
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
    // 모델이 예측한 동물상 데이터를 S3에 저장 -> S3에서 예측값 가져오기
    IEnumerator GetPredictedCharacterFromS3(string URL)
    {
        // 모델이 돌아갈 시간 3초 정도 대기
        yield return new WaitForSeconds(3f);

        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
            // 이미지 로드 완료까지 대기
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;

                // JSON 데이터를 Dictionary 형태로 파싱
                predictedData = JsonUtility.FromJson<PredictedAnimal>(jsonResponse);

                Debug.LogFormat("Get Predicted Character Successful \nNickname:{0}, Label:{1}, Percentage:{2}", predictedData.nickname, predictedData.label, predictedData.percentage);

                // Label로 Character Type 적용
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


    public void CameraOn() // 카메라 켜기
    {
        // 카메라 권한 확인
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) Permission.RequestUserPermission(Permission.Camera);

        // 카메라가 없다면
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No Camera");
            return;
        }

        // 스마트폰의 카메라 정보를 모두 가져옴.
        devices = WebCamTexture.devices;

        // 전면 카메라 찾기
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                // 전면 카메라를 가져옴
                frontCameraTexture = new WebCamTexture(devices[i].name);
                frontCameraTexture.requestedFPS = 60;

                // 전면 카메라 영상을 raw Image에 할당.
                cameraViewImage.texture = frontCameraTexture;

                // 카메라 시작하기
                frontCameraTexture.Play();
            }
        }
    }

    public void CameraOff() // 카메라 끄기
    {
        if (frontCameraTexture != null)
        {
            frontCameraTexture.Stop(); // 카메라 정지
            Destroy(frontCameraTexture); // 카메라 객체 반납
            frontCameraTexture = null; // 변수 초기화
        }
    }

    public void TakePicture() // 사진 촬영하기
    {
        // 동물상 예측중 판넬 ON
        predictionPanel.SetActive(true);

        // 촬영한 사진을 Texture 2D로 생성
        Texture2D picture = new Texture2D(frontCameraTexture.width, frontCameraTexture.height);
        picture.SetPixels(frontCameraTexture.GetPixels());
        picture.Apply();


        // 이미지를 90도 반시계 방향으로 회전하여 파일로 저장
        Texture2D rotatedPicture = RotateTexture(picture, -90);
        byte[] bytes = rotatedPicture.EncodeToPNG();

        // 이미지를 파일로 저장
        string fileName = PlayerInfo.playerInfo.nickname;
        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);

        // 저장한 파일 경로 출력
        Debug.Log("Picture saved at " + Application.persistentDataPath + "/" + fileName);

        // S3 버킷에 촬영한 사진 업로드
        _ = S3Manage.s3Manage.PostToS3(Application.persistentDataPath + "/" + fileName, PlayerInfo.playerInfo.nickname);

        // EC2 인스턴스에서 실행된 Flask 웹 서버에 닉네임 업로드
        StartCoroutine(PostNicknameToEC2("43.202.19.142", PlayerInfo.playerInfo.nickname));

        // 모델에서 예측된 값과 닉네임이 S3에 저장 -> S3에서 닉네임, 레이블, 확률 가져오기
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

            // 용량 제한 (바이트 단위 -> 50메가)
            if (seleted.Length > 50000000) return;

            // 파일이 존재한다면 불러오기
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(UploadImageToS3(file));
            }
        });
    }

    IEnumerator UploadImageToS3(string path)
    {
        // 동물상 예측중 판넬 ON
        predictionPanel.SetActive(true);

        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0]; // 확장자를 자르기 위해 . 기준으로 나눔
        string savePath = Application.persistentDataPath + "/Image"; // 불러오고 내부에 저장하기

        // 아직 한번도 저장하지 않았다면
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + fileName + ".png", fileData); // png로 저장

        _ = S3Manage.s3Manage.PostToS3(savePath + fileName + ".png", PlayerInfo.playerInfo.nickname); // S3에 업로드

        // 0.5초 동안 대기 -> S3에 이미지가 올라갈 시간을 줌
        yield return new WaitForSeconds(0.5f);

        // EC2 인스턴스에서 실행된 Flask 웹 서버에 닉네임 업로드
        StartCoroutine(PostNicknameToEC2("43.202.19.142", PlayerInfo.playerInfo.nickname));

        // 모델에서 예측된 값과 닉네임이 S3에 저장 -> S3에서 닉네임, 레이블, 확률 가져오기
        StartCoroutine(GetPredictedCharacterFromS3("https://predicted-character.s3.ap-northeast-2.amazonaws.com/" + PlayerInfo.playerInfo.nickname));
    }

    public void BackToCharacterCreateScene()
    {
        CameraOff();
        SceneManager.LoadScene("CharacterCreateScene");
    }
}