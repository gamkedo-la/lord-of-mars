using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PromptTriggerScript : MonoBehaviour
{
    [SerializeField] GameObject tutorialPromptBox;

    public string myPrompt;
    private bool hasShown = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) {
            return; // prevent grenades etc from triggering the tip
        }
        if (hasShown)
        {
            return;
        }
        hasShown = true;
        tutorialPromptBox.gameObject.SetActive(true);
        tutorialPromptBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = myPrompt;
        Time.timeScale = 0;
        
        if (gameObject.name == "DeathPromptTriggerPrefab")
        {
            GameManagerScript.tutorialShouldReload = true;
        }
    }
}
