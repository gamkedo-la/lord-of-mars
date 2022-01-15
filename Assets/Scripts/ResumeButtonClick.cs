using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButtonClick : ButtonScript
{
    private GameObject pauseManager;
    [SerializeField] GameObject pauseMenu;
    private void Start()
    {
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager");
    }
    public override void HandleButtonClick()
    {
        Debug.Log("resume button click recognized");
        pauseManager.GetComponent<PauseManagerScript>().UnPauseGame();
        pauseMenu.SetActive(false);
    }
}
