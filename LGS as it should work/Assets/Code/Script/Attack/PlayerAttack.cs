using UnityEngine;

namespace Code.Script
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private float playerDamage = 10f; // Default damage value
        private InputHandler _inputHandler;
        private PlayerMovement _playerMovement;
        private Vector2 _toEnemy;
        private PlayerAnimator _playerAnimator;  // Изменил тип с Animator на PlayerAnimator

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _inputHandler = GetComponent<InputHandler>();
            _playerAnimator = GetComponent<PlayerAnimator>();  // Убрал создание локальной переменной
        }

        private void Update()
        {
            if (_playerAnimator.IsAttacking()) 
                return;
            // Здесь можно добавить логику определения цели, если нужно.
            if (_inputHandler.OnAttack())
            {
                Vector2 LastMoveDirection = _playerMovement.LastMoveDirection;
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                
                foreach (Enemy enemy in enemies)
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

        public void ExecuteAttack(IDamagable target)
        {
            target.TakeDamage(playerDamage);
        }
    }
}