using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    Crawler crawler;

    void Start()
    {
        //Access the crawker object
        crawler = GetComponentInParent<Crawler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
        {
            crawler.Jump();
            //Debug.Log("Player has entered the jump trigger");
        }
    }
}
