using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Amazon.S3;
using Amazon.Runtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class S3Manage : MonoBehaviour
{
    public static S3Manage s3Manage;

    string accessKey = "AKIA4SLVEG5W3BBNELIJ";
    string secretKey = "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";

    public AmazonS3Client s3Client;



    // S3Manage Ŭ������ �̱� ������ ����
    // �� �ϳ��� �ν��Ͻ����� ������ ��𼭵� ���� ����
    // ���ٽ� S3Manage.s3Manage.������ ����
    void Awake()
    {
        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.APNortheast2);

        DontDestroyOnLoad(gameObject);

        // s3manage�� ������ �ν��Ͻ�
        if (s3Manage == null) s3Manage = this;
        // s3manage �ν��Ͻ��� �̰� �ƴ϶��, �ٸ� �ν��Ͻ� ����
        else if (s3Manage != this) Destroy(gameObject);
    }

    public async Task UploadToS3(object obj, string nickname)
    {
        PutObjectRequest request = null; // request ������ null�� �ʱ�ȭ
        
        // ���ε� �� obj�� ������ ���
        if (obj is string)
        {
            if(SceneManager.GetActiveScene().name == "GangScene")
            {
                request = new PutObjectRequest
                {
                    BucketName = "gang-whiteboard", // S3 ��Ŷ �̸�
                    Key = "picture", // S3�� ����� Key �̸� 
                    FilePath = obj.ToString(), // ���� ���
                    ContentType = "image/png" // ���� ����
                };
            }
            else
            {
                request = new PutObjectRequest
                {
                    BucketName = "project-userpicture", // S3 ��Ŷ �̸�
                    Key = nickname, // S3�� ����� Key �̸� = �г��� 
                    FilePath = obj.ToString(), // ���� ���
                    ContentType = "image/png" // ���� ����
                };
            }
        }
        // ���ε� �� obj�� Dictionary (�� Ŀ���͸���¡ ����)�� ���
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

    public string Finding()
    {
        string imgPath = "https://gang-whiteboard.s3.ap-northeast-2.amazonaws.com/picture";
        return imgPath;
    }
}
