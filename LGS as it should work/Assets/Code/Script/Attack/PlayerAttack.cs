using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Code.Script
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private float playerDamage = 10f; // Default damage value
        [FormerlySerializedAs("desiredAttackRange")] [SerializeField]
        private float attackRange = 0.1f;

        
        private ComponentGetter _componentGetter;

        private List<Enemy> _enemies = new List<Enemy>();

        private void Start()
        {
            _componentGetter = GetComponent<ComponentGetter>();
            // Initialize enemies list (assuming enemies are spawned at start)
            _enemies.AddRange(FindObjectsOfType<Enemy>());
        }

        private void Update()
        {
            if (_componentGetter.PlayerAnimator.IsAttacking()) return;

            if (_componentGetter.PlayerInputHandler.OnAttack())
            {
                CheckAndExecuteAttack();
                _componentGetter.PlayerAnimator.SetAttackAnimation();
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
    }
}