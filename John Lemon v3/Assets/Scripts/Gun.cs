using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

// this script is attached to the ProjectileAnchor gameobject
// this can be attached to a gun mesh, or in the absense of a gun John himself. 

public class Gun : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f; // how far player can shoot
    public int totalAmmo = 0;
    public int currentLoaded = 0;

    public GameObject john; // John Lemon > has rotation values (john's components rotate relative to him, which is to say not at all)
    public GameObject anchor; // the gun has the position to shoot from (but not rotation for some reason)
    public GameObject sight; // sphere placed on whatever is being aimed at

    // HUD ammo info is updated by the gun, since the gun keeps track of its own ammo 
    public GameObject hud;
    public TextMeshProUGUI currentLoadedText;
    public TextMeshProUGUI totalAmmoText;

    public int maxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        sight.SetActive(false);
        hud.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // todo: add check if there is anything to reload 
        // reloading does not change the total count of ammo, only moves stuff around 
        if (Input.GetKeyDown(KeyCode.R))
        {
            // do math: 
            if (totalAmmo <= maxAmmo) // if current total ammo is less than what can be loaded, load all of it
            {
                // set amountLoaded to ammoCount, keep ammoCount the same
                currentLoaded = totalAmmo;
                currentLoadedText.text = totalAmmo.ToString();
            }
            else // more ammo than can be loaded at one time: 
            {
                // load the max amount of ammo, keep ammoCount the same 
                int diff = maxAmmo - currentLoaded;
                currentLoaded = maxAmmo;
                currentLoadedText.text = currentLoaded.ToString();
            }
        }
    }

    public void UpdateAmmoHUD(int total, int loaded)
    {
        totalAmmoText.text = total.ToString();
        currentLoadedText.text = loaded.ToString();
    }

    // called by Interactible with type Ammo 
    public void PickUpAmmo(int count)
    {
        totalAmmo += count;
        totalAmmoText.text = totalAmmo.ToString();
    }

    public void StopAiming()
    {
        sight.SetActive(false);
    }

    // called by PlayerController on John 
    public void Aim()
    {
        RaycastHit hit;

        Vector3 origin = anchor.transform.position;
        Vector3 direction = john.transform.forward;

        Debug.DrawLine(origin, origin + direction * range, Color.black);
        //print("Parent position: " + parent.transform.position);

        // even if not hitting anything, still decrease ammo if fire button pushed
        // decrease ammo by 1 
        if (currentLoaded > 0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            currentLoaded -= 1;
            totalAmmo -= 1;
            UpdateAmmoHUD(totalAmmo, currentLoaded);
        }

        //start at player position, shoot out ray
        if (Physics.Raycast(origin, direction, out hit, range))
        {
            //print("Aiming at object: " + hit.transform.name + " tag: " + hit.transform.tag);

            // put laser sight on whatever is in the way
            // endpoint = (origin + direction * range)
            sight.SetActive(true);
            sight.transform.parent = hit.transform; // make sight a child of hit object
            sight.transform.position = hit.point; // point is position where collider was hit

            // handle impact
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (currentLoaded > 0) // have to actually have ammo in order to shoot 
                {
                    //print("Hit something!");
                    switch (hit.transform.tag)
                    {
                        case "Target":
                            // target should have its own class.
                            // targets destroyed on hit, with vfx - this is handled in Target.GetShot()
                            Target target = hit.transform.gameObject.GetComponent<Target>();
                            if (target != null)
                            {
                                target.GetShot(damage);
                            }
                            else
                            {
                                Debug.LogError("Could not find Target on gameObject " + hit.transform.gameObject.name);
                            }

                            break;
                        case "Enemy":
                            print("Enemy hit.");

                            // enemies have their own classes
                            // they take damage based on the current gun equipped 

                            break;
                        default:
                            // default is something we don't care about, so ignore the hit
                            Debug.LogError("Hit unclassified object: " + hit.transform.gameObject.name +
                                " Tag: " + hit.transform.gameObject.tag);
                            break;
                    }
                }
            }
        }
        else // if nothing is being aimed at, turn the sight off
        {
            sight.SetActive(false);
        }
    }
}
