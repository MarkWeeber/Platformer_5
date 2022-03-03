using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Inputs{

    public class ExplosionEffect : MonoBehaviour
    {
        [SerializeField] private Transform prefabTransform = null;
        [SerializeField] private LayerMask hitMask;
        [SerializeField] private float explosionAreaDamage = 15f;
        [SerializeField] private float explosionAreaRadius = 2f;
        [SerializeField] private float explosionDelayTime = 0.2f;
        private Transform prefabClone = null;
        private CircleCollider2D coll = null;
        private Collider2D[] affectedByExposion = null;
        private Health health = null;
        public void Explode()
        {
            prefabClone = Instantiate(prefabTransform, this.transform.position, Quaternion.identity);
            coll = prefabClone.GetComponent<CircleCollider2D>();
            coll.radius = explosionAreaRadius;
            affectedByExposion = Physics2D.OverlapCircleAll(this.transform.position, explosionAreaRadius);
            foreach (Collider2D item in affectedByExposion)
            {
                if(item.transform.CompareTag(GlobalStringVars.DAMAGEABLE_TAG))
                {
                    health = item.transform.parent.transform.gameObject.GetComponent<Health>();
                    if(health != null)
                    {
                        health.ModifyHealth(-explosionAreaDamage);
                        health.TakeDamage();
                    }
                }
            }
            Destroy(this.transform.gameObject);
            Destroy(prefabClone.gameObject, 0.5f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((hitMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                //Destroy(other.transform.gameObject);
                Invoke(nameof(Explode), explosionDelayTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((hitMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                //Destroy(other.transform.gameObject);
                Invoke(nameof(Explode), explosionDelayTime);
            }
        }
    }
}