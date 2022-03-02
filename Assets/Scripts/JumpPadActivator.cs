using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Inputs
{
    public class JumpPadActivator : MonoBehaviour
    {
        [SerializeField] private float JumpForceImpulse = 30f;
        [SerializeField] private LayerMask targetMask = 0;
        [SerializeField] private bool addFallingForce = false;
        [SerializeField] private float fallForceMultiplier = 2f;
        private Animator anim;
        private bool Activated = false;
        private Rigidbody2D rb = null;
        private float fallingForce = 0f;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0 && !Activated)
            {
                anim.SetTrigger("Jump");
                rb = other.GetComponent<Rigidbody2D>();
                if(addFallingForce)
                {
                    fallingForce = Mathf.Abs(rb.velocity.y) - JumpForceImpulse;
                    if(fallingForce < 0f)
                    {
                        fallingForce = 0f;
                    }
                }
                else
                {
                    fallingForce = 0f;
                }
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * (JumpForceImpulse + fallingForce * fallForceMultiplier), ForceMode2D.Impulse);
                Activated = true;
                Invoke(nameof(ResetActivation), 1f);
            }
        }

        private void ResetActivation()
        {
            Activated = false;
        }
    }
}