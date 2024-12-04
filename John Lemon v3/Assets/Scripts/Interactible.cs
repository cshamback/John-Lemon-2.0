using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public enum eObjectType { readableObject, ammo, weapon, door, key }

public class Interactible : MonoBehaviour
{
    [Header("General")]
    public cakeslice.Outline outline; // used to turn outline on and off
    public eObjectType type;
    public TextMeshProUGUI tooltipText;
    public GameObject john; // used to get john's position and decide if close enough
    public GameObject enemies; //Use to set enemies to active once a weapon is picked up.

    [Header("Ammo and Weapons Only: ")]
    [SerializeField]
    private Gun gun;
    public int ammoAmount;
    public int damageAmount;
    public int range;
    public MeshRenderer gunMesh;

    [Header("Readable Objects Only: ")]
    public Sprite readableImage; // the image to use
    public Image readableCanvasImage; // the place for the image to go 
    public GameObject readableCanvas;

    [Header("Doors only: ")]
    public GameObject door;
    public Door doorScript;

    [Header("Keys Only: ")]
    public string doorName;

    // Start is called before the first frame update
    void Start()
    {
        tooltipText.enabled = false;

        john = GameObject.FindGameObjectWithTag("Player");
        gun = john.GetComponentInChildren<Gun>(); // one of john's children has the gun gameobject 

        if (door != null)
        {
            doorScript = door.GetComponent<Door>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // distance between this object and john is < 2m
        float distance = Vector3.Distance(john.transform.position, gameObject.transform.position);

        if (distance < 2.5)
        {
            Debug.Log("John is in range.");
            // enable outline 
            outline.enabled = true;

            // deal with tooltip: set position and enable 
            if (type == eObjectType.key || type == eObjectType.weapon)
            {
                // bottom center of screen 
                // yeah yeah, hard coding bad
                // sue me 
                // or better yet, get the Camera.ScreenHeight or whatever and fix it 
                // 1080x1920 is goated tho so if you change the screen resolution and it fucks up that's on you 
                tooltipText.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(960, 200, 0));
            }
            else
            {
                // set to the position of this gameobject in world space - move up 0.5m 
                tooltipText.transform.position = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up / 2));
            }
            tooltipText.enabled = true;

            // if E is pressed while in range, handle interaction
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("Object selected: " + gameObject.name);
                switch (type)
                {
                    case eObjectType.readableObject:
                        print("Readable object selected.");

                        // open readableObject ui with correct image
                        readableCanvasImage.sprite = readableImage;
                        readableCanvas.SetActive(true);

                        break;
                    case eObjectType.ammo:
                        print("Ammo selected.");

                        // deal with ammo being picked up 
                        if (ammoAmount > 0)
                        {
                            gun.PickUpAmmo(ammoAmount);
                        }
                        else
                        {
                            Debug.LogError("Tried to pick up ammo from object " + this.gameObject.name + ", but ammo amount is " + ammoAmount);
                        }

                        // ammo cannot be picked up again 
                        Destroy(gameObject);
                        tooltipText.enabled = false;
                        break;
                    case eObjectType.weapon:
                        print("Weapon picked up: " + gameObject.name);
                        gun.SetProperties(damageAmount, range, gunMesh);

                        enemies.SetActive(true); // set enemies to active once weapon is picked up
                        
                        // weapon cannot be picked up again
                        Destroy(gameObject);
                        tooltipText.enabled = false;

                        break;
                    case eObjectType.door:
                        print("Door interacted with!");
                        foreach (KeyValuePair<string, bool> pair in GameManager.sGameManager.keyDict)
                        {
                            print("Current key value pair - Key: " + pair.Key + "Value: " + pair.Value);
                            if (pair.Key == door.name)
                            {
                                if (pair.Value == true)
                                {
                                    print("John has key for door " + door.name);
                                    doorScript.OpenDoor(); // for now, just sets gameobject.SetActive(false)
                                    tooltipText.enabled = false;
                                    break;
                                }
                                else
                                {
                                    print("John does NOT have the key for this door.");
                                }
                            }
                        }
                        break;
                    case eObjectType.key:
                        Debug.Log("Key to door " + doorName + " picked up.");
                        foreach (KeyValuePair<string, bool> pair in GameManager.sGameManager.keyDict)
                        {
                            Debug.Log("Current key value pair - Key: " + pair.Key + " Value: " + pair.Value);
                            Debug.Log("doorName: " + doorName + " pair.Key: " + pair.Key);
                            if (pair.Key == doorName)
                            {
                                GameManager.sGameManager.keyDict[pair.Key] = true;
                                Debug.Log("Found key-value pair. Updating keyDict. New key: " + pair.Key + " Value: " + GameManager.sGameManager.keyDict[pair.Key]);
                                break;
                            }
                        }
                        Debug.Log("KeyDict for loop finished.");
                        Destroy(gameObject); // cannot pick up key multiple times
                        tooltipText.enabled = false;
                        break;
                    default:
                        Debug.LogError("Type does not exist: " + type);
                        break;
                }
            }
        }
        else
        {
            //print("out of range. Distance: " + distance);
            outline.enabled = false;
            tooltipText.enabled = false;
        }
    }
}
