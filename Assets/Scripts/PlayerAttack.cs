using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    public class PlayerAttack : MonoBehaviour
    {
        private enum AutoAttackMode
        {
            rightCast = 0,
            leftCast = 1
        }

        [SerializeField] private float fireBallCastSpeed = 3f;
        [SerializeField] private float damageDeal = 30f;
        [SerializeField] private Transform rightCast = null;
        [SerializeField] private Transform leftCast = null;
        [SerializeField] private GameObject fireballPrefab = null;
        public bool autoAttack = false;
        [SerializeField] private AutoAttackMode autoAttackMode = AutoAttackMode.leftCast;
        [SerializeField] private float autoAttackTimer = 5f;
        [SerializeField] private DamageDealerOnTouch rightDamageDealerOnTouch = null;
        [SerializeField] private DamageDealerOnTouch lefttDamageDealerOnTouch = null;
        [SerializeField] private Vector2 rightCastDirection = Vector2.right;
        [SerializeField] private Vector2 leftCastDirection = Vector2.left;
        private FireBall fireBall = null;
        private float timer = 0f;
        private bool rightDamageDealerEnabled = false;
        private bool leftDamageDealerEnabled = false;
        private GameObject prefabInstance = null;
        private Rigidbody2D rb = null;

        private void Start()
        {
            if(autoAttack)
            {
                timer = autoAttackTimer;
            }
            rightDamageDealerOnTouch.DamageDeal = damageDeal;
            lefttDamageDealerOnTouch.DamageDeal = damageDeal;
            rightDamageDealerOnTouch.gameObject.SetActive(false);
            lefttDamageDealerOnTouch.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            // auto attacking
            if(autoAttack)
            {
                if (timer <= 0)
                {
                    // attack
                    switch (autoAttackMode)
                    {
                        case AutoAttackMode.rightCast:
                            prefabInstance = Instantiate(fireballPrefab, rightCast.position, Quaternion.identity);
                            fireBall = prefabInstance.GetComponent<FireBall>();
                            fireBall.damage = damageDeal;
                            rb = prefabInstance.GetComponent<Rigidbody2D>();
                            rb.velocity = rightCastDirection * fireBallCastSpeed;
                            break;
                        case AutoAttackMode.leftCast:
                            prefabInstance = Instantiate(fireballPrefab, leftCast.position, Quaternion.identity);
                            fireBall = prefabInstance.GetComponent<FireBall>();
                            fireBall.damage = damageDeal;
                            prefabInstance.GetComponent<SpriteRenderer>().flipX = true;
                            rb = prefabInstance.GetComponent<Rigidbody2D>();
                            rb.velocity = leftCastDirection * fireBallCastSpeed;
                            break;
                        default:
                            break;
                    }
                    timer = autoAttackTimer;
                }
                else
                {
                    timer -= Time.fixedDeltaTime;
                }
            }

            // swinging left
            if(!leftDamageDealerEnabled && lefttDamageDealerOnTouch.gameObject.activeSelf)
            {
                lefttDamageDealerOnTouch.gameObject.SetActive(false);
            }
            if(leftDamageDealerEnabled)
            {
                lefttDamageDealerOnTouch.gameObject.SetActive(true);
                leftDamageDealerEnabled = false;
            }

            // swinging right
            if (!rightDamageDealerEnabled && rightDamageDealerOnTouch.gameObject.activeSelf)
            {
                rightDamageDealerOnTouch.gameObject.SetActive(false);
            }
            if (rightDamageDealerEnabled)
            {
                rightDamageDealerOnTouch.gameObject.SetActive(true);
                rightDamageDealerEnabled = false;
            }

        }

        public void LeftCast()
        {
            prefabInstance = Instantiate(fireballPrefab, leftCast.position, Quaternion.identity);
            fireBall = prefabInstance.GetComponent<FireBall>();
            fireBall.damage = damageDeal;
            prefabInstance.GetComponent<SpriteRenderer>().flipX = true;
            rb = prefabInstance.GetComponent<Rigidbody2D>();
            rb.velocity = leftCastDirection * fireBallCastSpeed;
        }

        public void RightCast()
        {
            prefabInstance = Instantiate(fireballPrefab, rightCast.position, Quaternion.identity);
            fireBall = prefabInstance.GetComponent<FireBall>();
            fireBall.damage = damageDeal;
            rb = prefabInstance.GetComponent<Rigidbody2D>();
            rb.velocity = rightCastDirection * fireBallCastSpeed;
        }

        public void LeftSwing()
        {
            leftDamageDealerEnabled = true;
        }

        public void RightSwing()
        {
            rightDamageDealerEnabled = true;
        }
    }
}