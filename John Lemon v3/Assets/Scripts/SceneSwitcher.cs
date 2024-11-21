using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class SceneSwitcher : MonoBehaviour
{
    public string newScene; // string name of scene to switch to 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Player walked over collider.");

            // ---- SAVE DATA FROM CURRENT SCENE ----

            // gun is not a component of John, but a component of ProjectileAnchor
            // ProjectileAnchor is a child of John. 
            if (GameManager.sGameManager == null)
            {
                Debug.LogError("sGameManager is null!");
            }
            if (other.gameObject == null)
            {
                Debug.LogError("other.gameObject is null!");
            }
            if (other.gameObject.GetComponentInChildren<Gun>() == null)
            {
                Debug.LogError("Gun is null!");
            }
            GameManager.sGameManager.savedLoadedAmmo = other.gameObject.GetComponentInChildren<Gun>().currentLoaded;
            GameManager.sGameManager.savedTotalAmmo = other.gameObject.GetComponentInChildren<Gun>().totalAmmo;

            // ---- CHANGE SCENES ----
            if (newScene != "")
            {
                SceneManager.LoadScene(newScene);
            }
            else
            {
                Debug.LogError("No scene name given in SceneSwitcher.");
            }
        }
    }
}
