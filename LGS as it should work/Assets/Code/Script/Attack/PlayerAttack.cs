using UnityEngine;
using System.Collections.Generic;

namespace Code.Script
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO playerStatsSO;
        
        // Attack parameters
        private float _playerDamage;
        private float _attackRange;
        private float _attackCooldown;
        private float _attackVampire;
        private float _attackBurn;
        private float _burnDuration;

        // Cooldown and timers
        private float _attackTimer;
        private float _timeSinceLastAttack = Mathf.Infinity;

        // Buffs
        private bool _activeVampiric;
        private bool _activateBurn;

        private ComponentGetter _componentGetter;
        private List<Enemy> _enemies = new List<Enemy>();

        private void Start()
        {
            _componentGetter = GetComponent<ComponentGetter>();
            _enemies.AddRange(FindObjectsOfType<Enemy>());
            _attackTimer = 0f;

            LoadPermanentStats();
            LoadTemporaryStats();
        }

        private void LoadPermanentStats()
        {
            _playerDamage = playerStatsSO.playerDamage;
            _attackRange = playerStatsSO.attackRange;
            _attackCooldown = playerStatsSO.attackCooldown;
            _attackVampire = playerStatsSO.attackVampire;
            _attackBurn = playerStatsSO.attackBurn;
            _burnDuration = playerStatsSO.burnDuration;
            _activeVampiric = playerStatsSO.activeVampiric;
            _activateBurn = playerStatsSO.activateBurn;
        }

        private void LoadTemporaryStats()
        {
            if (PlayerPrefs.HasKey("TemporaryPlayerDamage"))
            {
                _playerDamage = PlayerPrefs.GetFloat("TemporaryPlayerDamage");
            }
            if (PlayerPrefs.HasKey("TemporaryAttackCooldown"))
            {
                _attackCooldown = PlayerPrefs.GetFloat("TemporaryAttackCooldown");
            }
            if (PlayerPrefs.HasKey("TemporaryAttackVampire"))
            {
                _attackVampire = PlayerPrefs.GetFloat("TemporaryAttackVampire");
            }
            if (PlayerPrefs.HasKey("TemporaryAttackBurn"))
            {
                _attackBurn = PlayerPrefs.GetFloat("TemporaryAttackBurn");
            }
            if (PlayerPrefs.HasKey("TemporaryVampiric"))
            {
                _activeVampiric = PlayerPrefs.GetInt("TemporaryVampiric") == 1;
            }
            if (PlayerPrefs.HasKey("TemporaryBurn"))
            {
                _activateBurn = PlayerPrefs.GetInt("TemporaryBurn") == 1;
            }
        }

        private void Update()
        {
            if (_componentGetter.PlayerAnimator.IsAttacking()) return;

            UpdateAttackTimer();
            UpdateLastAttackTime();

            if (_componentGetter.PlayerInputHandler.OnAttack() && _attackTimer <= 0)
            {
                PerformAttack();
            }

            UpdateHUDCooldown();
        }

        private void UpdateAttackTimer()
        {
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            }
        }

        private void UpdateLastAttackTime()
        {
            _timeSinceLastAttack += Time.deltaTime;
        }

        private void PerformAttack()
        {
            CheckAndExecuteAttack();
            _componentGetter.PlayerAnimator.SetAttackAnimation();
            _attackTimer = _attackCooldown;
            _timeSinceLastAttack = 0f;
        }

        private void UpdateHUDCooldown()
        {
            PlayerHUDManager.Instance.AttackCooldown(_attackCooldown, Mathf.Clamp(_timeSinceLastAttack, 0, _attackCooldown));
        }

        private void CheckAndExecuteAttack()
        {
            Vector2 lastMoveDirection = _componentGetter.PlayerMovement.LastMoveDirection.normalized;
            _enemies.RemoveAll(enemy => enemy == null);
            foreach (Enemy enemy in _enemies)
            {
                if (IsEnemyInRangeAndAngle(enemy, lastMoveDirection))
                {
                    ExecuteAttack(enemy);
                    HealByAttack();
                    BurnByAttack(enemy);
                }
            }
        }

        private bool IsEnemyInRangeAndAngle(Enemy enemy, Vector2 lastMoveDirection)
        {
            Vector2 toEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = toEnemy.magnitude;
            float angle = Vector2.Angle(lastMoveDirection, toEnemy / distanceToEnemy);
            return distanceToEnemy <= _attackRange && angle <= 45.0f;
        }

        private void HealByAttack()
        {
            if (_activeVampiric)
            {
                _componentGetter.HealthComponent.HealthRegen(_attackVampire);
            }
        }

        private void BurnByAttack(Enemy enemy)
        {
            if (_activateBurn)
            {
                enemy.ApplyDamageOverTime(_attackBurn, _burnDuration, 0.9f);
            }
        }

        public void ExecuteAttack(IDamagable target)
        {
            target.TakeDamage(_playerDamage);
        }

        public void IncreaseDamage(float amount)
        {
            _playerDamage += _playerDamage * (amount / 100);
            SaveTemporaryStats();
        }

        public void DecreaseAttackCooldown(float amount)
        {
            _attackCooldown -= _attackCooldown * (amount / 100);
            SaveTemporaryStats();
        }

        public void SaveTemporaryStats()
        {
            PlayerPrefs.SetFloat("TemporaryPlayerDamage", _playerDamage);
            PlayerPrefs.SetFloat("TemporaryAttackCooldown", _attackCooldown);
            PlayerPrefs.SetFloat("TemporaryAttackVampire", _attackVampire);
            PlayerPrefs.SetFloat("TemporaryAttackBurn", _attackBurn);
            PlayerPrefs.SetInt("TemporaryVampiric", _activeVampiric ? 1 : 0);
            PlayerPrefs.SetInt("TemporaryBurn", _activateBurn ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void TurnOnBurn()
        {
            if (_activateBurn)
            {
                _attackBurn += 1;
            }
            _activateBurn = true;
            SaveTemporaryStats();
        }

        public void TurnOnVampiric()
        {
            if (_activeVampiric)
            {
                _attackVampire += 2;
            }
            _activeVampiric = true;
            SaveTemporaryStats();
        }

        public void AddEnemy(Enemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
            }
        }
    }
}
