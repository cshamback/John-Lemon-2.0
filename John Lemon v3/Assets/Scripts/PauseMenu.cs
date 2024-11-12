using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
            if (isPaused)
            { // resume
                Resume();
            }
            else if (allowedToPause) // only pause if not closing something else
            { // pause
                pauseMenu.SetActive(true);
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
        print("Restart level.");

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");

        isPaused = false;
    }

    // TODO: implement start menu <3
    public void QuitToStart()
    {
        print("Quit to start.");

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");

        isPaused = false;
    }

    public void Resume()
    {
        print("Resumed, motherfucker");
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;

        isPaused = false;
    }

    public void Credits()
    {
        print("Show credits.");

        pauseMenu.SetActive(false);
        credits.SetActive(true);

        Time.timeScale = 0.0f;
    }
}
