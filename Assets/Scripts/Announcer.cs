using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs
{
    public class Announcer : MonoBehaviour
    {
        public string message = "";
        private GameManager gameManager = null;

        private void Start()
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
        }
        public void Announce()
        {
            gameManager.AnnounceMessage(message);
        }
    }
}