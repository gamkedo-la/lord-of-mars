using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int sceneToLoad;
    public bool releaseMouse = false;

    private void Awake()
    {
        PauseMenu.gamePaused = false;
        if(releaseMouse)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = releaseMouse;
    }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void DelayThenLoadScene(bool nextStage)
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        if (nextStage)
        {
            sceneToLoad++;
        }
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void EndGame()
    {
        Application.Quit();
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits Menu");
    }

    public void LoadEndMenu()
    {
        SceneManager.LoadScene("End Menu");
    }
}
