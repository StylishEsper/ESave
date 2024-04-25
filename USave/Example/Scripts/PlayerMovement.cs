//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Player movement for sample game.
//***************************************************************************************

using UnityEngine;

namespace Esper.USave.Example
{
    public class PlayerMovement : MonoBehaviour
    {
        [Tooltip("How fast the player can move.")]
        [SerializeField][Min(0)] private float movementSpeed = 1;

        [Tooltip("The strength of gravity.")]
        [SerializeField][Min(0)] private float gravity = 9.81f;

        private SpriteRenderer spriteRenderer;
        private CharacterController characterControl;
        private Animator anim;

        private Vector3 input;

        private float velocityY;
        private bool isRunning;

        private void Start()
        {
            // Get attached components
            anim = GetComponent<Animator>();
            characterControl = GetComponent<CharacterController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // Check if the player is running
            isRunning = Input.GetKey(KeyCode.LeftShift);

            anim.speed = 1;

            // Get the user input
            input = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Change animator state if moving
            if (input != Vector3.zero)
            {
                // Set flip
                spriteRenderer.flipX = input.x < 0;

                // Set anim speed
                if (isRunning)
                {
                    anim.speed = 2;
                }

                // Set animation state
                anim.Play("Walk");
            }
            else
            {
                // Only use gravity
                input = new(0, velocityY, 0);

                // Set idle animation state
                anim.Play("Idle");
            }
        }

        private void FixedUpdate()
        {
            // Calculate movement
            input *= (isRunning ? movementSpeed * 2 : movementSpeed) * Time.fixedDeltaTime;

            // Calculate gravity based on whether the player is grounded or not
            velocityY = characterControl.isGrounded ? 0 : velocityY -= gravity * Time.fixedDeltaTime;
            input.y = velocityY;

            // Apply movement
            characterControl.Move(input);
        }
    }
}