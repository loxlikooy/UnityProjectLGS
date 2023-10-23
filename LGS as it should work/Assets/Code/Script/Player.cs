using UnityEngine;

namespace Code.Script
{
    public class Player : MonoBehaviour, IDamagable, IAttackable
    {
        private Health _playerHealth;
        private PlayerMovement _playerMovement;
        private PlayerAttack _playerAttack;
        private Dash _playerDash;

        public Health HealthComponent => _playerHealth;
        public PlayerMovement Movement => _playerMovement;
        public PlayerAttack AttackComponent => _playerAttack;
        public Dash DashComponent => _playerDash;

        private void Awake()
        {
            _playerHealth = GetComponent<Health>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();
            _playerDash = GetComponent<Dash>();
        }

        public void TakeDamage(float damageAmount)
        {
            _playerHealth.TakeDamage(damageAmount);
        }

        public void Attack(IDamagable target)
        {
            _playerAttack.ExecuteAttack(target);
        }

    }
}