using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Amazon.S3;
using Amazon.Runtime;
using UnityEngine.SceneManagement;
using System.Net;

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

    public async Task PostToS3(object obj, string nickname)
    {
        PutObjectRequest request = null; // request ������ null�� �ʱ�ȭ

        // ���ε� �� obj�� ������ ���
        if (obj is string)
        {
            if (SceneManager.GetActiveScene().name == "GangScene")
            {
                request = new PutObjectRequest
                {
                    BucketName = "gang-whiteboard", // S3 ��Ŷ �̸�
                    Key = "picture", // S3�� ����� Key �̸� 
                    FilePath = obj.ToString(), // ���� ���
                    ContentType = "image/png" // ���� ����
                };
            }
            else if (SceneManager.GetActiveScene().name == "CameraScene")
            {
                request = new PutObjectRequest
                {
                    BucketName = "project-userpicture", // S3 ��Ŷ �̸�
                    Key = nickname, // S3�� ����� Key �̸� = �г��� 
                    FilePath = obj.ToString(), // ���� ���
                    ContentType = "image/png" // ���� ����
                };
            }
            else if (SceneManager.GetActiveScene().name == "ChatbotScene")
            {
                request = new PutObjectRequest
                {
                    BucketName = "question-database", // S3 ��Ŷ �̸�
                    Key = await GetUniqueKey("Question"), // �ߺ��� ���ϱ� ���� ����ũ�� Key ��������
                    ContentBody = obj.ToString(), // Json Value
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

    public async Task<string> GetUniqueKey(string baseKey)
    {
        string uniqueKey = baseKey;
        int count = 1;

        // Key�� �̹� �����ϴ��� Ȯ���ϰ�, �ߺ��� �߻��ϴ� ��쿡�� ���ڸ� �߰��Ͽ� ����ũ�� Key ����
        while (await CheckKeyExists(uniqueKey))
        {
            uniqueKey = $"{baseKey}{count}";
            count++;
        }

        return uniqueKey;
    }

    public async Task<bool> CheckKeyExists(string key)
    {
        var request = new GetObjectMetadataRequest
        {
            BucketName = "question-database", // S3 ��Ŷ �̸�
            Key = key // üũ�� Key
        };

        try
        {
            var response = await s3Client.GetObjectMetadataAsync(request);
            return true; // Key�� �����ϴ� ���
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
                return false; // Key�� �������� �ʴ� ���
            else
                throw; // ���ܸ� �ٽ� �����ϴ�.
        }
        catch (Exception e)
        {
            Debug.LogError($"Error checking if key exists: {e.Message}");
            throw; // ���ܸ� �ٽ� �����ϴ�.
        }
    }


    public string Finding()
    {
        string imgPath = "https://gang-whiteboard.s3.ap-northeast-2.amazonaws.com/picture";
        return imgPath;
    }
}
