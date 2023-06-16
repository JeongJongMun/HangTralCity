using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using WebSocketSharp;

[System.Serializable]
public class PredictedChat
{
    public string question;
    public string response;
}

public class ChatbotManage : MonoBehaviour
{
    [Header("InputField")]
    public TMP_InputField requestInputField;

    [Header("Button")]
    public Button sendBtn;

    [Header("Prefab")]
    public GameObject requestGroup;
    public GameObject responseGroup;
    public GameObject responseGroupImage;

    [Header("Content - PrefabSpawnPos")]
    public GameObject content;

    [Header("ScrollBar")]
    public Scrollbar scrollbar;

    // ������
    PredictedChat predictedData;

    void Start()
    {
        sendBtn.onClick.AddListener(Question);
    }

    void Question()
    {
        if (!requestInputField.text.IsNullOrEmpty())
        {
            string question = requestInputField.text;
            // ���� ��ǳ�� ����
            GameObject _requestGroup = Instantiate(requestGroup, new Vector2(0, 0), Quaternion.identity, content.transform);
            // ���� ����
            _requestGroup.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = question;
            // �Է�â �ʱ�ȭ
            requestInputField.text = "";
            // ��ũ���� ���ϴ����� ����
            Invoke("ScrollDown", 0.1f);

            // ���� ������ ������ ���� S3�� ������ ����
            _ = S3Manage.s3Manage.PostToS3(question, PlayerInfo.playerInfo.nickname);
            // EC2 �ν��Ͻ����� ����� Flask �� ������ ���� ���ε�
            StartCoroutine(PostQuestionToEC2("15.164.130.22", question, PlayerInfo.playerInfo.nickname));
            // ���� ������ ���� �����͸� S3�� ���� -> S3���� ���� ������ ��������
            StartCoroutine(GetChatBotSentenceFromS3("https://chatting-serivce.s3.ap-northeast-2.amazonaws.com/" + PlayerInfo.playerInfo.nickname));

        }
    }
    void Response(string response)
    {
        // �亯 ��ǳ�� ����
        GameObject _responseGroup = Instantiate(responseGroup, new Vector2(0, 0), Quaternion.identity, content.transform);
        // �亯 ����
        _responseGroup.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = response;
        // ��ũ���� ���ϴ����� ����
        Invoke("ScrollDown", 0.1f);
    }
    void ResponseImage(string imageName)
    {
        // �亯 ��ǳ�� ����
        GameObject _responseGroup = Instantiate(responseGroupImage, new Vector2(0, 0), Quaternion.identity, content.transform);
        // �亯 ����
        _responseGroup.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(imageName);
        // ��ũ���� ���ϴ����� ����
        Invoke("ScrollDown", 0.1f);
        Response("������ �ش� ��ġ�� �ֽ��ϴ�.");
    }
    void ScrollDown()
    {
        scrollbar.value = 0;

    }
    // AWS EC2 �������� ���� ���ε带 ���� �Լ�
    IEnumerator PostQuestionToEC2(string URL, string question, string nickname)
    {
        // S3�� ������ ���ε� �� �ð� 1�� ���� ���
        yield return new WaitForSeconds(1f);

        WWWForm form = new WWWForm();
        form.AddField("question", question);
        form.AddField("nickname", nickname);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Question Upload to EC2 Complete!");
        }
        www.Dispose();
    }
    // ê�� ������ �亯�� S3�� ���� -> S3���� �亯 ��������
    IEnumerator GetChatBotSentenceFromS3(string URL)
    {
        // ���� ���ư� �ð� 2.5�� ���� ���
        yield return new WaitForSeconds(2.5f);

        using UnityWebRequest www = UnityWebRequest.Get(URL);
        // �亯 �ε� �Ϸ���� ���
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;

            // JSON �����͸� Class ���·� �Ľ�
            predictedData = JsonUtility.FromJson<PredictedChat>(jsonResponse);

            // �亯 ����
            Debug.LogFormat("Response\n{0}", predictedData.response);
            
            if (predictedData.response.Contains("�̹���"))
            {
                ResponseImage(predictedData.response);
            }
            else
            {
                Response(predictedData.response);
            }

        }
        else
        {
            Debug.Log("GET ChatBot Response Value failed. Error: " + www.error);
        }
    }
}
