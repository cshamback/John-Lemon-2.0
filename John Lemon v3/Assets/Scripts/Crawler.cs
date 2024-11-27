using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    private Collider collide;
    //Varaibles
    [SerializeField] Vector3 jumpVector = new Vector3(0, 1, 1); //The vector that will be used to apply a force to the rigidbody.
    float disToGround;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        collide = GetComponent<Collider>();


        disToGround = collide.bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {
        if(agent.isStopped == false)
        {
            agent.destination = target.position;
        }
    }

    //Calls the jump animation
    public void Jump()
    {
        agent.isStopped = true; //Stops the agent from moving.
        animator.SetTrigger("JumpTrigger");
        Vector3 force = transform.forward * jumpVector.z + transform.up * jumpVector.y;
        rb.AddForce(force, ForceMode.Impulse); // Applies an impulse force in the forward direction.
        Invoke("IsGrounded", 0.75f);
    }

    //Makes the crawler start crawling again.
    public void KeepWalking()
    {
        animator.SetTrigger("WalkTrigger");
        rb.isKinematic = true;
        rb.isKinematic = false; //Resets the velocity of the rigidbody so it stops sliding.
        rb.constraints = RigidbodyConstraints.None; //Freezes the y position of the rigidbody.
        agent.isStopped = false; //Resumes the agent's movement. 
    }
    
private void IsGrounded() //This is to check if the enemy has hit the ground yet.
{
    bool grounded = false;
    while(!grounded)
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, disToGround + 0.1f);
        if(grounded)
        {
            grounded = true;
            rb.velocity = Vector3.zero; //Resets the velocity of the rigidbody so it stops sliding.
            rb.constraints = RigidbodyConstraints.FreezeAll; //Freezes the y position of the rigidbody.
        }
    }

}
}
