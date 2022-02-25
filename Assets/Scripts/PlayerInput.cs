using UnityEngine;

namespace Platformer.Inputs{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInput : MonoBehaviour
    {
        public bool Activated = true;
        [SerializeField] private float deadZone = 0.01f;
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private LayerMask groundedMask = 0;
        [SerializeField] private LayerMask jumpOffMask = 0;
        [SerializeField] private PlayerAttack playerAttack = null;
        [SerializeField] private Joystick joystick = null;
        [SerializeField] private float overlapSphereExtension = 0.05f;
        [SerializeField] private Vector2 overlapSpherePosition = Vector2.zero;
        [SerializeField] private CircleCollider2D circleCollider2D = null;
        [SerializeField] private float timeToJumpOff = 0.5f;
        private float horizontalInput = 0f;
        private float verticalInput = 0f;
        private float manualHorizontalInputLeft = 0f;
        private float manualHorizontalInputRight = 0f;
        private float jumpOffTimer = 0f;
        private PlayerMovement playerMovement = null;
        private SpriteRenderer spriteRenderer = null;
        private Animator animator = null;
        private bool jumpButtonPressed = false;
        private bool attacking = false;
        private bool altAttacking = false;
        private bool manualAttack = false;
        private bool manualAltAttack = false;
        private bool manualJump = false;
        private Vector2 overlapCircleTransform = Vector2.zero;
        private int playerLayer;
        private int groundJumpOffLayer;
        private LayerMask actualMask = 0;
        private Rigidbody2D rigidBody;

        void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
            playerLayer = LayerMask.NameToLayer(GlobalStringVars.PLAYER_LAYER);
            groundJumpOffLayer = LayerMask.NameToLayer(GlobalStringVars.GROUND_JUMP_OFF_LAYER);
            actualMask = groundedMask;
        }
        void Update()
        {
            if(Activated)
            {
                // tracking horizontal axis
                horizontalInput = 0f;
                horizontalInput += Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
                if(joystick != null)
                {
                    horizontalInput += joystick.Horizontal;
                }
                horizontalInput += manualHorizontalInputRight - manualHorizontalInputLeft;
                horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

                // tracking vertical axis
                verticalInput = 0f;
                verticalInput += Input.GetAxisRaw(GlobalStringVars.VERTICAL_AXIS);
                verticalInput += joystick.Vertical;
                verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);
                // making player either move either crouch
                if(Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
                {
                    horizontalInput = 0;
                }
                else
                {
                    verticalInput = 0;
                }

                // tracking jump
                if((Input.GetButtonDown(GlobalStringVars.JUMP_BUTTON) || manualJump) && isGrounded && !attacking && !altAttacking)
                {
                    if(verticalInput < -0.51f)
                    {
                        JumpOff();
                    }
                    else
                    {
                        manualJump = false;
                        jumpButtonPressed = true;
                        animator.SetBool("MakeJump", true);
                    }
                }
                // tracking fire button - attacking
                if((Input.GetButtonDown(GlobalStringVars.FIRE_1) || manualAttack) && isGrounded && !altAttacking)
                {
                    manualAttack = false;
                    animator.SetBool("Attack", true);
                    attacking = true;
                    horizontalInput = 0;
                    rigidBody.velocity = Vector3.zero;
                    if (spriteRenderer.flipX)
                    {
                        playerAttack.LeftSwing();
                    }
                    else
                    {
                        playerAttack.RightSwing();
                    }
                }

                // tracking alternate fire button - alt attacking
                if ((Input.GetButtonDown(GlobalStringVars.FIRE_2) || manualAltAttack) && isGrounded && !attacking)
                {
                    manualAltAttack = false;
                    animator.SetBool("AltAttack", true);
                    altAttacking = true;
                    horizontalInput = 0;
                    rigidBody.velocity = Vector3.zero;
                    if (spriteRenderer.flipX)
                    {
                        playerAttack.LeftCast();
                    }
                    else
                    {
                        playerAttack.RightCast();
                    }
                }
            }
            else
            {
                horizontalInput = 0;
                verticalInput = 0;
            }
            manualAttack = false;
            manualAltAttack = false;
            manualJump = false;
        }

        private void LateUpdate()
        {
            // always reset these so animations work correctly
            animator.SetBool("MakeJump", false);
        }
        void FixedUpdate()
        {
            // overlap shpere
            overlapCircleTransform = new Vector2(transform.position.x + circleCollider2D.offset.x * 2 + overlapSpherePosition.x,
                                                    transform.position.y + circleCollider2D.offset.y * 2 + overlapSpherePosition.y);
            isGrounded = Physics2D.OverlapCircle(overlapCircleTransform, circleCollider2D.radius * 2 + overlapSphereExtension, actualMask);
            if(isGrounded)
            {
                animator.SetBool("StandingOnGround", true);
            }
            else
            {
                animator.SetBool("StandingOnGround", false);
            }
            // moving
            if(Mathf.Abs(horizontalInput) > deadZone && !attacking && !altAttacking)
            {
                playerMovement.Move(horizontalInput);
                if(horizontalInput > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
            // jumping
            if(jumpButtonPressed)
            {
                playerMovement.Jump();
                jumpButtonPressed = false;
            }
            // apply vertical speed
            animator.SetFloat("VerticalSpeed", rigidBody.velocity.y);

            // apply vertical input
            animator.SetFloat("VerticalInput", verticalInput);

            // manage jump offs
            if (jumpOffTimer == timeToJumpOff)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, groundJumpOffLayer, true);
                actualMask = jumpOffMask;
            }
            if (jumpOffTimer > 0)
            {
                jumpOffTimer -= Time.fixedDeltaTime;
            }
            else if (jumpOffTimer < 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, groundJumpOffLayer, false);
                jumpOffTimer = 0;
                actualMask = groundedMask;
            }
        }

        public void ResetAttack()
        {
            attacking = false;
            animator.SetBool("Attack", false);
        }

        public void ResetAltAttack()
        {
            altAttacking = false;
            animator.SetBool("AltAttack", false);
        }
        
        public void Attack()
        {
            manualAttack = true;
        }
        public void AltAttack()
        {
            manualAltAttack = true;
        }

        public void Jump()
        {
            manualJump = true;
        }

        public void JumpOff()
        {
            jumpOffTimer = timeToJumpOff;
        }

        public void OnAxisLeftDown(float moveValue)
        {
            moveValue = Mathf.Clamp(moveValue, 0f, 1f);
            manualHorizontalInputLeft = moveValue;
        }

        public void OnAxisLeftUp()
        {
            manualHorizontalInputLeft = 0f;
        }

        public void OnAxisRightDown(float moveValue)
        {
            moveValue = Mathf.Clamp(moveValue, 0f, 1f);
            manualHorizontalInputRight = moveValue;
        }

        public void OnAxisRighttUp()
        {
            manualHorizontalInputRight = 0f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            float radius = 0f;
            if(circleCollider2D != null)
            {
                overlapCircleTransform = new Vector2(
                                                    transform.position.x + circleCollider2D.offset.x * 2 + overlapSpherePosition.x,
                                                    transform.position.y + circleCollider2D.offset.y * 2 + overlapSpherePosition.y
                                                );
                radius = circleCollider2D.radius * 2 + overlapSphereExtension;
            }
            Gizmos.DrawWireSphere (
                overlapCircleTransform, radius);
        }
    }
}