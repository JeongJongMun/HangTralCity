using Amazon.EC2.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreateManage : MonoBehaviour
{
    // 동물상 예측을 통한 결과 값
    // 0 = puppy
    // 1 = cat
    // 2 = bear
    // 3 = dino
    // 4 = rabbit
    private int type = 0;

    public Button camera_btn, create_btn, left_btn, right_btn, checkBtn;
    private Animator animator;
    static public bool isPredicted;
    public GameObject prediction, afterPrediction, beforePrediction;
    

    private void Start()
    {
        create_btn.onClick.AddListener(ClickCreateBtn);
        camera_btn.onClick.AddListener(ClickCameraBtn);
        left_btn.onClick.AddListener(ClickLeftBtn);
        right_btn.onClick.AddListener(ClickRightBtn);
        checkBtn.onClick.AddListener(ClickCheckBtn);

        animator = GetComponent<Animator>();

        if (isPredicted) Predicting();
    }
    private void Predicting()
    {
        prediction.SetActive(true);
        afterPrediction.SetActive(false);

        Invoke("Predicted", 3);
    }
    private void Predicted()
    {
        beforePrediction.SetActive(false);
        afterPrediction.SetActive(true);
    }
    private void ClickCheckBtn()
    {
        prediction.SetActive(false);
        isPredicted = false;
    }

    private void ClickCreateBtn() {
        PlayerInfo.playerInfo.characterType = type;
        SceneManager.LoadScene("MainScene");
    }

    private void ClickCameraBtn() {
        SceneManager.LoadScene("CameraScene");
    }

    private void ClickLeftBtn() {
        type--;
        if (type == -1) type = 4;
        animator.SetInteger("type", type);
        PlayerInfo.playerInfo.characterType = type;
    }

    private void ClickRightBtn() {
        type++;
        if (type == 5) type = 0;
        animator.SetInteger("type", type);
        PlayerInfo.playerInfo.characterType = type;
    }
}
