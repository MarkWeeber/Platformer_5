using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadActivator : MonoBehaviour
{
    [SerializeField] private float JumpForceImpulse = 30f;
    [SerializeField] private LayerMask targetMask = 0;
    private Animator anim;
    private bool Activated = false;
    private Rigidbody2D rb = null;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0 && !Activated)
        {
            anim.SetTrigger("Jump");
            rb = other.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * JumpForceImpulse, ForceMode2D.Impulse);
            Activated = true;
            Invoke(nameof(ResetActivation), 1f);
        }
    }

    private void ResetActivation()
    {
        Activated = false;
    }
}
