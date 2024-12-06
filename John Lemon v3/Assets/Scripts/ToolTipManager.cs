using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Data;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager S;
    [SerializeField] private GameObject tooltipDisplay;
    [SerializeField] private CanvasGroup tooltipGroup;
    [SerializeField] private TextMeshProUGUI tooltipText;

    public string[] toolTips = new string[6] {
        "Press W/S to move.",
        "Press A/D to rotate.",
        "Press R to load your weapon.",
        "Hold right click to aim. While aiming, you can rotate and use W/S to aim up and down.",
        "Left click or press space to shoot.",
        "When you've destroyed all the targets, you may return to reality."
    };
    public bool[] conditions = new bool[6] {
        true, false, false, false, false, false
    };

    public int currentTip = 0;

    // set up singleton pattern 
    void Awake()
    {
        if (S != null && S != this)
        {
            Destroy(gameObject);
        }
        S = this;
        DontDestroyOnLoad(this);

        FindToolTips();

        tooltipGroup.enabled = true;
        tooltipText.enabled = true;
        tooltipText.text = "";

        /*Debug.Log("Just set tooltip text active. Active: " + tooltipText.IsActive()
        + " Enabled: " + tooltipText.enabled
        + " Value: " + tooltipText.text);*/
    }

    // Update is called once per frame
    void Update()
    {
        // DISPLAY/CHANGE TOOLTIPS ----

        // check if the current condition's tooltip is true AND it's not currently active
        // if this is the case, we need to display the current tooltip 
        if (tooltipDisplay != null && tooltipGroup != null)
        {
            //Debug.Log("CURRENT TIP: " + currentTip);
            if (conditions[currentTip] == true && tooltipGroup.alpha == 0)
            {
                //Debug.Log("Displaying current tooltip: " + toolTips[currentTip]);
                tooltipGroup.alpha = 0.5f;
                tooltipText.text = toolTips[currentTip];
            }
            // if the current tooltip is no longer valid but it is displayed,
            // stop displaying it -> get ready for the next one
            else if (conditions[currentTip] == false && tooltipGroup.alpha != 0)
            {
                //Debug.Log("Hiding tooltip: " + conditions[currentTip]);
                tooltipGroup.alpha = 0;
                currentTip = currentTip++ % toolTips.Length; //Doing this so there isn't an error
                //When the level is restarted, the tooltip will start from the beginning.
            }
        }
        else
        {
            FindToolTips();
        }

        // SET TOOLTIP CONDITIONS ----
        if (conditions[0] && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)))
        {
            conditions[0] = false;
            conditions[1] = true;
        }
        if (conditions[1] && Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            conditions[1] = false;
        }
        if (SceneManager.GetActiveScene().name == "TargetPractice2")
        {
            conditions[2] = true;
            if (conditions[2] && Input.GetKeyDown(KeyCode.R))
            {
                conditions[2] = false;
                conditions[3] = true;
            }
            if (conditions[3] && Input.GetMouseButton(1))
            {
                conditions[3] = false;
                conditions[4] = true;
            }
            if (conditions[4] && Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space))
            {
                conditions[4] = false;
                conditions[5] = true;
            }
        }
        else
        {
            //print("Not in scene Target Practice.");
            conditions[5] = false;
        }
    }

    private void SetConditions(int index, bool condition)
    {
        if (conditions[index] && condition)
        {
            conditions[index] = false;
            if (index < (conditions.Length - 1))
            {
                conditions[index + 1] = true;
            }
        }
    }

    public void FindToolTips()
    {
        // find these programmatically at the start of each scene
        tooltipDisplay = GameObject.FindWithTag("TooltipUI");
        if (tooltipDisplay == null)
        {
            Debug.LogError("Could not find tooltip in scene.");
        }

        tooltipGroup = tooltipDisplay.GetComponent<CanvasGroup>();
        tooltipText = tooltipDisplay.GetComponentInChildren<TextMeshProUGUI>();
        tooltipGroup.alpha = 0;
    }
}
