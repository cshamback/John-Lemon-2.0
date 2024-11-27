using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: implement actual physical opening of door 
    public void OpenDoor()
    {
        print("Calling OpenDoor.");
        gameObject.SetActive(false);
    }
}
