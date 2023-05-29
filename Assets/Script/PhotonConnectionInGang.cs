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
    public GameObject img; // 오른쪽 위에 UI로 작게 뜨는 이미지
    public GameObject img2; // 강의동 칠판에 대문짝만하게 뜨는 이미지
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

        PhotonNetwork.LocalPlayer.NickName = PlayerInfo.playerInfo.nickname; // 닉네임 가져오기


        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3((float)-9.9, 6, -1), Quaternion.identity);
        Debug.LogFormat("{0}님이 방에 참가하였습니다.", PlayerInfo.playerInfo.nickname);

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

            // 용량 제한 (바이트 단위 -> 50메가)
            if (seleted.Length > 50000000) return;

            // 파일이 존재한다면 불러오기
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(UploadImage(file));
                img.SetActive(true);
                string url = S3Manage.s3Manage.Finding(); // url 가져오기
                PV.RPC("Upload", RpcTarget.AllBuffered, url);
            }
        });
    }

    IEnumerator UploadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0]; // 확장자를 자르기 위해 . 기준으로 나눔
        string savePath = UnityEngine.Application.persistentDataPath + "/Image"; // 불러오고 내부에 저장하기
        Debug.Log(UnityEngine.Application.persistentDataPath);

        // 아직 한번도 저장하지 않았다면
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + fileName + ".png", fileData); // png로 저장

        S3Manage.s3Manage.UploadToS3(savePath + fileName + ".png", PlayerInfo.playerInfo.nickname); // S3에 업로드
    }

    IEnumerator GetImage(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // 이미지 로드 완료까지 대기
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // 로드한 이미지를 RawImage에 설정
                if(img.GetComponent<RawImage>().texture != null) img.GetComponent<RawImage>().texture = null;

                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                img.GetComponent<RawImage>().texture = texture;
                ImageSizeSetting(img.GetComponent<RawImage>(), 500, 500); // 사이즈 조절

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



    void ImageSizeSetting(RawImage img, float x, float y) // 이미지, 최대 x, 최대 y
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if (x / y > imgX / imgY) // 이미지의 세로 길이가 더 길다
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (y / imgY));
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (y / imgY));
        }
        else // 이미지의 가로 길이가 더 길다
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (x / imgX));
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (x / imgX));
        }
    }

    void SpriteSizeSetting(SpriteRenderer spriteRenderer, float x, float y) // 스프라이트 렌더러, 최대 너비, 최대 높이
    {
        var sprite = spriteRenderer.sprite;
        var imgX = sprite.bounds.size.x * 100;
        var imgY = sprite.bounds.size.y * 100;
        Debug.LogFormat("{0},{1}", imgX, imgY);

        if (x / y > imgX / imgY) // 이미지의 세로 길이가 더 길다
        {
            spriteRenderer.transform.localScale = new Vector3(x / imgX, x / imgX, 1f);
        }
        else // 이미지의 가로 길이가 더 길다
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
