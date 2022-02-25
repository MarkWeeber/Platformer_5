using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerMessenger : MonoBehaviour
    {
        [SerializeField] private ChainLineSwitcher chainLineSwitcher = null;
        [SerializeField] private float switchValue = 1f;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.transform.gameObject.layer == LayerMask.NameToLayer(GlobalStringVars.PLAYER_LAYER))
            {
                chainLineSwitcher.SwitchSliderMotor(switchValue);
            }
        }
    }
}