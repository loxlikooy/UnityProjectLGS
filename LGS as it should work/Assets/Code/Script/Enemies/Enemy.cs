using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Script
{
    public abstract class Enemy : MonoBehaviour, IDamagable, IAttackable
    {
        [Header("Stats")]
        public float health;
        public float damage;

        [FormerlySerializedAs("_patrolSpeed")]
        [Header("Movement Settings")]
        [SerializeField] private float patrolSpeed = 3f;
        [SerializeField] private float chaseSpeed = 5f;
        [SerializeField] private float patrolRange = 5f;

        [FormerlySerializedAs("_detectionRadius")]
        [Header("Detection & Attack Settings")]
        [SerializeField] private float detectionRadius = 5f;
        [FormerlySerializedAs("_attackRadius")] [SerializeField] private float attackRadius = 2f;
        [SerializeField] private float attackCooldown = 1f;

        [Header("Layers")]
        [SerializeField] private LayerMask backgroundLayer;
        [SerializeField] private LayerMask collideObjectsLayer;

        private Rigidbody2D _rb;
        private Transform _player;
        private Vector2 _randomPatrolPoint;
        private float _timeSinceLastAttack = 0f;
        private EnemyState _currentState;

        private enum EnemyState
        {
            Patrolling,
            Chasing,
            Attacking
        }

        private void Start()
        {
            InitializeComponents();
            SetInitialState();
        }

        private void InitializeComponents()
        {
            _rb = GetComponent<Rigidbody2D>();
            _player = FindObjectOfType<PlayerAttack>().transform;
        }

        private void SetInitialState()
        {
            _currentState = EnemyState.Patrolling;
            PickRandomPatrolPoint();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            HandleStates();
        }

        private void HandleStates()
        {
            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    Patrol();
                    break;
                case EnemyState.Chasing:
                    Chase();
                    break;
                case EnemyState.Attacking:
                    Attack();
                    break;
            }
        }

        private void Patrol()
        {
            MoveTowards(_randomPatrolPoint, patrolSpeed);
            
            if (IsCloseTo(_randomPatrolPoint))
                PickRandomPatrolPoint();

            if (IsCloseTo(_player.position, detectionRadius))
                _currentState = EnemyState.Chasing;
        }

        private void Chase()
        {
            MoveTowards(_player.position, chaseSpeed);

            if (IsCloseTo(_player.position, attackRadius))
                _currentState = EnemyState.Attacking;
            else if (!IsCloseTo(_player.position, detectionRadius))
                _currentState = EnemyState.Patrolling;
        }

        private void Attack()
        {
            if (_timeSinceLastAttack >= attackCooldown)
            {
                TryDealDamageToPlayer();
                _timeSinceLastAttack = 0f;
            }

            _currentState = IsCloseTo(_player.position, attackRadius) ? EnemyState.Chasing : EnemyState.Patrolling;
        }

        private void TryDealDamageToPlayer()
        {
            IDamagable playerDamagable = _player.GetComponent<IDamagable>();
            if (playerDamagable != null)
                playerDamagable.TakeDamage(damage);
        }

        private void PickRandomPatrolPoint()
        {
            _randomPatrolPoint = GetRandomPointWithinPatrolRange();
            for (int i = 0; i < 10; i++)
            {
                if (IsValidPatrolPoint(_randomPatrolPoint))
                    return;

                _randomPatrolPoint = GetRandomPointWithinPatrolRange();
            }
        }

        private Vector2 GetRandomPointWithinPatrolRange()
        {
            return (Vector2)transform.position + new Vector2(Random.Range(-patrolRange, patrolRange), Random.Range(-patrolRange, patrolRange));
        }

        private bool IsValidPatrolPoint(Vector2 point)
        {
            Collider2D overlap = Physics2D.OverlapPoint(point, backgroundLayer);
            if (overlap == null)
                return false;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, point - (Vector2)transform.position, Vector2.Distance(transform.position, point), collideObjectsLayer);
            return hit.collider == null;
        }

        private bool IsCloseTo(Vector2 target, float threshold = 0.1f)
        {
            return Vector2.Distance(transform.position, target) < threshold;
        }

        private void MoveTowards(Vector2 target, float speed)
        {
            _rb.position = Vector2.MoveTowards(_rb.position, target, speed * Time.deltaTime);
        }

        public void TakeDamage(float damageAmount)
        {
            health -= damageAmount;
            if (health <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public void Attack(IDamagable target)
        {
            target.TakeDamage(damage);
        }
    }
}
