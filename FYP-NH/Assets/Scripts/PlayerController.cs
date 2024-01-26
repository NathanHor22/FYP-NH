using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = .8f;

    //References to playerInput component
    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    //basic player movement references
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction dashAction;

    //bool isRunning;
    //float stamina = 5f;
    //float maxStamina = 5f;
    
    public float dashLength = 0.5f;
    public float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    private float activeMoveSpeed;
    public float dashSpeed;
    
    private void Start()
    {
        activeMoveSpeed = playerSpeed;

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Run"];
        dashAction = playerInput.actions["Dash"];
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x , 0, input.y);

        //Camera follows where the player is facing
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if(runAction.triggered) {
            
        }

        if(dashAction.triggered) {
            Debug.Log("Dashing lil bro");
            if(dashCoolCounter <=0 && dashCounter <= 0) {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if(dashCounter > 0) {
            dashCounter -= Time.deltaTime;

            if( dashCounter <= 0) {
                activeMoveSpeed = playerSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if(dashCoolCounter > 0) {
            dashCoolCounter -= Time.deltaTime;
        }


        //Player looks to where the camera is directed
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //Concept: player can only sprint for 5 seconds 
    //Counter to count the moment the runaction is triggered
    //Once counter reaches 5, stop the sprint and then have cooldown on the sprint 5 seconds
    // //after 5 seconds can sprint again
}

