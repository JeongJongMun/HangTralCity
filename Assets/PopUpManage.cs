using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpManage : MonoBehaviour
{
    public GameObject popUpObject;
    public TMP_Text popUpText;
    public float fadeInDuration = 0.3f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 0.3f;

    private bool isTransitioning = false;

    private void Start()
    {
        SetPopUpAlpha(0f);
        popUpObject.SetActive(false);
    }

    public void ShowPopup(string error)
    {
        if (isTransitioning) return;

        popUpText.text = error;

        StartCoroutine(ShowAndHidePopup());
    }

    private IEnumerator ShowAndHidePopup()
    {
        isTransitioning = true;
        SetPopUpAlpha(0f);
        popUpObject.SetActive(true);

        // Fade in
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / fadeInDuration;
            float alpha = Mathf.Lerp(0f, 1f, normalizedTime);
            SetPopUpAlpha(alpha);
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / fadeOutDuration;
            float alpha = Mathf.Lerp(1f, 0f, normalizedTime);
            SetPopUpAlpha(alpha);
            yield return null;
        }

        popUpObject.SetActive(false);
        isTransitioning = false;
    }

    private void SetPopUpAlpha(float alpha)
    {
        Color textColor = popUpText.color;
        textColor.a = alpha;
        popUpText.color = textColor;
    }
}
