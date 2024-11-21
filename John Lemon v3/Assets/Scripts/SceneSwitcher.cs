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
    public int numTargets; // current number of active targets

    // called AFTER scenemanager.loadscene is fully finished
    void Start()
    {
        // once new scene has loaded, set the active scene to the current scene name.
        // this allows the gamemanager to figure out what the current scene is and act accordingly.
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        print("SceneSwitcher start method called.");
    }

    // Update is called once per frame
    void Update()
    {
        // if in the tutorial and john has shot all the targets, return!
        if (currentScene == "TargetPractice2" && numTargets <= 0)
        {
            print("Changing scenes.");
            ChangeScenes(null, false); // don't need to pass in a John GO because we're not saving his data
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
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
            GameManager.sGameManager.savedLoadedAmmo = john.gameObject.GetComponentInChildren<Gun>().currentLoaded;
            GameManager.sGameManager.savedTotalAmmo = john.gameObject.GetComponentInChildren<Gun>().totalAmmo;
            GameManager.sGameManager.savedPosition = john.transform.position;
        }

        // ---- CHANGE SCENES ----
        if (newScene != "" && newScene != null)
        {
            print("Changing scenes from: " + SceneManager.GetActiveScene().name + " to " + newScene);
            SceneManager.LoadScene(newScene);
        }
        else
        {
            Debug.LogError("No scene name given in SceneSwitcher.");
        }
    }
}
