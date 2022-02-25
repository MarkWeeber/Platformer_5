using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    public class DamageDealerOnTouch : MonoBehaviour
    {
        public float DamageDeal = 1f;
        private Health health = null;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.transform.gameObject.tag == GlobalStringVars.DAMAGEABLE_TAG)
            {
                health = other.transform.parent.transform.gameObject.GetComponent<Health>();
                health.ModifyHealth(-DamageDeal);
                health.TakeDamage();
            }
        }
    }
}