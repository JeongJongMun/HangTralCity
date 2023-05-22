using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Post : MonoBehaviour
{
    GameObject postPanel;
    TMP_Text postTitleTxt;
    TMP_Text detailTitleTxt;

    void Start()
    {
        postPanel = GameObject.Find("Canvas").transform.GetChild(7).gameObject;
        gameObject.GetComponent<Button>().onClick.AddListener(LoadPost);
    }

    void LoadPost()
    {
        postPanel.SetActive(true);
        postTitleTxt = GameObject.Find("PostTitleTxt").GetComponent<TMP_Text>();
        detailTitleTxt = GameObject.Find("PostDetailTxt").GetComponent<TMP_Text>();
        postTitleTxt.text = gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;
        detailTitleTxt.text = gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text;
    }
}
