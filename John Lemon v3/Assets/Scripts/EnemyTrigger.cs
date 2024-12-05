using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject enemyModel; //Assign the model to this in the inspector.
    public Transform player;
    Crawler crawler;
    public GameObject jumpDetector;

    void Start()
    {
        //Access the crawker object
        crawler = GetComponentInParent<Crawler>();
        enemyModel.SetActive(false);
        jumpDetector.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
        {
            enemyModel.SetActive(true);
            crawler.Activate();
            gameObject.SetActive(false);
            jumpDetector.SetActive(true);
        }
    }
}
