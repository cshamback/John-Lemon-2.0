using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor.Animations;
using UnityEngine;

// controls show/hide of all UI objects. 

public class UIManager : MonoBehaviour
{
    public static UIManager S;
    public GameObject pauseMenu;
    public GameObject readableObject;
    public GameObject credits;
    public GameObject hud;
    public GameObject gameOver;
    private GameObject deathCover;
    private GameObject deathContent;
    private TextMeshProUGUI currentAmmo;
    private TextMeshProUGUI loadedAmmo;

    private Gun gun;

    private bool readableObjectOpen = false;
    public bool pauseMenuOpen = false;
    public bool creditsOpen = false;
    private bool hudOpen = true;
    private float m_Timer = 0f; //Timer for the fade in and out of the game over screen.
    public float transitionDuration = 3.0f; //The duration of fade ins and outs. Used for Game Over.

    void Awake()
    {
        if (S != null && S != this)
        {
            Destroy(gameObject);
        }
        S = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        hud.SetActive(true); // HUD is active while playing

        // find these programmatically to cut down on number of annoying ass drag and drops
        gun = GameObject.FindGameObjectWithTag("EquippedGun").GetComponent<Gun>();
        currentAmmo = hud.GetComponentsInChildren<TextMeshProUGUI>()[0];
        loadedAmmo = hud.GetComponentsInChildren<TextMeshProUGUI>()[1];

        currentAmmo.text = gun.currentLoaded.ToString();
        loadedAmmo.text = gun.totalAmmo.ToString();

        //The content of the game over screen is it's first child, and the cover is the second.
        deathContent = gameOver.gameObject.transform.GetChild(0).gameObject;
        deathCover = gameOver.gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (creditsOpen)
            {
                CloseOpenUI(credits, creditsOpen);
            }
            if (readableObjectOpen)
            {
                CloseOpenUI(readableObject, readableObjectOpen);
            }
        }

        // readableObject UI is opened by another class (Interactible)
        // as such, UIManager must check if it is open and if so close HUD
        if (readableObjectOpen)
        {
            hud.SetActive(false);
            Time.timeScale = 0f;
        }

    }

    public void CloseOpenUI(GameObject ui, bool isOpen)
    {
        print("Closing open ui: " + ui.name);
        Time.timeScale = 1.0f;

        PauseMenu.isPaused = false;
        PauseMenu.allowedToPause = false;

        SetVisibility(ui, isOpen);
        SetVisibility(hud, hudOpen);
    }

    public void SetVisibility(GameObject ui, bool isOpen)
    {

        // if currently "open", close it by setting alpha to 0 
        if (isOpen)
        {
            print("IsOpen for " + ui.name + ": " + isOpen + " setting alpha to 0.");
            ui.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        { // if currently closed, "open" it by setting alpha to 0.5
            print("IsOpen for " + ui.name + ": " + isOpen + " setting alpha to 0.5.");
            ui.GetComponent<CanvasGroup>().alpha = 0.5f;
        }

        isOpen = !isOpen;
        print("isOpen is now: " + isOpen);
    }

    public void KillJohn()
    {
        deathContent.SetActive(false); //make sure the other stuff is hidden.
        gameOver.SetActive(true); //turns on the game object menu.
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        m_Timer = 0f; //Reset the timer.
        Color coverColor = deathCover.GetComponent<Image>().color; //Get the color of the cover.

        // Fade in
        while (m_Timer < transitionDuration) //While the timer is less than the duration of the fade in and out.
        {
            m_Timer += Time.deltaTime; //update the timer.
            coverColor.a = m_Timer / transitionDuration; //Update the alpha with the percent full the timer is.
            deathCover.GetComponent<Image>().color = coverColor; //Give the cover the new alpha.
            yield return null; //Wait for the next frame.
        }

        //Now that the cover is covering everything, we can sneakily turn the contents on behind it.
        deathContent.SetActive(true);

        // Fade out
        while (m_Timer > 0) //While the timer is greater than 0.
        {
            m_Timer -= Time.deltaTime; //start reversing timer.
            coverColor.a = m_Timer / transitionDuration; //Update the alpha with the percent full the timer is.
            deathCover.GetComponent<Image>().color = coverColor; //Give the cover the new alpha.
            yield return null; //Wait for the next frame.
        }
        deathCover.SetActive(false); //Turn off the cover so it can be interacted with.
    }
}
