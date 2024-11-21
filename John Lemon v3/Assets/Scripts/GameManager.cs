using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager sGameManager; // singleton - one GM to control entire game

    [SerializeField]
    private GameObject john;
    [SerializeField]
    private Gun gun;

    public SceneSwitcher sceneSwitcher;

    // this is information that needs to be saved and reloaded between scene changes
    [Header("Saved Data")]
    public int savedLoadedAmmo;
    public int savedTotalAmmo;
    public Vector3 savedPosition;

    void Awake()
    {
        print("Awake called for gamemanager. Current scene: " + SceneManager.GetActiveScene().name);
        if (sGameManager)
        {
            Destroy(this);
        }
        else
        {
            sGameManager = this;
        }
        DontDestroyOnLoad(this); // allows GameManager singleton's data to be saved between scenes

        // subscribe to sceneLoaded event for load order purposes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene loaded: " + scene.name);
        print("Active scene: " + SceneManager.GetActiveScene().name);

        if (scene.name != SceneManager.GetActiveScene().name)
        {
            Debug.LogError("Loaded scene does not match active scene.");
        }

        sceneSwitcher = null; // clear any old sceneswitchers

        john = GameObject.FindGameObjectWithTag("Player");
        gun = john.GetComponentInChildren<Gun>();
        sceneSwitcher = GameObject.FindGameObjectWithTag("SceneSwitcher")?.GetComponent<SceneSwitcher>();

        if (sceneSwitcher != null)
        {
            sceneSwitcher.currentScene = scene.name;
            // in the tutorial scene, we change scenes (back to the first scene)
            // when john has shot all the targets
            if (sceneSwitcher.currentScene == "TargetPractice2")
            {
                print("GameManager: Current scene is TargetPractice2.");
                sceneSwitcher.numTargets = 5;
                sceneSwitcher.newScene = "SampleScene";

                gun.totalAmmo = 1000;
            }
            else
            {
                sceneSwitcher.numTargets = -1;

                gun.totalAmmo = savedTotalAmmo;
                gun.currentLoaded = savedLoadedAmmo;
            }
        }
        else
        {
            Debug.LogError("Sceneswitcher not found in scene: " + scene.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveTarget()
    {
        print("RemoveTarget(). Now calling sceneSwitcher.");
        sceneSwitcher.numTargets--;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
