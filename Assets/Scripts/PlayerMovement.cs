using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Inputs{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private AnimationCurve moveCurve = null;
        [SerializeField] private float jumpForce = 5f;
        private Rigidbody2D rigidBody;
        private float moveForce;
        private void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Move(float direction)
        {
            moveForce = moveCurve.Evaluate(direction);
            rigidBody.velocity = new Vector2(moveForce , rigidBody.velocity.y);
        }

        public void Jump()
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x , jumpForce);
        }
    }
}