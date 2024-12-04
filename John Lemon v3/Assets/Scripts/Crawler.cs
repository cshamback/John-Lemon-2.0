using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] //Requires there to be a navmesh agent component.
[RequireComponent(typeof(Rigidbody))] //Requires there to be a rigidbody component.
public class Crawler : MonoBehaviour
{
    [SerializeField] private GameObject target; //What the agent will move towards.
    //Components / objects
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private Collider collide;
    private Material enemyMap;
    public ParticleSystem bloodSplat;
    //Varaibles
    [SerializeField] Vector3 jumpVector = new Vector3(0, 1, 1); //The vector that will be used to apply a force to the rigidbody.
    public int health = 100;
    float disToGround;
    [SerializeField] private bool canHurt = false; //making these viewable for debugging purposes.
    [SerializeField] bool canCheckRange = true; //same.
    bool lookAtPlayer = false;
    Vector3 playerDirection;
    Quaternion lookRotation;

    //components to deactivate
    CapsuleCollider toucher;
    SphereCollider youThere;
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        collide = GetComponent<Collider>();
        enemyMap = GetComponentInChildren<Renderer>().material;
        toucher = GetComponent<CapsuleCollider>();
        youThere = GetComponent<SphereCollider>();

        disToGround = collide.bounds.extents.y;

        agent.isStopped = true;

        toucher.enabled = false;
        youThere.enabled = false;
    }

    public void Activate()
    {
        Jump();
        canHurt = false;
        toucher.enabled = true;
        youThere.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.isStopped == false)
        {
            agent.destination = target.transform.position;
        }

/*
        if(lookAtPlayer)
        {
            playerDirection = (target.transform.position - transform.position).normalized;
            playerDirection.y = 0;
            lookRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f);
        }
        */
    }

    private void OnTriggerEnter(Collider other) { //This trigger is for if the player is close enough to attack.
        if(other.transform.tag == "Player")
        {
            Debug.Log("TriggerEnter is being called");
            //Debug.Log("CanAttack");
            agent.isStopped = true;
            animator.SetTrigger("AttackTrigger");
            canHurt = true;
            if(canCheckRange)
            {
                //Debug.Log("Telling it to attack.");
                canCheckRange = false;
                animator.SetTrigger("AttackTrigger");
                Invoke("dealDamage", 0.833f);
            }
        }
    }

    private void OnTriggerExit(Collider other) { //This trigger is for if the player is too far away to attack.
        if(other.transform.tag == "Player")
        {
        /*
        rb.isKinematic = true;
        rb.isKinematic = false; //Resets the velocity of the rigidbody so it stops sliding.
        rb.constraints = RigidbodyConstraints.None; //Freezes the y position of the rigidbody.
        */
            //Debug.Log("Can't Attack");
            canHurt = false;
            agent.isStopped = false;
            animator.SetTrigger("WalkTrigger");
        }
    }

    //Calls the jump animation
    public void Jump()
    {
        lookAtPlayer = false;
        agent.isStopped = true; //Stops the agent from moving.
        animator.SetTrigger("JumpTrigger");
        Vector3 force = transform.forward * jumpVector.z + transform.up * jumpVector.y;
        rb.AddForce(force, ForceMode.Impulse); // Applies an impulse force in the forward direction.
        Invoke("IsGrounded", 0.75f);
        canHurt = false;

    }
    private void IsGrounded() //This is to check if the enemy has hit the ground yet.
    {
        bool grounded = false;
        while(!grounded)
        {
            grounded = Physics.Raycast(transform.position, -Vector3.up, disToGround + 0.1f);
            if(grounded)
            {
                lookAtPlayer = true;
                rb.velocity = Vector3.zero; //Resets the velocity of the rigidbody so it stops sliding.
                //rb.constraints = RigidbodyConstraints.FreezeAll; //Freezes the y position of the rigidbody.
                agent.isStopped = false; //Starts the agent moving again.
            }
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        bloodSplat.Play();
        if(health <= 0)
        {
            agent.isStopped = true;
            bloodSplat.Play();
            bloodSplat.Play();
            animator.SetTrigger("DeathTrigger");
            Vector3 force = -transform.forward * jumpVector.z;
            rb.AddForce(force, ForceMode.Impulse); // Applies an impulse force in the forward direction.
            Destroy(gameObject, 0.75f);
        }
    }

/*
I'm quite proud of the logic behind this method. by having a boolean gate sustem,
it ensures that the method can only be called once every 0.8 seconds.
Outside functions set canHurt to be true so that the method can deal damage.
But before damage can start, the method to call the damage dealer can only be called 
If the canCheckRange function is true, then outside functions are allowed to call the method,
and canCheckRange is set to false so that isn't not called repeatedly. canCheckRange can only
be set to true by the damage dealer methods, which sets it to true if it was not able to deal
damage when called. the dealDamage method also calls itself repeatedly, to ensure it keeps
dealing damage if nothing has changed. Basically, canHurt tells whether or not the enemy is
capable of attacking, and canCheckRange is used to ensure outisde methods don't repeatedly call
it while it's calling itelf. They can only call it if it's done calling itself.
*/
    public void dealDamage()
    {
        Debug.Log("dealDamage is being called");
        if(target.activeSelf == false) //The player was killed.
            {
                animator.SetTrigger("DeathTrigger");
                canHurt = false;
                canCheckRange = false;
            }
        if(canHurt)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll; //Freezes the y position of the rigidbody.
            canCheckRange = false;
            Debug.Log("growl take that");
            target.GetComponent<PlayerController>().hurtPlayer(10);
            animator.SetTrigger("AttackTrigger");
            Invoke("dealDamage", 0.833f); //Recursievly call the function to check and deal
        } else
        {
            //Debug.Log("Can't attack");
            rb.constraints = RigidbodyConstraints.None; //unFreezes the y position of the rigidbody.
            canCheckRange = true;
        }
    }
}
