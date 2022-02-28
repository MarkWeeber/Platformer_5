using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Inputs{
    public class OpenExitPortalAction : Action
    {
        private Animator anim = null;
        private Collider2D coll = null;

        private void Start()
        {
            anim = GetComponent<Animator>();
            coll = GetComponent<Collider2D>();
            coll.enabled = false;
        }

        public override void Activate()
        {
            coll.enabled = true;
            anim.SetBool("OpenPortal", true);
            base.Activate();
        }

        public override void Deactivate()
        {
            coll.enabled = false;
            anim.SetBool("OpenPortal", false);
            base.Deactivate();
        }
    }
}