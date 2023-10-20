using Code.Script;
using UnityEngine;

namespace Code.Script
{
    public class MoveVelocity : MonoBehaviour, IMoveVelocity
    {
        [SerializeField] private float moveSpeed = 50f;

        private Vector2 velocityVector;
        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 vector)
        {
            this.velocityVector = vector;
        }

        private void FixedUpdate()
        {
            rigidbody2D.velocity = velocityVector * moveSpeed * Time.fixedDeltaTime;
        }
    }
}