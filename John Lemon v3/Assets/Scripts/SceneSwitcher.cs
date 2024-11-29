using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/* PURPOSE ----
    For any given scene, keep track of when to switch scenes. 
    Also, switch scenes when needed. 
*/

[RequireComponent(typeof(BoxCollider))]
public class SceneSwitcher : MonoBehaviour
{
    public string newScene; // string name of scene to switch to

    [Tooltip("This is the name of the scene the SceneSwitcher is currently in.")]
    public string currentScene;

    [Header("Tutorial Scene Data")]
    public int numTargets = -1; // current number of active targets

    void Start()
    {
        print("Number of targets in scene: " + numTargets);
    }

    // Update is called once per frame
    void Update()
    {
        // if in the tutorial and john has shot all the targets, return!
        if (/*currentScene == "TargetPractice2" && */ numTargets == 0)
        {
            //print("Calling changescenes(null, false). Switching to scene + " + newScene);
            GameManager.sGameManager.tutorialComplete = true;
            print("Setting tutorialComplete to true. TutorialComplete: " + GameManager.sGameManager.tutorialComplete);
            ChangeScenes(null, false); // don't need to pass in a John GO because we're not saving his data
        }

        // if this is the tutorial trigger and the tutorial is complete, disable trigger
        if (GameManager.sGameManager.tutorialComplete && gameObject.name == "TutorialCollider")
        {
            print("Setting gameObject false on gameObject named " + gameObject.name);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !GameManager.sGameManager.tutorialComplete)
        {
            print("Player hit collider!!");
            ChangeScenes(other.gameObject, true);
        }
    }

    void ChangeScenes(GameObject john, bool saveData)
    {
        // ---- SAVE DATA FROM CURRENT SCENE ----

        // gun is not a component of John, but a component of ProjectileAnchor
        // ProjectileAnchor is a grandchild of John.
        if (saveData)
        {
            //print("Saving data from SceneSwitcher for scene: " + currentScene);
            GameManager.sGameManager.savedLoadedAmmo = john.GetComponentInChildren<Gun>().currentLoaded;
            GameManager.sGameManager.savedTotalAmmo = john.GetComponentInChildren<Gun>().totalAmmo;
            GameManager.sGameManager.savedPosition = john.transform.localPosition;

            //print("Saved position: " + GameManager.sGameManager.savedPosition);
        }

        // ---- CHANGE SCENES ----
        if (newScene != "" && newScene != null)
        {
            Debug.Log("Changing scenes from: " + SceneManager.GetActiveScene().name + " to " + newScene);
            SceneManager.LoadScene(newScene);
        }
        else
        {
            Debug.LogError("No scene name given in SceneSwitcher.");
        }
    }
}
