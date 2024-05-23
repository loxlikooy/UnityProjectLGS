using UnityEngine;
using System.Collections.Generic;

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
        private float attackVampire = 2f;
        [SerializeField]
        private float attackBurn = 7f; // Burn damage per second
        [SerializeField]
        private float burnDuration = 3f; // Duration of burn effect

        private float _attackTimer; // Таймер для отслеживания кулдауна
        private float _timeSinceLastAttack = Mathf.Infinity; // Время с последней атаки

        private bool _activeVampiric;
        private bool _activateBurn;

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

            // Обновление времени с последней атаки
            _timeSinceLastAttack += Time.deltaTime;

            if (_componentGetter.PlayerInputHandler.OnAttack() && _attackTimer <= 0)
            {
                CheckAndExecuteAttack();
                _componentGetter.PlayerAnimator.SetAttackAnimation();
                _attackTimer = attackCooldown; // Сброс таймера кулдауна
                _timeSinceLastAttack = 0f; // Сброс времени с последней атаки
            }

            PlayerHUDManager.Instance.AttackCooldown(attackCooldown, Mathf.Clamp(_timeSinceLastAttack, 0, attackCooldown));
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
                    BurnByAttack(enemy);
                }
            }
        }

        private void HealByAttack()
        {
            if (!_activeVampiric)
            {
                return;
            }
            _componentGetter.HealthComponent.HealthRegen(attackVampire);
        }

        private void BurnByAttack(Enemy enemy)
        {
            if (!_activateBurn)
            {
                return;
            }
            enemy.ApplyDamageOverTime(attackBurn, burnDuration, 0.9f); // Apply burn effect
        }

        public void ExecuteAttack(IDamagable target)
        {
            target.TakeDamage(playerDamage);
        }

        public void IncreaseDamage(float amount)
        {
            playerDamage += playerDamage * (amount/100);
        }

        public void DecreaseAttackCooldown(float amount)
        {
            attackCooldown -= attackCooldown * 0.2f;
        }

        public void SaveAttackStats()
        {
            PlayerPrefs.SetFloat("PlayerDamage", playerDamage);
            PlayerPrefs.SetFloat("AttackCooldown", attackCooldown);
            PlayerPrefs.SetInt("Vampiric", _activeVampiric ? 1 : 0);
            PlayerPrefs.SetInt("Burn", _activateBurn ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void TurnOnBurn()
        {
            _activateBurn = true;
        }

        public void TurnOnVampiric()
        {
            _activeVampiric = true;
        }

        private void LoadAttackStats()
        {
            if (PlayerPrefs.HasKey("PlayerDamage"))
            {
                playerDamage = PlayerPrefs.GetFloat("PlayerDamage");
            }

            if (PlayerPrefs.HasKey("Vampiric"))
            {
                _activeVampiric = PlayerPrefs.GetInt("Vampiric") == 1;
            }

            if (PlayerPrefs.HasKey("AttackCooldown"))
            {
                attackCooldown = PlayerPrefs.GetFloat("AttackCooldown");
            }

            if (PlayerPrefs.HasKey("Burn"))
            {
                _activateBurn = PlayerPrefs.GetInt("Burn") == 1;
            }
        }

        // Новый метод для добавления врагов
        public void AddEnemy(Enemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
            }
        }
    }
}
