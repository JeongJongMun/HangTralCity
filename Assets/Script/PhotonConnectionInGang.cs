using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.XR.ARSubsystems;
using static System.Net.Mime.MediaTypeNames;

public class PhotonConnectionInGang : MonoBehaviourPunCallbacks
{
    [Header("User In and Out")]
    public GameObject UserInPanel;
    public GameObject UserOutPanel;

    [Header("ChatPanel")]
    private UnityEngine.UI.Text ChatText;
    public InputField ChatInput;

    [Header("WhiteBoard")]
    public GameObject img; // ������ ���� UI�� �۰� �ߴ� �̹���
    public GameObject img2; // ���ǵ� ĥ�ǿ� �빮¦���ϰ� �ߴ� �̹���
    public Button galleryBtn;

    [Header("ETC")]
    public PhotonView PV;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Screen.SetResolution(1080, 2340, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;

            UserOutPanel.SetActive(false);
        }
        
    }
    void Start()
    {
        img.SetActive(false);
        galleryBtn.onClick.AddListener(ClickImageLoad);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster\n");

        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.playerInfo.nickname; // �г��� ��������


        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3((float)-9.9, 6, -1), Quaternion.identity);
        Debug.LogFormat("{0}���� �濡 �����Ͽ����ϴ�.", PlayerInfo.playerInfo.nickname);

        Invoke("Panel_", 1f);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainScene");
        Debug.Log("Move\n");
    }

    private void Panel_()
    {
        UserInPanel.SetActive(false);
    }

    public void ClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo seleted = new FileInfo(file);

            // �뷮 ���� (����Ʈ ���� -> 50�ް�)
            if (seleted.Length > 50000000) return;

            // ������ �����Ѵٸ� �ҷ�����
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(UploadImage(file));
                img.SetActive(true);
                string url = S3Manage.s3Manage.Finding(); // url ��������
                PV.RPC("Upload", RpcTarget.AllBuffered, url);
            }
        });
    }

    IEnumerator UploadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0]; // Ȯ���ڸ� �ڸ��� ���� . �������� ����
        string savePath = UnityEngine.Application.persistentDataPath + "/Image"; // �ҷ����� ���ο� �����ϱ�
        Debug.Log(UnityEngine.Application.persistentDataPath);

        // ���� �ѹ��� �������� �ʾҴٸ�
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + fileName + ".png", fileData); // png�� ����

        S3Manage.s3Manage.UploadToS3(savePath + fileName + ".png", PlayerInfo.playerInfo.nickname); // S3�� ���ε�
    }

    IEnumerator GetImage(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // �̹��� �ε� �Ϸ���� ���
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // �ε��� �̹����� RawImage�� ����
                if(img.GetComponent<RawImage>().texture != null) img.GetComponent<RawImage>().texture = null;

                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                img.GetComponent<RawImage>().texture = texture;
                ImageSizeSetting(img.GetComponent<RawImage>(), 500, 500); // ������ ����

                Sprite tempSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                img2.GetComponent<SpriteRenderer>().sprite = tempSprite;
                img2.transform.localScale = Vector3.one;
                SpriteSizeSetting(img2.GetComponent<SpriteRenderer>(), 1000, 1000);

            }
            else
            {
                Debug.Log("Image download failed. Error: " + www.error);
            }
        }
    }



    void ImageSizeSetting(RawImage img, float x, float y) // �̹���, �ִ� x, �ִ� y
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if (x / y > imgX / imgY) // �̹����� ���� ���̰� �� ���
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (y / imgY));
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (y / imgY));
        }
        else // �̹����� ���� ���̰� �� ���
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (x / imgX));
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (x / imgX));
        }
    }

    void SpriteSizeSetting(SpriteRenderer spriteRenderer, float x, float y) // ��������Ʈ ������, �ִ� �ʺ�, �ִ� ����
    {
        var sprite = spriteRenderer.sprite;
        var imgX = sprite.bounds.size.x * 100;
        var imgY = sprite.bounds.size.y * 100;
        Debug.LogFormat("{0},{1}", imgX, imgY);

        if (x / y > imgX / imgY) // �̹����� ���� ���̰� �� ���
        {
            spriteRenderer.transform.localScale = new Vector3(x / imgX, x / imgX, 1f);
        }
        else // �̹����� ���� ���̰� �� ���
        {
            spriteRenderer.transform.localScale = new Vector3(y / imgY, y / imgY, 1f);
        }
        Debug.Log(sprite.bounds.size.x * 100);
        Debug.Log(sprite.bounds.size.y * 100);
    }

    [PunRPC]
    private void Upload(string url)
    {
        StartCoroutine(GetImage(url));
    }
}
