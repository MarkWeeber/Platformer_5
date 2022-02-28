using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace Platformer.Inputs
{
    [RequireComponent(typeof(Collider2D))]
    public class CutsceneDirector : MonoBehaviour
    {
        [SerializeField] private Text announceText = null;
        [SerializeField] private CanvasRenderer announcerPanel = null;
        [SerializeField] private CanvasRenderer letterBoxPanel = null;
        [SerializeField] private CanvasRenderer inGamePanel = null;
        [SerializeField] private PlayerInput playerInput = null;
        [SerializeField] private CutSceneWaypoint [] waypoints = null;
        [SerializeField] private CinemachineBrain cBrain = null;
        [SerializeField] private LayerMask targetMask = 0;
        [SerializeField] private CinemachineVirtualCamera initialCam = null;
        private CinemachineVirtualCamera currentCam = null;
        private float travelTime = 0f;
        private int currentIndex = 0;
        private int waypointsLength = 0;
        private int initialCamPriority = 0;
        private Collider2D coll = null;
        private bool Activated = false;
        private ParallaxEffect parallaxEffect = null;

        void Start()
        {
            parallaxEffect = GameObject.FindObjectOfType<ParallaxEffect>();
            coll = GetComponent<Collider2D>();
            travelTime = cBrain.m_DefaultBlend.m_Time;
            waypointsLength = waypoints.Length;
            initialCamPriority = initialCam.Priority;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if((targetMask.value & (1 << other.transform.gameObject.layer)) > 0 && !Activated)
            {
                Activated = true;
                StartCutscene();
            }
        }

        private void StartCutscene()
        {
            // initiate
            if(waypointsLength > 0 && currentIndex == 0)
            {
                playerInput.Activated = false;
                letterBoxPanel.gameObject.SetActive(true);
                inGamePanel.gameObject.SetActive(false);
                coll.enabled = false;
                initialCam.Priority = 0;
                currentCam = waypoints[currentIndex].vcam;
                currentCam.Priority = 1;
                if(parallaxEffect != null)
                {
                    parallaxEffect.useFixedUpdate = false;
                }
                Invoke(nameof(ActivateNextWayPoint), travelTime);
            }
            // initiate failed
            else
            {
                Destroy(this);
            }
        }

        private void TravelToNextWaypoint()
        {
            // still waypoints are left
            if(currentIndex < waypointsLength - 1)
            {
                announcerPanel.gameObject.SetActive(false);
                announceText.text = "";
                currentCam.Priority = 0;
                currentIndex++;
                currentCam = waypoints[currentIndex].vcam;
                currentCam.Priority = 1;;
                Invoke(nameof(ActivateNextWayPoint), travelTime);
            }
            // last waypoint
            else
            {
                announcerPanel.gameObject.SetActive(false);
                announceText.text = "";
                initialCam.Priority = initialCamPriority;
                currentCam.Priority = 0;
                letterBoxPanel.gameObject.SetActive(false);
                inGamePanel.gameObject.SetActive(true);
                playerInput.Activated = true;
                if(parallaxEffect != null)
                {
                    parallaxEffect.useFixedUpdate = true;
                }
                Destroy(this);
            }
        }

        private void ActivateNextWayPoint()
        {
            announcerPanel.gameObject.SetActive(true);
            announceText.text = waypoints[currentIndex].text;
            Invoke(nameof(TravelToNextWaypoint), waypoints[currentIndex].timer);
        }
    }
}