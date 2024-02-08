using UnityEngine;

namespace Code.Script
{
    public abstract class Enemy : MonoBehaviour, IDamagable, IAttackable
    {

        [Header("EXP Gainer")] 
        [SerializeField] private float expValue;
        [Header("Stats")] 
        [SerializeField]private float health;
        [SerializeField]private float damage;

        
        [Header("Movement Settings")]
        [SerializeField] private float patrolSpeed = 3f;
        [SerializeField] private float chaseSpeed = 5f;
        [SerializeField] private float patrolRange = 5f;

        
        [Header("Detection & Attack Settings")]
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private float attackRadius = 2f;
        [SerializeField] private float attackCooldown = 1f;

        [Header("Layers")]
        [SerializeField] private LayerMask backgroundLayer;
        [SerializeField] private LayerMask collideObjectsLayer;

        private Rigidbody2D _rb;
        private Transform _player;
        private Vector2 _randomPatrolPoint;
        private float _timeSinceLastAttack = 0f;
        private EnemyState _currentState;
        private Animator _animator; 
        private SpriteRenderer _spriteRenderer;
        private EXP _exp;


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
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
        }

        private void InitializeComponents()
        {
            GameObject playerGameObject = GameObject.FindWithTag("Player");
            _rb = GetComponent<Rigidbody2D>();
            _player = FindObjectOfType<PlayerAttack>().transform;
            _exp = playerGameObject.GetComponent<EXP>();
        }

        private void SetInitialState()
        {
            _currentState = EnemyState.Patrolling;
            
        }

        private void Update()
        {
            HandleStates();
        }

        private void HandleStates()
        {
            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    Patrol();
                    _animator.SetBool("Patrolling", true);
                    break;
                case EnemyState.Chasing:
                    Chase();
                    _animator.SetBool("Patrolling", false);
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
            
            if (_player == null)
            {
                return;
            }

            MoveTowards(_player.position, chaseSpeed);

            if (IsCloseTo(_player.position, attackRadius))
                _currentState = EnemyState.Attacking;
            else if (!IsCloseTo(_player.position, detectionRadius))
                _currentState = EnemyState.Patrolling;
            
        }

        private void Attack()
        {
            if (_player == null)
            {
                return;
            }
            _timeSinceLastAttack += Time.deltaTime;
            if (_timeSinceLastAttack >= attackCooldown)
            {
                TryDealDamageToPlayer();
                _timeSinceLastAttack = 0f;
            }
            

            if (!IsCloseTo(_player.position, attackRadius))
            {
                _timeSinceLastAttack = 0f;
                _currentState = IsCloseTo(_player.position, attackRadius) ? EnemyState.Chasing : EnemyState.Patrolling;
            }
           
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
            health -= damageAmount;
            if (health <= 0)
                Die();
        }

        private void Die()
        {
            _exp.AddExp(expValue);
            Destroy(gameObject);
            this.enabled = false;
        }

        public void Attack(IDamagable target)
        {
            target.TakeDamage(damage);
        }
    }
}
