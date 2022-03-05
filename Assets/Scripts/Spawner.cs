using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab = null;
        private GameObject prefabInstance = null;
        void Start()
        {
            InvokeRepeating(nameof(CheckSpawn), 0.1f, 2);
        }

        private void CheckSpawn()
        {
            if(prefabInstance == null)
            {
                prefabInstance = Instantiate(prefab, transform.position, Quaternion.identity) ;
            }
        }
    }
}