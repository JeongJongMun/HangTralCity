using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManage : MonoBehaviour
{

    // ���� ȭ�� �ǹ��� �����ϱ� ��ư ������ �̺�Ʈ ����
    public Button buildingGang, buildingHack, buildingDorm, buildingPlayground;
    public Button gangBtn, hackBtn, dormBtn, playgroundBtn;

    void Start()
    {
        buildingGang.GetComponent<Button>().onClick.AddListener(EnterGang);
        gangBtn.GetComponent<Button>().onClick.AddListener(EnterGang);

        buildingDorm.GetComponent<Button>().onClick.AddListener(EnterDorm);
        dormBtn.GetComponent<Button>().onClick.AddListener(EnterDorm);

        buildingHack.GetComponent<Button>().onClick.AddListener(EnterHack);
        hackBtn.GetComponent<Button>().onClick.AddListener(EnterHack);

        buildingPlayground.GetComponent<Button>().onClick.AddListener(EnterPlayground);
        playgroundBtn.GetComponent<Button>().onClick.AddListener(EnterPlayground);
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
