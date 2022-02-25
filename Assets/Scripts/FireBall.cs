using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class FireBall : MonoBehaviour
    {
        public LayerMask hitableLayerMask = 0;
        public float damage = 10f;
        public float destroyTime = 0.6f;
        private Animator animator = null;
        private Health health = null;
        private Rigidbody2D rb = null;
        private Collider2D coll2D = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            coll2D = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.transform.tag == GlobalStringVars.DAMAGEABLE_TAG)
            {
                health = collider.transform.parent.transform.gameObject.GetComponent<Health>();
                if(health != null)
                {
                    health.ModifyHealth(-damage);
                    health.TakeDamage();
                }
            }
            if((hitableLayerMask.value & (1 << collider.transform.gameObject.layer)) > 0)
            {
                animator.SetTrigger("Hit");
                Destroy(this.gameObject, destroyTime);
                rb.velocity = Vector2.zero;
                coll2D.enabled = false;
            }
        }
    }
}