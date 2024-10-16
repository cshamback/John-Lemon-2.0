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

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        readableObject.SetActive(false);
        credits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && credits.activeInHierarchy)
        {
            CloseCredits();
        }

    }

    public void CloseCredits()
    {
        print("Hiding credits.");
        credits.SetActive(false);

        Time.timeScale = 1.0f;
        PauseMenu.isPaused = false;
    }
}
