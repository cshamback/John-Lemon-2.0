using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager sGameManager; // singleton - one GM to control entire game
    public GameObject john;
    public Gun gun;

    // this is information that needs to be saved and reloaded between scene changes
    [Header("Saved Data")]
    public int savedLoadedAmmo;
    public int savedTotalAmmo;

    void Awake()
    {
        if (sGameManager == null || sGameManager != this)
        {
            sGameManager = this;
        }
        DontDestroyOnLoad(this); // allows GameManager singleton's data to be saved between scenes
    }

    // Start is called before the first frame update
    void Start()
    {
        // for the tutorial scene, john is given 100 ammo but forced to reload on his own
        if (SceneManager.GetActiveScene().name == "TargetPractice2")
        {
            gun.totalAmmo = 1000;
        } // for all other scenes, he has the same amount of ammo 
        else
        {
            gun.totalAmmo = savedTotalAmmo;
            gun.currentLoaded = savedLoadedAmmo;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
