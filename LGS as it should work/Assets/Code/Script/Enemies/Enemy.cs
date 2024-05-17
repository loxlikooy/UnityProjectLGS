using UnityEngine;

namespace Code.Script
{
    public abstract class Enemy : MonoBehaviour, IDamagable, IAttackable
    {
        // Stats
        private float _health;
        private float _maxHealth;

        // Scriptable Object
        [SerializeField]
        private EnemyData enemyData;

        // Components
        private Rigidbody2D _rb;
        protected Transform Player;
        private Vector2 _randomPatrolPoint;
        private float _timeSinceLastAttack;
        private EnemyState _currentState;
        private Animator _animator; 
        private SpriteRenderer _spriteRenderer;
        private EXP _exp;
        private bool _isChasing;
        private float _attackRadius;

        private enum EnemyState
        {
            Patrolling,
            Chasing,
            Attacking
        }

        private void Start()
        {
            _health = enemyData.enemyHealth;
            _maxHealth = enemyData.enemyHealth;
            _attackRadius = enemyData.enemyAttackRadius;
            InitializeComponents();
            SetInitialState();
            PickRandomPatrolPoint();
        }

        protected virtual void InitializeComponents()
        {
            GameObject playerGameObject = GameObject.FindWithTag("Player");
            _rb = GetComponent<Rigidbody2D>();
            Player = FindObjectOfType<PlayerAttack>().transform;
            _exp = playerGameObject.GetComponent<EXP>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void SetInitialState()
        {
            _currentState = EnemyState.Patrolling;
        }

        protected virtual void Update()
        {
            HandleStates();
            Debug.Log(_attackRadius);
        }

        private void HandleStates()
        {
            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    Patrol();
                    _animator.SetBool("Patrolling", true);
                    _animator.SetBool("Chasing", false);
                    _animator.SetBool("Attacking", false);
                    break;
                case EnemyState.Chasing:
                    Chase();
                    _animator.SetBool("Patrolling", false);
                    _animator.SetBool("Chasing", true);
                    _animator.SetBool("Attacking", false);
                    break;
                case EnemyState.Attacking:
                    Attack();
                    _animator.SetBool("Patrolling", false);
                    _animator.SetBool("Chasing", false);
                    _animator.SetBool("Attacking", true);
                    break;
            }
        }

        private void Patrol()
        {
            if (Player == null || Player.gameObject == null) return;
            MoveTowards(_randomPatrolPoint, enemyData.enemyPatrolSpeed);

            if (IsCloseTo(_randomPatrolPoint))
                PickRandomPatrolPoint();

            if (IsCloseTo(Player.position, enemyData.enemyDetectionRadius))
                _currentState = EnemyState.Chasing;
        }

        protected virtual void Chase()
        {
            if (Player == null || Player.gameObject == null) return;

            MoveTowards(Player.position, enemyData.enemyChaseSpeed);

            if (!_isChasing)
            {
                EnemyManager.StartChasing();
                _isChasing = true; 
            }

            if (!IsCloseTo(Player.position, enemyData.enemyDetectionRadius))
            {
                _currentState = EnemyState.Patrolling;
                if (_isChasing)
                {
                    EnemyManager.StopChasing();
                    _isChasing = false;
                }
            }

            if (IsCloseTo(Player.position, _attackRadius))
                _currentState = EnemyState.Attacking;
        }

        private void Attack()
        {
            if (Player == null) return;
            // Increase attack radius by 20% when attacking
            _attackRadius = enemyData.enemyAttackRadius * 1.5f;

            _timeSinceLastAttack += Time.deltaTime;
            if (_timeSinceLastAttack >= enemyData.enemyAttackCooldown)
            {
                TryDealDamageToPlayer();
                _timeSinceLastAttack = 0f;
            }

            if (!IsCloseTo(Player.position,_attackRadius))
            {
                _timeSinceLastAttack = 0f;
                _currentState = EnemyState.Patrolling;

                // Revert attack radius to original when out of range
                _attackRadius =  enemyData.enemyAttackRadius;
            }
        }

        private void TryDealDamageToPlayer()
        {
            IDamagable playerDamagable = Player.GetComponent<IDamagable>();
            if (playerDamagable != null)
                playerDamagable.TakeDamage(enemyData.enemyDamage);
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
            return (Vector2)transform.position + new Vector2(Random.Range(-enemyData.enemyPatrolRange, enemyData.enemyPatrolRange), Random.Range(-enemyData.enemyPatrolRange, enemyData.enemyPatrolRange));
        }

        private bool IsValidPatrolPoint(Vector2 point)
        {
            Collider2D overlap = Physics2D.OverlapPoint(point, enemyData.enemyBackgroundLayer);
            if (overlap == null)
                return false;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, point - (Vector2)transform.position, Vector2.Distance(transform.position, point), enemyData.enemyCollideObjectsLayer);
            return hit.collider == null;
        }

        private bool IsCloseTo(Vector2 target, float threshold = 0.1f)
        {
            return Vector2.Distance(transform.position, target) < threshold;
        }

        private void MoveTowards(Vector2 target, float speed)
        {
            Vector2 moveDirection = target - _rb.position;

            // Flip the sprite based on the movement direction
            if (moveDirection.x > 0.01f) // Moving right
            {
                _spriteRenderer.flipX = false;
            }
            else if (moveDirection.x < -0.01f) // Moving left
            {
                _spriteRenderer.flipX = true;
            }

            _rb.position = Vector2.MoveTowards(_rb.position, target, speed * Time.deltaTime);
        }

        public void TakeDamage(float damageAmount)
        {
            _health -= damageAmount;
            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            EnemyManager.StopChasing();
            _isChasing = false;
            _exp.AddExp(enemyData.enemyExpValue);
            Destroy(gameObject);
            this.enabled = false;
        }

        public void Attack(IDamagable target)
        {
            target.TakeDamage(enemyData.enemyDamage);
        }

        public float GetCurrentHealth()
        {
            return _health;
        }

        public float GetMaxHealth()
        {
            return _maxHealth;
        }
    }
}