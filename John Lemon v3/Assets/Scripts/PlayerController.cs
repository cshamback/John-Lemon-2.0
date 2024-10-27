using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Needed to access input system library
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab; // prefab of bullet to be fired 

    public Gun projectileAnchor; // location on John's model he fires from -> handles Shoot() method
    public float projectileSpeed = 40;

    // Use to store player input
    private Vector2 playerInput;
    private Vector3 playerVelocity;

    private Rigidbody rigid;

    // Store ref to player's character controller to be used to move.
    // Fill out field in inspector
    [SerializeField] CharacterController controller;

    // Player speed adjustment value.
    [SerializeField] private float playerSpeed = 1.5f;

    // Player rotation speed adjustement
    [SerializeField] private float playerRotation = 100f;

    [SerializeField] private float gravityValue = -9.81f;

    private Animator animator;

    private void OnMove(InputValue value)
    {
        // store value received from input either keyboard or controller
        playerInput = value.Get<Vector2>();

        float inputMagnitude = playerInput.magnitude;

        // speed param DNE in animator 
        //animator.SetFloat("Speed", inputMagnitude);
    }

    private bool checkForAim()
    {
        // mouse button 1 is RMB (0 is LMB)
        // GetMouseButton returns true while user is holding down the RMB
        return Input.GetMouseButton(1);
    }

    private void PlayerMovement()
    {
        // Use transform.rotate API. Use the declaration that uses axis of rotation
        // and "X" amount of degrees to rotate around given axis by. We will use
        // transform.up since that is the center axis of our character. For the degrees,
        // take player.x input, multiply it by rotation speed and time.deltaTime to smooth.
        transform.Rotate(0, playerInput.x * playerRotation * Time.deltaTime, 0);

        bool isAiming = checkForAim();

        // if aiming, don't allow movement
        // just do animations
        if (isAiming)
        {
            // call laser class to do laser sight stuff

            // do animation
            animator.SetBool("isAiming", true);
            animator.SetBool("IsWalking", false);
        }
        else // not aiming: allow movement! Also, set animations
        {
            // let him stop aiming please for the love of got
            animator.SetBool("isAiming", false);

            Vector3 move = transform.forward * playerInput.y;

            // Use characxter controller API to move player. transform.forward
            // Gives player's facing direction, which is multiplied by speed,
            // and time.deltatime to make it smoother, and playinput.y to only
            // allow for forward and backward movement.
            controller.Move(move * playerSpeed * Time.deltaTime);

            if (controller.isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            // move player in y direction and set animations
            playerVelocity.y += gravityValue * Time.deltaTime;

            controller.Move(playerVelocity * Time.deltaTime);

            bool isMoving = playerInput.magnitude > 0;
            animator.SetBool("IsWalking", isMoving);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (checkForAim())
        {
            // handles aiming and firing
            projectileAnchor.Aim();
        }
        else
        {
            projectileAnchor.StopAiming(); // hides laser sight
        }
    }
}
