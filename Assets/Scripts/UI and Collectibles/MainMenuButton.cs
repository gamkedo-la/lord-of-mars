using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color32 startingColor;
    public Color32 highlightColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("MainMenuButton(): OnPointerEnter() was called for object " + name);
        gameObject.GetComponent<Image>().color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("MainMenuButton(): OnPointerExit() was called for object " + name);
        gameObject.GetComponent<Image>().color = startingColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (name == "StartButton")
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (name == "Credits") {
            SceneManager.LoadScene("Credits Menu");
        } else if (name == "QuitButton")
        {
            Application.Quit();
        }
    }
}