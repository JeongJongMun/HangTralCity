using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpManage : MonoBehaviour
{
    public void Click_SignUp_Btn() {
        SceneManager.LoadScene("SignInScene");
    }
    public void Click_Back_Btn() {
        SceneManager.LoadScene("SignInScene");
    }
}
