using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs{
    public class EnemyDetectionTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask = 0;
        [SerializeField] private EnemySimpleAI enemySimpleAI = null;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                enemySimpleAI.AttackTarget(other.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                enemySimpleAI.StopAttacking();
            }
        }
    }
}