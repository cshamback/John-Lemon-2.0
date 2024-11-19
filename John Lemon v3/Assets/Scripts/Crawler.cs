using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] //Requires there to be a navmesh agent component.
[RequireComponent(typeof(Rigidbody))] //Requires there to be a rigidbody component.
public class Crawler : MonoBehaviour
{
    [SerializeField] private Transform target; //What the agent will move towards.
    //Components
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    //Varaibles
    [SerializeField] Vector3 jumpVector = new Vector3(0, 1, 1); //The vector that will be used to apply a force to the rigidbody.


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }

    //Calls the jump animation
    public void Jump()
    {
        animator.SetTrigger("JumpTrigger");
        agent.isStopped = true; //Stops the agent from moving.
        //Vector3 force = transform.forward * jumpVector.z + transform.up * jumpVector.y;
        //rb.AddForce(force, ForceMode.Impulse); // Applies an impulse force in the forward direction.
    }

    //Makes the crawler start crawling again.
    public void keepWalking()
    {
        animator.SetTrigger("WalkTrigger");
        agent.isStopped = false; //Resumes the agent's movement.
    }
}
