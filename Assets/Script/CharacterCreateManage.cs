using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreateManage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click_CreateBtn() {
        SceneManager.LoadScene("MainScene");
    }
    public void Click_ReCreateBtn() {
        Debug.Log("Clicked RecreateBtn");
    }
    public void Click_LeftBtn() {
        Debug.Log("Clicked LeftBtn");
    }
    public void Click_RightBtn() {
        Debug.Log("Clicked RightBtn");
    }
}
