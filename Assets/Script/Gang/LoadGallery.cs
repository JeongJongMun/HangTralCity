using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ExitGames.Client.Photon;

public class LoadGallery : MonoBehaviour
{
    public GameObject img; // 오른쪽 위에 UI로 작게 뜨는 이미지
    public GameObject img2; // 강의동 칠판에 대문짝만하게 뜨는 이미지
    public Button galleryBtn;
    void Start()
    {
        img.SetActive(false);
        galleryBtn.onClick.AddListener(ClickImageLoad);
        //if (File.Exists(Application.persistentDataPath + "/Image")) File.ReadAllBytes(Application.persistentDataPath + "/Image"); // 저장된 파일이 있다면 읽어오기
    }

    public void ClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo seleted = new FileInfo(file);

            // 용량 제한 (바이트 단위 -> 50메가)
            if (seleted.Length > 50000000) return;

            // 파일이 존재한다면 불러오기
            if (!string.IsNullOrEmpty(file)) StartCoroutine(LoadImage(file));

        });
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0]; // 확장자를 자르기 위해 . 기준으로 나눔
        string savePath = Application.persistentDataPath + "/Image"; // 불러오고 내부에 저장하기
        Debug.Log(Application.persistentDataPath);

        // 아직 한번도 저장하지 않았다면
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + fileName + ".png", fileData); // png로 저장

        var tempImage = File.ReadAllBytes(savePath + fileName + ".png"); // 저장한 png 불러오기

        // Byte를 Image로 전환
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(tempImage);

        // Byte를 Sprite로 전환
        Sprite tempSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);

        // 오른쪽 위 UI로 작게 뜨는 이미지
        img.SetActive(true); // 활성화
        img.GetComponent<RawImage>().texture = tex; // 이미지 적용
        ImageSizeSetting(img.GetComponent<RawImage>(), 500, 500); // 사이즈 조절

        // 강의동 칠판에 대문짝만하게 뜨는 이미지
        img2.GetComponent<SpriteRenderer>().sprite = tempSprite;
        img2.transform.localScale = Vector3.one;
        SpriteSizeSetting(img2.GetComponent<SpriteRenderer>(), 1000, 1000);
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
}
