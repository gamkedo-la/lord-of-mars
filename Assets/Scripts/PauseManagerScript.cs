using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManagerScript : MonoBehaviour
{
    private GameObject pauseImage;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        pauseImage = GameObject.FindGameObjectWithTag("PauseImage");
        pauseImage.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (!GameManagerScript.gameIsPaused)
            {
                Debug.Log("pauseImage: " + pauseImage.name);
                pauseImage.SetActive(true);
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = true;
                Time.timeScale = 0;
                GameManagerScript.gameIsPaused = true;
            }
            else
            {
                pauseImage.SetActive(false);
                Time.timeScale = 1;
                GameManagerScript.gameIsPaused = false;
            }
        }
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        GameManagerScript.gameIsPaused = false;
    }
}
