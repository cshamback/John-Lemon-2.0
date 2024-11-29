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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform == player)
        {
            crawler.KeepWalking();
        }
    }
}
