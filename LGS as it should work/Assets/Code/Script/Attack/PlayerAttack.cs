using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Code.Script
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private float playerDamage = 10f; // Default damage value
        [SerializeField]
        private float attackRange = 1f;
        [SerializeField]
        private float attackCooldown = 2f; // Длительность кулдауна атаки в секундах
        [SerializeField] 
        private float attackVampire = 10f;

        private float _attackTimer; // Таймер для отслеживания кулдауна
        private bool _active;

        private ComponentGetter _componentGetter;
        private List<Enemy> _enemies = new List<Enemy>();

        private void Start()
        {
            _componentGetter = GetComponent<ComponentGetter>();
            _enemies.AddRange(FindObjectsOfType<Enemy>());
            _attackTimer = 0f; // Инициализация таймера кулдауна
            LoadAttackStats();
        }

        private void Update()
        {
            if (_componentGetter.PlayerAnimator.IsAttacking()) return;
            // Обновление таймера кулдауна
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            }

            if (_componentGetter.PlayerInputHandler.OnAttack() && _attackTimer <= 0)
            {
                CheckAndExecuteAttack();
                _componentGetter.PlayerAnimator.SetAttackAnimation();
                _attackTimer = attackCooldown; // Сброс таймера кулдауна
            }
        }

        private void CheckAndExecuteAttack()
        {
            Vector2 lastMoveDirection = _componentGetter.PlayerMovement.LastMoveDirection.normalized;
            _enemies.RemoveAll(enemy => enemy == null);
            foreach (Enemy enemy in _enemies)
            {
                Vector2 toEnemy = enemy.transform.position - transform.position;
                float distanceToEnemy = toEnemy.magnitude;
                float angle = Vector2.Angle(lastMoveDirection, toEnemy / distanceToEnemy);
                if (distanceToEnemy <= attackRange && angle <= 45.0f)
                {
                     ExecuteAttack(enemy);
                     HealByAttack();
                     break; 
                }
            }
        }

        private void HealByAttack()
        {
            if (!_active)
            {
                return;
            }
            _componentGetter.HealthComponent.HealthRegen(attackVampire);
        }

        public void ExecuteAttack(IDamagable target)
        {
            target.TakeDamage(playerDamage);
        }

        public void IncreaseDamage(float amount)
        {
            playerDamage += playerDamage * 0.3f;
        }

        public void DecreaseAttackCooldown(float amount)
        {
            attackCooldown = attackCooldown * 0.8f;
           
        }

        public void SaveAttackStats()
        {
            PlayerPrefs.SetFloat("PlayerDamage", playerDamage);
            PlayerPrefs.SetFloat("AttackCooldown", attackCooldown);
            PlayerPrefs.SetInt("Vampiric", _active ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void TurnOnVampiric()
        {
            _active = true;
        }
        

        private void LoadAttackStats()
        {
            if (PlayerPrefs.HasKey("PlayerDamage"))
            {
                playerDamage = PlayerPrefs.GetFloat("PlayerDamage");
            }
            
            if (PlayerPrefs.HasKey("Vampiric"))
            {
                _active = PlayerPrefs.GetInt("Vampiric") == 1;
            }

            if (PlayerPrefs.HasKey("AttackCooldown"))
            {
                attackCooldown = PlayerPrefs.GetFloat("AttackCooldown");
            }
        }
    }
}
