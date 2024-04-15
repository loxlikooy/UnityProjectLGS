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

        private float attackTimer; // Таймер для отслеживания кулдауна

        private ComponentGetter _componentGetter;
        private List<Enemy> _enemies = new List<Enemy>();

        private void Start()
        {
            _componentGetter = GetComponent<ComponentGetter>();
            _enemies.AddRange(FindObjectsOfType<Enemy>());
            attackTimer = 0f; // Инициализация таймера кулдауна
            LoadAttackStats();
        }

        private void Update()
        {
            if (_componentGetter.PlayerAnimator.IsAttacking()) return;
            // Обновление таймера кулдауна
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            if (_componentGetter.PlayerInputHandler.OnAttack() && attackTimer <= 0)
            {
                CheckAndExecuteAttack();
                _componentGetter.PlayerAnimator.SetAttackAnimation();
                attackTimer = attackCooldown; // Сброс таймера кулдауна
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
                     break; 
                }
            }
        }

        public void ExecuteAttack(IDamagable target)
        {
            target.TakeDamage(playerDamage);
        }

        public void IncreaseDamage()
        {
            playerDamage += playerDamage * 0.3f;
           
        }

        public void DecreaseAttackCooldown()
        {
            attackCooldown = attackCooldown * 0.8f;
           
        }

        public void SaveAttackStats()
        {
            PlayerPrefs.SetFloat("PlayerDamage", playerDamage);
            PlayerPrefs.SetFloat("AttackCooldown", attackCooldown);
            PlayerPrefs.Save();
        }

        private void LoadAttackStats()
        {
            if (PlayerPrefs.HasKey("PlayerDamage"))
            {
                playerDamage = PlayerPrefs.GetFloat("PlayerDamage");
            }

            if (PlayerPrefs.HasKey("AttackCooldown"))
            {
                attackCooldown = PlayerPrefs.GetFloat("AttackCooldown");
            }
        }
    }
}
