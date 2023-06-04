using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManage : MonoBehaviour
{

    // ���� ȭ�� �ǹ��� �����ϱ� ��ư ������ �̺�Ʈ ����
    [Header("Buildings")]
    public Button buildingGang;
    public Button buildingDorm;
    public Button buildingHack;
    public Button buildingPlayground;

    [Header("Buttons")]
    public Button btnGang;
    public Button btnDorm;
    public Button btnHack;
    public Button btnPlayground;

    void Start()
    {
        buildingGang.GetComponent<Button>().onClick.AddListener(EnterGang);
        btnGang.GetComponent<Button>().onClick.AddListener(EnterGang);

        buildingDorm.GetComponent<Button>().onClick.AddListener(EnterDorm);
        btnDorm.GetComponent<Button>().onClick.AddListener(EnterDorm);

        buildingHack.GetComponent<Button>().onClick.AddListener(EnterHack);
        btnHack.GetComponent<Button>().onClick.AddListener(EnterHack);

        buildingPlayground.GetComponent<Button>().onClick.AddListener(EnterPlayground);
        btnPlayground.GetComponent<Button>().onClick.AddListener(EnterPlayground);
    }

    void EnterGang()
    {
        SceneManager.LoadScene("GangScene");
    }
    void EnterDorm()
    {
        SceneManager.LoadScene("DormScene");
    }
    void EnterHack()
    {
        SceneManager.LoadScene("HackScene");
    }
    void EnterPlayground()
    {
        SceneManager.LoadScene("MiniGameScene");
    }
}
