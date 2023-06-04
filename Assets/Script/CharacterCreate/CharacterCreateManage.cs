using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreateManage : MonoBehaviour
{
    public int type = 0;
    private Animator animator;

    [Header("Character Sprite")]
    public Sprite[] sprites;
    Dictionary<int, string> predictedAnimalName = new Dictionary<int, string>()
    {
        {0, "������"},
        {1, "�����"},
        {2, "��"},
        {3, "����"},
        {4, "�䳢"}
    };

    [Header("Button")]
    public Button cameraBtn;
    public Button createBtn;
    public Button leftBtn;
    public Button rightBtn;
    public Button checkBtn;

    [Header("Panel")]
    public GameObject prediction;
    public GameObject afterPrediction;
    public GameObject beforePrediction;

    [Header("Predict")]
    static public bool isPredicted;
    static public int predictedType = 0; // Default �� 0
    static public int percentage = 100; // Default �� 100%
    public GameObject predictedCharacter;
    public TMP_Text description;


    void Start()
    {
        createBtn.onClick.AddListener(ClickCreateBtn);
        cameraBtn.onClick.AddListener(ClickCameraBtn);
        leftBtn.onClick.AddListener(ClickLeftBtn);
        rightBtn.onClick.AddListener(ClickRightBtn);
        checkBtn.onClick.AddListener(ClickCheckBtn);

        animator = GetComponent<Animator>();

        if (isPredicted) Predicting();
    }
    // ���� �� ����
    void Predicting()
    {
        prediction.SetActive(true);
        afterPrediction.SetActive(false);
        // �� �ν� ����
        if (predictedType == -1)
        {
            predictedCharacter.GetComponent<Image>().sprite = sprites[0];
            description.text = "�� �νĿ� �����Ͽ����ϴ�! �ٽ� �Կ����ּ���.";
        }
        else
        {
            // �����ֱ� ĳ���Ϳ� ������ �� ����
            predictedCharacter.GetComponent<Image>().sprite = sprites[predictedType];
            // ���� � ĳ���Ͱ� ������� % ��Ȯ������ �˸�
            description.text = "����� " + percentage + "%�� ��Ȯ���� " + predictedAnimalName[predictedType] + " ���Դϴ�!";

            // �ִϸ��̼� ĳ���Ϳ� ������ �� ����
            type = predictedType;
            animator.SetInteger("type", type);
            PlayerInfo.playerInfo.characterType = type;
        }

        Invoke("Predicted", 3);
    }
    void Predicted()
    {
        beforePrediction.SetActive(false);
        afterPrediction.SetActive(true);
    }
    void ClickCheckBtn()
    {
        prediction.SetActive(false);
        isPredicted = false;
    }

    void ClickCreateBtn() {
        PlayerInfo.playerInfo.characterType = type;
        SceneManager.LoadScene("MainScene");
    }

    void ClickCameraBtn() {
        SceneManager.LoadScene("CameraScene");
    }

    void ClickLeftBtn() {
        type--;
        if (type == -1) type = 4;
        animator.SetInteger("type", type);
        PlayerInfo.playerInfo.characterType = type;
    }

    void ClickRightBtn() {
        type++;
        if (type == 5) type = 0;
        animator.SetInteger("type", type);
        PlayerInfo.playerInfo.characterType = type;
    }
}
