using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
        protected EnemyState _currentState;
        protected Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private EXP _exp;
        private bool _isChasing;
        private float _attackRadius;
        private Room _room;
        private Coroutine _damageOverTimeCoroutine;
        protected IDamagable playerDamagable;

        public event Action<Enemy> OnDeath; // Event for enemy death

        protected enum EnemyState
        {
            Patrolling,
            Chasing,
            Attacking
        }

        // Dynamic damage effect variables
        public int minPieces = 3; // Minimum number of pieces to generate
        public int maxPieces = 10; // Maximum number of pieces to generate
        public float minPieceSize = 0.05f; // Minimum size of damage pieces
        public float maxPieceSize = 0.2f; // Maximum size of damage pieces

        protected virtual void Start()
        {
            _health = enemyData.enemyHealth;
            _maxHealth = enemyData.enemyHealth;
            _attackRadius = enemyData.enemyAttackRadius;
            InitializeComponents();
            SetInitialState();
            PickRandomPatrolPoint();

            // Find the Room component in the parent hierarchy
            _room = GetComponentInParent<Room>();
            if (_room != null)
            {
                _room.AddEnemy(gameObject);
            }
        }

        public virtual void InitializeComponents()
        {
            GameObject playerGameObject = GameObject.FindWithTag("Player");
            _rb = GetComponent<Rigidbody2D>();
            Player = FindObjectOfType<PlayerAttack>().transform;
            _exp = playerGameObject.GetComponent<EXP>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            playerDamagable = Player.GetComponent<IDamagable>();
        }

        private void SetInitialState()
        {
            _currentState = EnemyState.Patrolling;
        }

        protected virtual void Update()
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
                    _animator.SetBool("Chasing", false);
                    _animator.SetBool("Attacking", false);
                    _animator.SetBool("PreAttacking", false);
                    break;
                case EnemyState.Chasing:
                    Chase();
                    _animator.SetBool("Patrolling", false);
                    _animator.SetBool("Chasing", true);
                    _animator.SetBool("Attacking", false);
                    _animator.SetBool("PreAttacking", false);
                    break;
                case EnemyState.Attacking:
                    Attack();
                    _animator.SetBool("PreAttacking", true);
                    _animator.SetBool("Patrolling", false);
                    _animator.SetBool("Chasing", false);
                    break;
            }
        }

        protected virtual void Patrol()
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
                _timeSinceLastAttack = 0f;
                if (_isChasing)
                {
                    EnemyManager.StopChasing();
                    _isChasing = false;
                }
            }

            if (IsCloseTo(Player.position, _attackRadius))
                _currentState = EnemyState.Attacking;
        }

        protected virtual void Attack()
        {
            if (Player == null) return;

            _attackRadius = enemyData.enemyAttackRadius * 1.5f;
            _timeSinceLastAttack += Time.deltaTime;
            if (_timeSinceLastAttack >= enemyData.enemyAttackCooldown)
            {
                _animator.SetBool("Attacking", true);
                _timeSinceLastAttack = 0f;
            }

            if (IsCloseTo(Player.position, _attackRadius)) return;
            
            _currentState = EnemyState.Chasing;
            _attackRadius = enemyData.enemyAttackRadius;
        }

        private void TryDealDamageToPlayer()
        {
            if (playerDamagable != null)
            {
                playerDamagable.TakeDamage(enemyData.enemyDamage);
            }
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
            ShowDamageEffect();

            if (_health <= 0)
                Die();
        }

        private void ShowDamageEffect()
        {
            if (_spriteRenderer != null)
            {
                int numberOfPieces = Random.Range(minPieces, maxPieces);
                for (int i = 0; i < numberOfPieces; i++)
                {
                    CreateDamagePiece();
                }
            }
        }

        private void CreateDamagePiece()
        {
            // Randomize the size of the damage piece
            float pieceWidth = _spriteRenderer.sprite.rect.width * Random.Range(minPieceSize, maxPieceSize);
            float pieceHeight = _spriteRenderer.sprite.rect.height * Random.Range(minPieceSize, maxPieceSize);

            // Generate a random position within the bounds of the sprite
            float x = Random.Range(0, _spriteRenderer.sprite.rect.width - pieceWidth);
            float y = Random.Range(0, _spriteRenderer.sprite.rect.height - pieceHeight);

            // Create a new sprite for the damage piece
            Sprite pieceSprite = Sprite.Create(_spriteRenderer.sprite.texture,
                                               new Rect(x, y, pieceWidth, pieceHeight),
                                               new Vector2(0.5f, 0.5f));

            // Create a GameObject for the damage piece
            GameObject piece = new GameObject("DamagePiece");
            piece.transform.position = (Vector2)transform.position + Random.insideUnitCircle * 0.5f;

            // Add components to the piece GameObject
            SpriteRenderer pieceSpriteRenderer = piece.AddComponent<SpriteRenderer>();
            Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
            piece.AddComponent<BoxCollider2D>();

            // Set the sprite of the damage piece
            pieceSpriteRenderer.sprite = pieceSprite;
            pieceSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder + 1; // Ensure the piece is rendered above the enemy

            // Add some random movement to the piece
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f)), ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-200f, 200f));
        }

        private void Die()
        {
            if (_damageOverTimeCoroutine != null)
            {
                StopCoroutine(_damageOverTimeCoroutine);
            }

            EnemyManager.StopChasing();
            _isChasing = false;
            _exp.AddExp(enemyData.enemyExpValue);
            OnDeath?.Invoke(this); // Trigger the death event

            // Notify the room that this enemy has died
            if (_room != null)
            {
                _room.RemoveEnemy(gameObject);
                _room.AllEnemiesDefeated();
            }

            Destroy(gameObject);
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

        private void IsNotAttacking()
        {
            _animator.SetBool("Attacking", false);
        }

        public void ApplyDamageOverTime(float damageAmount, float duration, float interval)
        {
            if (_damageOverTimeCoroutine != null)
            {
                StopCoroutine(_damageOverTimeCoroutine);
            }
            _damageOverTimeCoroutine = StartCoroutine(DamageOverTimeCoroutine(damageAmount, duration, interval));
        }

        private IEnumerator DamageOverTimeCoroutine(float damageAmount, float duration, float interval)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                TakeDamage(damageAmount);
                elapsed += interval;
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
