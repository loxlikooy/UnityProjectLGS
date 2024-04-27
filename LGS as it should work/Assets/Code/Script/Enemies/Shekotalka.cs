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
        private Rigidbody2D _playerRigidBody;
        
        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _playerRigidBody = _player.GetComponent<Rigidbody2D>();

        }
        
        protected override void Chase()
        {
            base.Chase(); // Вызов основной логики погони из базового класса

            if (Vector2.Distance(_player.position, transform.position) <= pullRadius)
            {
                if (_timeSinceLastPull >= pullCooldown)
                {
                    PullPlayer();
                    _timeSinceLastPull = 0;
                }
            }
            _timeSinceLastPull += Time.deltaTime;
        }

        private void PullPlayer()
        {
            Vector2 pullDirection = (transform.position - _player.position).normalized;
            float distance = Vector2.Distance(_player.position, transform.position);
            float pullForce = Mathf.Lerp(0, pullStrength, 1 - (distance / pullRadius));

           _playerRigidBody.AddForce(pullDirection * pullForce, ForceMode2D.Force);
        }

    }
}