using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Inputs
{
    public class DeathZone : MonoBehaviour
    {
        private Health health = null;
        void OnTriggerEnter2D(Collider2D other)
        {
            health = other.GetComponent<Health>();
            if(health != null)
            {
                health.OnDeath();
            }
        }
    }
}