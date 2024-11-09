using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public enum eObjectType { readableObject, ammo, weapon }

public class Interactible : MonoBehaviour
{
    public GameObject john; // used to get john's position and decide if close enough
    [SerializeField]
    private Gun gun;
    public int ammoAmount;

    public cakeslice.Outline outline; // used to turn outline on and off

    public eObjectType type;

    public TextMeshProUGUI tooltipText;

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

                        // open readableObject ui with correct contents
                        // can be text or image 

                        break;
                    case eObjectType.ammo:
                        print("Ammo selected.");

                        // deal with ammo being picked up 
                        if (ammoAmount > 0)
                        {
                            gun.PickUpAmmo(ammoAmount);
                            print("From interactible: John picked up ammo amount " + ammoAmount);
                        }
                        else
                        {
                            Debug.LogError("Tried to pick up ammo from object " + this.gameObject.name + ", but ammo amount is " + ammoAmount);
                        }

                        // ammo cannot be picked up again 
                        Destroy(gameObject);
                        break;
                    case eObjectType.weapon:
                        print("Weapon selected.");

                        // weapon cannot be picked up again
                        Destroy(gameObject);
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
