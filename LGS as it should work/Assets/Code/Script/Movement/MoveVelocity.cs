using UnityEngine;

namespace Code.Script
{
    public class MoveVelocity : MonoBehaviour, IMoveVelocity
    {
        [SerializeField] private float moveSpeed = 50f;

        private Vector2 _velocityVector;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 vector)
        {
            _velocityVector = vector;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = (_velocityVector * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}