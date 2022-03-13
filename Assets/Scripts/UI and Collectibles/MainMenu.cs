using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public float fadeSpeed;
    public float growSpeed;
    public int targetTextSize;

    public TMP_Text titleText;
    public GameObject mainMenuPanel;
    public GameObject levelPanel;

    private void Start()
    {
        StartFade();
    }

    public void StartFade()
    {
        StartCoroutine(FadeInText(titleText.transform));
        StartCoroutine(GrowText(titleText.transform));

        foreach (Transform child in mainMenuPanel.transform)
        {
            StartCoroutine(FadeInButton(child));
        }
        foreach (Transform child in levelPanel.transform)
        {
            StartCoroutine(FadeInButton(child));
        }

    }

    private IEnumerator FadeInText(Transform textToFade)
    {
        Color textColor = textToFade.gameObject.GetComponent<TMP_Text>().color;

        while (textColor.a < 1)
        {
            float fadeAmount = textColor.a + (fadeSpeed * Time.fixedDeltaTime);

            textColor = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
            textToFade.gameObject.GetComponent<TMP_Text>().color = textColor;

            yield return null;
        }
        textColor = new Color(textColor.r, textColor.g, textColor.b, 1.0f);
        textToFade.gameObject.GetComponent<TMP_Text>().color = textColor;
    }

    private IEnumerator GrowText(Transform textToGrow)
    {
        float textFontSize = textToGrow.gameObject.GetComponent<TMP_Text>().fontSize;

        while (textFontSize < targetTextSize)
        {
            float newSize = textFontSize + (growSpeed * Time.fixedDeltaTime);
            textFontSize = newSize;

            textToGrow.GetComponent<TMP_Text>().fontSize = textFontSize;

            yield return null;
        }
    }

    private IEnumerator FadeInButton(Transform imageToFade)
    {
        //yield return new WaitWhile(() => titleText.color.a < 1);

        Color objectColor = imageToFade.gameObject.GetComponent<Image>().color;

        while (objectColor.a < 1)
        {
            float fadeAmount = objectColor.a + (fadeSpeed * Time.fixedDeltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            imageToFade.gameObject.GetComponent<Image>().color = objectColor;

            yield return null;
        }
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 1.0f);
        imageToFade.gameObject.GetComponent<Image>().color = objectColor;
    }
}