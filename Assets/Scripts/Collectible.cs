using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Inputs
{
    public class Collectible : MonoBehaviour
    {
        public enum CollectibleType
        {
            GoldCoin, BlueGem, Secret
        }

        private Announcer announcer = null;
        public LayerMask targetMask = 0;
        public CollectibleType collectibleType;
        private GameManager gameManager = null;
        private bool Activated = false;

        void Start()
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            announcer = GetComponent<Announcer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0 && !Activated)
            {
                Activated = true;
                switch (collectibleType)
                {
                    
                    case CollectibleType.GoldCoin:
                        gameManager.GoldCointCounter++;
                        break;
                    case CollectibleType.BlueGem:
                        gameManager.BlueGemCounter++;
                        break;
                    case CollectibleType.Secret:
                        gameManager.secretsFoundCounter++;
                        announcer.Announce();
                        break;
                    default: break;
                }
                Destroy(gameObject);
            }
        }
    }
}