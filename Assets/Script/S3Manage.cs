using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Amazon.S3;
using Amazon.Runtime;
using UnityEngine.SceneManagement;

public class S3Manage : MonoBehaviour
{
    public static S3Manage s3Manage;

    string accessKey = "AKIA4SLVEG5W3BBNELIJ";
    string secretKey = "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";

    public AmazonS3Client s3Client;

    // S3Manage 클래스를 싱글 톤으로 생성
    // 단 하나의 인스턴스만을 가지며 어디서든 접근 가능
    // 접근시 S3Manage.s3Manage.변수로 접근
    void Awake()
    {
        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.APNortheast2);

        DontDestroyOnLoad(gameObject);

        // s3manage가 유일한 인스턴스
        if (s3Manage == null) s3Manage = this;
        // s3manage 인스턴스가 이게 아니라면, 다른 인스턴스 삭제
        else if (s3Manage != this) Destroy(gameObject);
    }

    public async Task PostPictureToS3(object obj, string nickname)
    {
        PutObjectRequest request = null; // request 변수를 null로 초기화
        
        // 업로드 할 obj가 사진일 경우
        if (obj is string)
        {
            if (SceneManager.GetActiveScene().name == "GangScene")
            {
                request = new PutObjectRequest
                {
                    BucketName = "gang-whiteboard", // S3 버킷 이름
                    Key = "picture", // S3에 저장될 Key 이름 
                    FilePath = obj.ToString(), // 파일 경로
                    ContentType = "image/png" // 파일 형식
                };
            }
            else if (SceneManager.GetActiveScene().name == "CameraScene")
            {
                request = new PutObjectRequest
                {
                    BucketName = "project-userpicture", // S3 버킷 이름
                    Key = nickname, // S3에 저장될 Key 이름 = 닉네임 
                    FilePath = obj.ToString(), // 파일 경로
                    ContentType = "image/png" // 파일 형식
                };
            }
        }
        try
        {
            var response = await s3Client.PutObjectAsync(request);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error uploading file to S3: {e.Message}");
        }
    }

    public string Finding()
    {
        string imgPath = "https://gang-whiteboard.s3.ap-northeast-2.amazonaws.com/picture";
        return imgPath;
    }
}
