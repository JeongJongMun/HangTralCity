using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreateManage : MonoBehaviour
{
    // ������ ������ ���� ��� ��
    // 0 = puppy
    // 1 = cat
    // 2 = bear
    // 3 = dino
    // 4 = rabbit
    private int type = 0;

    public Button camera_btn, create_btn, left_btn, right_btn;
    private Animator animator;
    

    private void Start()
    {
        create_btn.onClick.AddListener(ClickCreateBtn);
        camera_btn.onClick.AddListener(ClickCameraBtn);
        left_btn.onClick.AddListener(ClickLeftBtn);
        right_btn.onClick.AddListener(ClickRightBtn);

        animator = GetComponent<Animator>();
    }

    private void ClickCreateBtn() {
        PlayerInfo.player_info.character_type = type;
        SceneManager.LoadScene("MainScene");
    }

    private void ClickCameraBtn() {
        SceneManager.LoadScene("CameraScene");
    }

    private void ClickLeftBtn() {
        type--;
        if (type == -1) type = 4;
        animator.SetInteger("type", type);
        PlayerInfo.player_info.character_type = type;
    }

    private void ClickRightBtn() {
        type++;
        if (type == 5) type = 0;
        animator.SetInteger("type", type);
        PlayerInfo.player_info.character_type = type;
    }
}
