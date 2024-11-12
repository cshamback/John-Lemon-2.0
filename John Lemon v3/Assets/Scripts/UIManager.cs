using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls show/hide of all UI objects. 

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject readableObject;
    public GameObject credits;
    public GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        readableObject.SetActive(false);
        credits.SetActive(false);

        hud.SetActive(true); // HUD is active while playing
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (credits.activeInHierarchy)
            {
                CloseOpenUI(credits);
            }
            if (readableObject.activeInHierarchy)
            {
                CloseOpenUI(readableObject);
            }
        }

        // readableObject UI is opened by another class (Interactible)
        // as such, UIManager must check if it is open and if so close HUD
        if (readableObject.activeInHierarchy)
        {
            hud.SetActive(false);
            Time.timeScale = 0f;
        }

    }

    public void CloseOpenUI(GameObject ui)
    {
        Time.timeScale = 1.0f;

        PauseMenu.isPaused = false;
        PauseMenu.allowedToPause = false;

        ui.SetActive(false);
        hud.SetActive(true);
    }
}
