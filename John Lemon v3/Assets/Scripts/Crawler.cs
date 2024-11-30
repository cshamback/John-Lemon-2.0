using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    bool canHurt = false;
    float damageBaseDistance;
    [SerializeField] float damageBonusDistance = 0.1f;
    RaycastHit hitInfo;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        collide = GetComponent<Collider>();
        enemyMap = GetComponentInChildren<Renderer>().material;


        disToGround = collide.bounds.extents.y;
        damageBaseDistance = collide.bounds.extents.z;

        agent.isStopped = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Activate()
    {
        Jump();
        Invoke("KeepWalking", 0.8f);
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.isStopped == false)
        {
            agent.destination = target.transform.position;
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
        canHurt = false;
        animator.SetTrigger("WalkTrigger");
        rb.isKinematic = true;
        rb.isKinematic = false; //Resets the velocity of the rigidbody so it stops sliding.
        rb.constraints = RigidbodyConstraints.None; //Freezes the y position of the rigidbody.
        agent.isStopped = false; //Resumes the agent's movement. 
    }

    private void IsGrounded() //This is to check if the enemy has hit the ground yet.
    {
        bool grounded = false;
        while (!grounded)
        {
            grounded = Physics.Raycast(transform.position, -Vector3.up, disToGround + 0.1f);
            if (grounded)
            {
                rb.velocity = Vector3.zero; //Resets the velocity of the rigidbody so it stops sliding.
                rb.constraints = RigidbodyConstraints.FreezeAll; //Freezes the y position of the rigidbody.
                Invoke("dealDamage", 0.5f); //Gives the animation time to get to the contact part.
            }
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        bloodSplat.Play();
        if (health <= 0)
        {
            agent.isStopped = true;
            bloodSplat.Play();
            bloodSplat.Play();
            animator.SetTrigger("DeathTrigger");
            Destroy(gameObject, 0.25f);
        }
    }

    public void dealDamage()
    {
        canHurt = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, damageBaseDistance + damageBonusDistance);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * (damageBaseDistance + damageBonusDistance), Color.red);
        if (canHurt && hitInfo.transform == target.transform)
        {
            Debug.Log("take that");
            //Put a call to the deal damage script on the player side here, for say 15 damage.
            Invoke("dealDamage", 0.833f); //Recursievly call the function to check and deal
        }
    }
}
