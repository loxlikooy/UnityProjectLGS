using UnityEngine;

namespace Code.Script
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private float playerDamage = 10f;
        private InputHandler _inputHandler;
        private PlayerMovement _playerMovement;
        private Vector2 _toEnemy;
        private PlayerAnimator _playerAnimator; 

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _inputHandler = GetComponent<InputHandler>();
            _playerAnimator = GetComponent<PlayerAnimator>(); 
        }

        private void Update()
        {
            if (_playerAnimator.IsAttacking()) 
                return;
            if (_inputHandler.OnAttack())
            {
                Vector2 LastMoveDirection = _playerMovement.LastMoveDirection;
                EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
                
                foreach (EnemyBase enemy in enemies)
                {
                    _toEnemy = (enemy.transform.position - transform.position).normalized;
                    float angle = Vector2.Angle(LastMoveDirection, _toEnemy);
                    if (angle <= 90.0f)
                    {
                        ExecuteAttack(enemy);
                    }
                }
                _playerAnimator.SetAttackAnimation();
            }
        }

        public void ExecuteAttack(IDamageable target)
        {
            target.TakeDamage(playerDamage);
        }
    }
}