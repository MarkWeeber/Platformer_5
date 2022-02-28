using UnityEngine;

namespace Platformer.Inputs{
    public class StepOnTrigger : MonoBehaviour
    {
        [SerializeField] private Color normalColor = Color.gray;
        [SerializeField] private Color steppedOnColor = Color.green;
        [SerializeField] private SpriteRenderer boxRenderer = null;
        [SerializeField] private LayerMask targetMask = 0;
        [SerializeField] private Action [] actions = null;
        void Start()
        {
            boxRenderer.color = normalColor;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0 )
            {
                // trigger on
                boxRenderer.color = steppedOnColor;
                if(actions != null)
                {
                    foreach (Action item in actions)
                    {
                        item.Activate();
                    }
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0 )
            {
                // trigger off
                boxRenderer.color = normalColor;
                if(actions != null)
                {
                    foreach (Action item in actions)
                    {
                        item.Deactivate();
                    }
                }
            }
        }
    }
}