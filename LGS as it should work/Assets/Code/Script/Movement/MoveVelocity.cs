using UnityEngine;

namespace Code.Script
{
    public class MoveVelocity : MonoBehaviour, IMoveVelocity
    {
        [SerializeField] private float moveSpeed = 50f;

        private Vector2 _velocityVector;
        private ComponentGetter _componentGetter;

        private void Awake()
        {
            _componentGetter = GetComponent<ComponentGetter>();
        }

        public void SetVelocity(Vector2 vector)
        {
            _velocityVector = vector;
        }

        private void FixedUpdate()
        {
            _componentGetter.PlayerRGB.velocity = (_velocityVector * (moveSpeed * Time.fixedDeltaTime));
        }

        public void IncreaseSpeed()
        {
            moveSpeed = moveSpeed + 5f;
        }
    }
}