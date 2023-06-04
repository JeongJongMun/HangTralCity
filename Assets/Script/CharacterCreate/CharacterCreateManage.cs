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
        {0, "강아지"},
        {1, "고양이"},
        {2, "곰"},
        {3, "공룡"},
        {4, "토끼"}
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
    static public int predictedType = 0; // Default 값 0
    static public int percentage = 100; // Default 값 100%
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
    // 예측 값 적용
    void Predicting()
    {
        prediction.SetActive(true);
        afterPrediction.SetActive(false);
        // 얼굴 인식 실패
        if (predictedType == -1)
        {
            predictedCharacter.GetComponent<Image>().sprite = sprites[0];
            description.text = "얼굴 인식에 실패하였습니다! 다시 촬영해주세요.";
        }
        else
        {
            // 보여주기 캐릭터에 예측된 값 적용
            predictedCharacter.GetComponent<Image>().sprite = sprites[predictedType];
            // 설명에 어떤 캐릭터가 어느정도 % 정확도인지 알림
            description.text = "당신은 " + percentage + "%의 정확도로 " + predictedAnimalName[predictedType] + " 상입니다!";

            // 애니메이션 캐릭터에 예측된 값 적용
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
