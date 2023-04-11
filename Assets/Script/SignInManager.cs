using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{
    public void Click_SignIn_Btn() {
        SceneManager.LoadScene("MainScene");
    }

    public void Click_SignUp_Btn() {
        SceneManager.LoadScene("SignUpScene");
    }
}
