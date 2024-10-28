using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] //Requires there to be a navmesh agent component.
public class Crawler : MonoBehaviour
{
    [SerializeField] private Transform target; //What the agent will move towards.
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
}
