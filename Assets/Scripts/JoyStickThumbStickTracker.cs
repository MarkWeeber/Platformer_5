using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Platformer.Inputs{       
    public class JoyStickThumbStickTracker : MonoBehaviour
    {
        [SerializeField] private CanvasRenderer joystickThumbStick = null;
        private Vector3 canvasOriginalPosition;
        private void Start()
        {
            canvasOriginalPosition = joystickThumbStick.gameObject.transform.position;
        }
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    
                    PointerEventData pointer = new PointerEventData(EventSystem.current);
                    pointer.position = Input.mousePosition;
                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointer, raycastResults);
                    if(raycastResults.Count > 0)
                    {
                        if(raycastResults[0].gameObject.CompareTag(GlobalStringVars.JOYSTICK_TAG))
                        {
                            joystickThumbStick.transform.position = pointer.position;
                        }
                    }
                }            
            }
            else
            {
                joystickThumbStick.transform.position = canvasOriginalPosition;
            }
        }
    }
}