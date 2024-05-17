using UnityEngine;

namespace Code.Script
{
    public class Shekotalka : Enemy
    {
        [Header("Magnet Settings")]
        [SerializeField] private float pullStrength = 2f;
        [SerializeField] private float pullRadius = 3f;
        [SerializeField] private float pullCooldown = 2f;
        private float _timeSinceLastPull;
        private bool _isPulling;
        private Rigidbody2D _playerRigidBody;

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _playerRigidBody = Player.GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            base.Update();

            if (!_isPulling) return;
                PullPlayer();
        }

        protected override void Chase()
        {
            base.Chase(); // Call base class chase logic

            if (Vector2.Distance(Player.position, transform.position) <= pullRadius)
            {
                if (_timeSinceLastPull >= pullCooldown)
                {
                    _isPulling = true;
                    Invoke("StopPulling", 1.26f);
                    _timeSinceLastPull = 0f;
                }
            }
            _timeSinceLastPull += Time.deltaTime;
        }

        private void PullPlayer()
        {
            Vector2 pullDirection = (transform.position - Player.position).normalized;
            float distance = Vector2.Distance(Player.position, transform.position);
            float pullForce = Mathf.Lerp(0, pullStrength, 1 - (distance / pullRadius));

            // Apply force to the player's Rigidbody
            _playerRigidBody.AddForce(pullDirection * pullForce, ForceMode2D.Force);
        }

        private void StopPulling()
        {
            _isPulling = false;
        }
    }
}