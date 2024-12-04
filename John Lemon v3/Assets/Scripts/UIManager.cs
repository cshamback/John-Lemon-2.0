using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private TextMeshProUGUI currentAmmo;
    private TextMeshProUGUI loadedAmmo;

    private Gun gun;

    public bool readableObjectOpen = false;
    public bool pauseMenuOpen = false;
    public bool creditsOpen = false;
    private bool hudOpen = true;

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

        SetVisibility(readableObject, true);
        SetVisibility(pauseMenu, true);
        SetVisibility(credits, true);

        readableObjectOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed. CreditsOpen: " + creditsOpen + " readableObjOpen: " + readableObjectOpen);
            if (creditsOpen)
            {
                CloseOpenUI(credits, creditsOpen);
            }
            if (readableObjectOpen)
            {
                Debug.Log("Esc. pressed with readable object open. Closing readable object.");
                CloseOpenUI(readableObject, readableObjectOpen);
                readableObjectOpen = false;
            }
            else
            {
                Debug.Log("ReadableObject not open.");
            }
        }

        // readableObject UI is opened by another class (Interactible)
        // as such, UIManager must check if it is open and if so close HUD
        if (readableObjectOpen)
        {
            SetVisibility(hud, true);
            Time.timeScale = 0f;
        }

    }

    public void CloseOpenUI(GameObject ui, bool isOpen)
    {
        //print("Closing open ui: " + ui.name);

        PauseMenu.isPaused = false;
        PauseMenu.allowedToPause = false;

        SetVisibility(ui, isOpen);
        SetVisibility(hud, false);
        //Debug.Log("PauseMenuOpen: " + pauseMenuOpen);
        SetVisibility(pauseMenu, true);

        Time.timeScale = 1.0f;
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
            if (ui.name == "ReadableObject")
            {
                ui.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else
            {
                print("IsOpen for " + ui.name + ": " + isOpen + " setting alpha to 0.5.");
                ui.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
            //Debug.Log("Ui name: " + ui.name + " Current alpha: " + ui.GetComponent<CanvasGroup>().alpha);
        }

        isOpen = !isOpen;
        print("isOpen is now: " + isOpen + " for " + ui.name);

        if (ui.name == "ReadableObject")
        {
            Debug.Log("Opened readable object.");
            readableObjectOpen = true;
        }
    }
}
