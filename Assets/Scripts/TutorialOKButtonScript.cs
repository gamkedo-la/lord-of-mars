using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialOKButtonScript : MonoBehaviour
{
    [SerializeField] GameObject tutorialDialogBox;

    private void Start()
    {
        GameManagerScript.aTutorialPromptIsOn = true;
        
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            tutorialDialogBox.gameObject.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            GameManagerScript.aTutorialPromptIsOn = false;
            if (GameManagerScript.tutorialShouldReload)
            {
                GameManagerScript.tutorialShouldReload = false;
                SceneManager.LoadScene("Tutorial");
            }
            
        }
    }
    public void HandleClick()
    {
        tutorialDialogBox.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
