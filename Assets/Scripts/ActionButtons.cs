using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Platformer.Inputs{       
    public class ActionButtons : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private CanvasRenderer joystickThumbStick = null;
        //[SerializeField] private EventTrigger [] eventTriggers = null;
        public CanvasRenderer cv = null;
        private void Start()
        {
            
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            cv.SetColor(Color.red);
            if (EventSystem.current.IsPointerOverGameObject())
            {
                
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);
            }
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                //cv.SetColor(Color.red);
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
        }
    }
}