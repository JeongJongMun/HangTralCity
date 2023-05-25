using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Amazon.S3;
using Amazon.Runtime;
using System.Collections.Generic;

public class s3uploading : MonoBehaviour
{
    public static s3uploading S3Uploading;

    public string accessKey = "AKIA4SLVEG5W3BBNELIJ";
    public string secretKey = "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";

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
        if (S3Uploading == null) S3Uploading = this;
        // s3manage 인스턴스가 이게 아니라면, 다른 인스턴스 삭제
        else if (S3Uploading != this) Destroy(gameObject);
    }

    public async Task UploadToS3(object obj, string nickname)
    {
        PutObjectRequest request = null; // request 변수를 null로 초기화

        // 업로드 할 obj가 사진일 경우
        if (obj is string)
        {
            request = new PutObjectRequest
            {
                BucketName = "project-userpicture", // S3 버킷 이름
                Key = nickname, // S3에 저장될 Key 이름 = 닉네임 
                FilePath = obj.ToString(), // 파일 경로
                ContentType = "image/png" // 파일 형식
            };
        }
        // 업로드 할 obj가 Dictionary (방 커스터마이징 정보)일 경우
        else if (obj is Dictionary<string, List<Vector3>>)
        {
            string json = JsonUtility.ToJson(obj);

            request = new PutObjectRequest
            {
                BucketName = "room-customize-info",
                Key = nickname,
                ContentBody = json
            };
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
}
