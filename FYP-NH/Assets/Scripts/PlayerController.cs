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
    private float rotationSpeed = 5f;

    [SerializeField]
    public float playerHealth = 100f;

    [SerializeField]
    private float animationSmoothTime = 0.1f;
    [SerializeField]
    private float animationPlayTransition = 0.15f;

    //Initializes basic character physics
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    //player moveset
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction AttackAction;

    //player animations
    private Animator animator;
    int jumpAnimation;

    int attackAnimation;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;
    
    public AudioSource walkSound;

    public AudioSource attackSound;
    private void Awake()
    {

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        //makes it easier to call all the input functions instead of typing it out every single time
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        AttackAction = playerInput.actions["Attack"];
        //locks the cursor and disappear when it goes to game mode
        Cursor.lockState = CursorLockMode.Locked;
        
        //Animations
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Jumping");
        attackAnimation = Animator.StringToHash("Attack");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
    }



    void Update()
    {
        //Setting jumping and jump animation
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            
        }
        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x , 0, currentAnimationBlendVector.y);

        //Camera follows where the player is facing
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        //Blending the strafing animation
        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Player looks to where the camera is directed
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //Audio for footsteps
            if(moveAction.triggered) {
                walkSound.enabled = true;
        } else {
            walkSound.enabled = false;
        }

        if(AttackAction.triggered) 
        {
            Debug.Log("Attacking");
            animator.CrossFade(attackAnimation, animationPlayTransition);
            attackSound.enabled = true;
        }
        else
        {
            attackSound.enabled = false;    
        }
    }

    //Concept: player can only sprint for 5 seconds 
    //Counter to count the moment the runaction is triggered
    //Once counter reaches 5, stop the sprint and then have cooldown on the sprint 5 seconds
    // //after 5 seconds can sprint again



}

