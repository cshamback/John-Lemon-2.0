using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Needed to access input system library for player movement
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject gunGO; // location on John's model he fires from
    public Gun projectileAnchor; // handles Shoot() method

    // amount the player is able to move the laser sight up or down while aiming 
    public float anchorVerticalRange = 0.01f;
    public Vector3 anchorOriginalPosition; // position of gun GameObject - throws an error if i try to set it up here. gets init in start()

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
        {   // do animation
            animator.SetBool("isAiming", true);
            animator.SetBool("IsWalking", false);

            /*Debug.Log("Anchor original position: " + anchorOriginalPosition +
                " Anchor current position: " + projectileAnchor.transform.position +
                " Anchor local position: " + projectileAnchor.transform.localPosition);*/
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

        // localPosition is position relative to parent, not world
        anchorOriginalPosition = gunGO.transform.localPosition; // position to return to when not aiming
        projectileAnchor.transform.localPosition = anchorOriginalPosition; // enter this position on start
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (checkForAim())
        {
            // handles laser sight and firing
            projectileAnchor.Aim();
            // allow player to move gun up/down with W and S only when aiming

            if (Input.GetKey(KeyCode.W))
            { // move projectileAnchor up, within limit
                print("Aiming up.");
                if (projectileAnchor.transform.localPosition.z < anchorOriginalPosition.z + anchorVerticalRange)
                {
                    projectileAnchor.transform.localPosition += new Vector3(0, 0, 0.0005f);
                    print("Current position: " + projectileAnchor.transform.localPosition.z +
                        "Upper range: " + (anchorOriginalPosition.z + anchorVerticalRange));
                }
            }
            else if (Input.GetKey(KeyCode.S))
            { // move projectileAnchor down, within limit
                print("Aiming down.");
                if (projectileAnchor.transform.localPosition.z > anchorOriginalPosition.z - anchorVerticalRange)
                {
                    projectileAnchor.transform.localPosition -= new Vector3(0, 0, 0.0005f);
                    print("Current position: " + projectileAnchor.transform.localPosition.z +
                        "Lower range: " + (anchorOriginalPosition.z - anchorVerticalRange));
                }
            }
        }
        else
        {
            projectileAnchor.StopAiming(); // hides laser sight
            projectileAnchor.transform.localPosition = anchorOriginalPosition; // puts gun back after player done aiming
        }
    }
}
