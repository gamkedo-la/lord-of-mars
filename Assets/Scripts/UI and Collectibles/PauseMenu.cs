using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public static bool gamePaused = false;

    public GameObject playerCanvas;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public bool isPauseMuted = false;

    private void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }


    public void QuitGame()
    {
        if (gamePaused)
        {
            Resume();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void OptionsMenu()
    {
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void OptionsMenuBack()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void IsMuted()
    {
        isPauseMuted = !isPauseMuted;

        AudioListener.pause = isPauseMuted;
    }
}
