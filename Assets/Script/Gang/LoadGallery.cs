using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ExitGames.Client.Photon;

public class LoadGallery : MonoBehaviour
{
    public GameObject img; // ������ ���� UI�� �۰� �ߴ� �̹���
    public GameObject img2; // ���ǵ� ĥ�ǿ� �빮¦���ϰ� �ߴ� �̹���
    public Button galleryBtn;
    void Start()
    {
        img.SetActive(false);
        galleryBtn.onClick.AddListener(ClickImageLoad);
        //if (File.Exists(Application.persistentDataPath + "/Image")) File.ReadAllBytes(Application.persistentDataPath + "/Image"); // ����� ������ �ִٸ� �о����
    }

    public void ClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo seleted = new FileInfo(file);

            // �뷮 ���� (����Ʈ ���� -> 50�ް�)
            if (seleted.Length > 50000000) return;

            // ������ �����Ѵٸ� �ҷ�����
            if (!string.IsNullOrEmpty(file)) StartCoroutine(LoadImage(file));

        });
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0]; // Ȯ���ڸ� �ڸ��� ���� . �������� ����
        string savePath = Application.persistentDataPath + "/Image"; // �ҷ����� ���ο� �����ϱ�
        Debug.Log(Application.persistentDataPath);

        // ���� �ѹ��� �������� �ʾҴٸ�
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + fileName + ".png", fileData); // png�� ����

        var tempImage = File.ReadAllBytes(savePath + fileName + ".png"); // ������ png �ҷ�����

        // Byte�� Image�� ��ȯ
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(tempImage);

        // Byte�� Sprite�� ��ȯ
        Sprite tempSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);

        // ������ �� UI�� �۰� �ߴ� �̹���
        img.SetActive(true); // Ȱ��ȭ
        img.GetComponent<RawImage>().texture = tex; // �̹��� ����
        ImageSizeSetting(img.GetComponent<RawImage>(), 500, 500); // ������ ����

        // ���ǵ� ĥ�ǿ� �빮¦���ϰ� �ߴ� �̹���
        img2.GetComponent<SpriteRenderer>().sprite = tempSprite;
        img2.transform.localScale = Vector3.one;
        SpriteSizeSetting(img2.GetComponent<SpriteRenderer>(), 1000, 1000);
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
}
