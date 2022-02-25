using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs{
    public class EnemySimpleAI : MonoBehaviour
    {
        public enum EnemySimpleAIState
        {
            Patroling, AtWaipoint, Chasing, Attacking, AlertCoolDown, Dead, Stagger
        }
        [SerializeField] private Transform[] path = null;
        [SerializeField] private float waitAtWaipointTime = 3f;
        [SerializeField] private float minDistanceToWaipoint = 0.3f;
        [SerializeField] private float minDistanceToTarget = 1.5f;
        [SerializeField] private float alertCooldown = 3f;
        [SerializeField] private float patrolSpeed = 4f;
        [SerializeField] private float chasingSpeed = 8f;
        [SerializeField] private float attackTime = 2f;
        [SerializeField] private float staggerTime = 0.6f;
        [SerializeField] private PlayerAttack attacker = null;
        [SerializeField] private Transform healthBar = null;
        
        private EnemySimpleAIState state = EnemySimpleAIState.Patroling;
        private Transform activeDestination = null;
        private Transform activeTarget = null;
        private float waitAtWaipointTimer = 0f;
        private float distanceToTargetX = 0f;
        private float alertCooldownTimer = 0f;
        private float attackTimer = 0f;
        private float staggerTimer = 0f;
        private Animator animator = null;
        private Rigidbody2D rb;
        private int pathLenght;
        private int currentIndex = 0;
        private Collider2D coll = null;
        private GameManager gameManager = null;
        private void Start()
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            coll = transform.Find("DamageHitBox").GetComponent<Collider2D>();
            pathLenght = path.Length;
            if(pathLenght > 0)
            {
                activeDestination = path[0];
            }
        }

        private void Update()
        {
            if(state != EnemySimpleAIState.Dead)
            {
                // stagger
                if(staggerTimer > 0)
                {
                    staggerTimer -= Time.deltaTime;
                }
                // target in sight
                else if (activeTarget != null)
                {
                    distanceToTargetX = activeTarget.position.x - transform.position.x;
                    // attacking
                    if (Mathf.Abs(distanceToTargetX) < minDistanceToTarget)
                    {
                        state = EnemySimpleAIState.Attacking;
                        // make sure to turn to target
                        if(distanceToTargetX > 0)
                        {
                            FlipToRight(); // turn right
                        }
                        else
                        {
                            FlipToLeft(); // turn left
                        }
                    }
                    // chasing
                    else
                    {
                        state = EnemySimpleAIState.Chasing;
                    }
                }
                // alert cooldown
                else if (alertCooldownTimer > 0)
                {
                    alertCooldownTimer -= Time.deltaTime;
                    state = EnemySimpleAIState.AlertCoolDown;
                    activeDestination = path[currentIndex];
                    distanceToTargetX = activeDestination.position.x - transform.position.x;
                    // waipoint reached, set path to next destination
                    if (Mathf.Abs(distanceToTargetX) < minDistanceToWaipoint)
                    {
                        waitAtWaipointTimer = waitAtWaipointTime;
                        currentIndex = GetNextIndex(currentIndex);
                    }
                }
                // no targets - just routine
                else
                {
                    activeDestination = path[currentIndex];
                    distanceToTargetX = activeDestination.position.x - transform.position.x;
                    // off to next destination
                    if (waitAtWaipointTimer <= 0 && Mathf.Abs(distanceToTargetX) > minDistanceToWaipoint)
                    {
                        state = EnemySimpleAIState.Patroling;
                    }
                    // waiting
                    if (waitAtWaipointTimer > 0)
                    {
                        waitAtWaipointTimer -= Time.deltaTime;
                    }
                    // waipoint reached, set path to next destination
                    if (Mathf.Abs(distanceToTargetX) < minDistanceToWaipoint)
                    {
                        waitAtWaipointTimer = waitAtWaipointTime;
                        state = EnemySimpleAIState.AtWaipoint;
                        currentIndex = GetNextIndex(currentIndex);
                    }
                }
            }
            if(healthBar != null)
            {
                healthBar.transform.position = this.transform.position;
            }
        }

        private void FixedUpdate()
        {
            switch (state)
            {
                case EnemySimpleAIState.Patroling:
                    Move(patrolSpeed);
                    animator.SetBool("Attacking", false);
                    break;
                case EnemySimpleAIState.AtWaipoint:
                    animator.SetBool("Attacking", false);
                    break;
                case EnemySimpleAIState.Chasing:
                    animator.SetBool("Attacking", false);
                    Move(chasingSpeed);
                    break;
                case EnemySimpleAIState.Attacking:
                    animator.SetBool("Attacking", true);
                    if(attackTimer <= 0)
                    {
                        attacker.RightSwing();
                        attackTimer = attackTime;
                    }
                    break;
                case EnemySimpleAIState.AlertCoolDown:
                    animator.SetBool("Attacking", false);
                    Move(chasingSpeed);
                    break;
                default:
                    break;
            }
            animator.SetFloat("MoveSpeed", Mathf.Abs(rb.velocity.x));
            if(attackTimer > 0)
            {
                attackTimer -= Time.fixedDeltaTime;
            }
        }

        private void Move(float speed)
        {
            float direction = 0;
            if(distanceToTargetX > 0)
            {
                direction = 1;
                FlipToRight();
            }
            else
            {
                direction = -1;
                FlipToLeft();
            }
            rb.velocity = new Vector2(speed * direction , rb.velocity.y);
        }

        public void AttackTarget(Transform _transform)
        {
           activeTarget = _transform;
        }

        public void StopAttacking()
        {
            activeTarget = null;
            alertCooldownTimer = alertCooldown;

        }

        private int GetNextIndex(int currentIndex)
        {
            if(pathLenght - 1 <= currentIndex)
            {
                currentIndex = 0;
            }
            else currentIndex++;
            return currentIndex;
        }

        private void FlipToLeft()
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        private void FlipToRight()
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        public void OnDeath()
        {
            state = EnemySimpleAIState.Dead;
            coll.enabled = false;
            gameManager.enemySlayCounter++;
            Destroy(healthBar.gameObject);
        }

        public void OnTakeDamage()
        {
            state = EnemySimpleAIState.Stagger;
            animator.SetTrigger("TakeHit");
            staggerTimer = staggerTime;
        }
    }
}