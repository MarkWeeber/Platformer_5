using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    public class LevelCompleteZone : MonoBehaviour
    {
        private GameManager gameManager = null;
        
        void Start()
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(GlobalStringVars.PLAYER_LAYER))
            {
                gameManager.LevelComplete();
            }
        }
    }
}
