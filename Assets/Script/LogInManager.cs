using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click_LogIn_Btn() {
        SceneManager.LoadScene("MainScene");
    }

    public void Click_SignIn_Btn() {
        SceneManager.LoadScene("SignInScene");
    }
}
