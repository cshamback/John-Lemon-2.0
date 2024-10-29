using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// this script is attached to the ProjectileAnchor gameobject
// this can be attached to a gun mesh, or in the absense of a gun John himself. 

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f; // how far player can shoot

    public GameObject john; // John Lemon > has rotation values (john's components rotate relative to him, which is to say not at all)
    public GameObject anchor; // the gun has the position to shoot from (but not rotation for some reason)
    public GameObject sight; // sphere placed on whatever is being aimed at

    // Start is called before the first frame update
    void Start()
    {
        sight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopAiming()
    {
        sight.SetActive(false);
    }

    public void Aim()
    {
        RaycastHit hit;

        Vector3 origin = anchor.transform.position;
        Vector3 direction = john.transform.forward;

        Debug.DrawLine(origin, origin + direction * range, Color.black);
        //print("Parent position: " + parent.transform.position);

        //start at player position, shoot out ray
        if (Physics.Raycast(origin, direction, out hit, range))
        {
            // put laser sight on whatever is in the way
            // endpoint = (origin + direction * range)
            sight.SetActive(true);
            sight.transform.parent = hit.transform; // make sight a child of hit object
            sight.transform.position = hit.point; // point is position where collider was hit

            // handle impact
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                switch (hit.transform.tag)
                {
                    case "Target":
                        print("Target hit.");

                        // target should have its own class.
                        // targets destroyed on hit, with vfx

                        break;
                    case "Enemy":
                        print("Enemy hit.");

                        // enemies have their own classes
                        // they take damage based on the current gun equipped 

                        break;
                    default:
                        // default is something we don't care about, so ignore the hit
                        break;
                }
            }
        }
        else // if nothing is being aimed at, turn the sight off
        {
            sight.SetActive(false);
        }
    }
}
