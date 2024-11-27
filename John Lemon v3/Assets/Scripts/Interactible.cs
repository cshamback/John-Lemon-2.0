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

    [Header("Ammo and Weapons Only: ")]
    public GameObject john; // used to get john's position and decide if close enough
    [SerializeField]
    private Gun gun;
    public int ammoAmount;

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

        gun = john.GetComponentInChildren<Gun>(); // one of john's children has the gun gameobject 
    }

    // Update is called once per frame
    void Update()
    {
        // distance between this object and john is < 1m
        float distance = Vector3.Distance(john.transform.position, gameObject.transform.position);
        if (distance < 1)
        {

            // enable outline 
            outline.enabled = true;

            // deal with tooltip: set position and enable 
            // set to the position of this gameobject in world space - move up 0.5m 
            tooltipText.transform.position = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up / 2));
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
                        break;
                    case eObjectType.weapon:
                        print("Weapon picked up: " + gameObject.name);
                        Gun weapon = gameObject.GetComponent<Gun>();

                        if (weapon == null)
                        {
                            Debug.LogError("Gameobject " + gameObject.name + " does not have a Gun component.");
                        }
                        else
                        {
                            gun.SetStats();
                        }

                        // weapon cannot be picked up again
                        Destroy(gameObject);
                        break;
                    case eObjectType.door:
                        print("Door interacted with!");
                        foreach (KeyValuePair<string, bool> pair in GameManager.sGameManager.keyDict)
                        {
                            print("Current key value pair: \nKey: " + pair.Key + "\nValue: " + pair.Value);
                            if (pair.Key == door.name)
                            {
                                if (pair.Value == true)
                                {
                                    print("John has key for door " + door.name);
                                    doorScript.OpenDoor(); // for now, just sets gameobject.SetActive(false)
                                    break;
                                }
                                else
                                {
                                    print("John does NOT have the key for this door.");
                                }
                            }
                        }
                        Debug.LogError("Could not find door named " + door.name + " in keyDict.");
                        break;
                    case eObjectType.key:
                        print("Key interacted with!");
                        foreach (KeyValuePair<string, bool> pair in GameManager.sGameManager.keyDict)
                        {
                            print("Current key value pair: \nKey: " + pair.Key + "\nValue: " + pair.Value);
                            if (pair.Key == doorName)
                            {
                                print("Found key-value pair. Updating keyDict.");
                                GameManager.sGameManager.keyDict[pair.Key] = true;
                                break;
                            }
                        }
                        Debug.LogError("Did not find key for door " + doorName);
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
