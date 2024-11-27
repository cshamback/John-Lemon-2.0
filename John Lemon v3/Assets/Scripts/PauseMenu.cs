using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool allowedToPause = true; // make sure exiting another menu with esc doesnt also pause 

    public GameObject pauseMenu;
    public GameObject credits;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Escape key released.");
            if (isPaused)
            { // resume
                Debug.Log("Calling resume.");
                Resume();
            }
            else if (allowedToPause) // only pause if not closing something else
            { // pause
                UIManager.S.SetVisibility(pauseMenu, UIManager.S.pauseMenuOpen);
                Time.timeScale = 0.0f;

                isPaused = true;
            }
        }

        // flipping this back AFTER checking for pause makes sure it skips the pause signal only once
        if (!allowedToPause)
        {
            allowedToPause = true;
        }
    }

    // BUTTON METHODS ----

    public void RestartLevel()
    {
        Debug.Log("Restart level.");

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");

        isPaused = false;
    }

    // TODO: implement start menu <3
    public void QuitToStart()
    {
        Debug.Log("Quit to start.");

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");

        isPaused = false;
    }

    public void Resume()
    {
        Debug.Log("Resumed, motherfucker");
        UIManager.S.SetVisibility(pauseMenu, isPaused);
        Time.timeScale = 1.0f;

        isPaused = false;
        UIManager.S.pauseMenuOpen = false;
        Debug.Log("Changed pauseMenuOpen to " + UIManager.S.pauseMenuOpen + " and isPaused to: " + isPaused);
    }

    public void Credits()
    {
        Debug.Log("Show credits.");

        UIManager.S.SetVisibility(pauseMenu, isPaused);
        UIManager.S.SetVisibility(credits, UIManager.S.creditsOpen);

        Time.timeScale = 0.0f;
        isPaused = false;
        Debug.Log("Changed pauseMenuOpen to " + UIManager.S.pauseMenuOpen + " and isPaused to: " + isPaused);
    }
}
