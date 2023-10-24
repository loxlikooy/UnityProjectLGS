using UnityEngine;

namespace Code.Script
{
    public class Slime : Enemy
    {
        private enum EnemyState
        {
            Patrolling,
            Chasing,
            Attacking
        }

        private EnemyState currentState;

        [SerializeField]
        private float _patrolSpeed = 3f;
        [SerializeField]
        private float _chaseSpeed = 5f;
        [SerializeField]
        private float _detectionRadius = 5f;
        [SerializeField]
        private float _attackRadius = 2f;
        [SerializeField]
        private float attackCooldown = 1f;
        [SerializeField]
        private float patrolRange = 5f; // The range within which the slime can pick a random patrol point

        private float timeSinceLastAttack = 0f;

        private Rigidbody2D _rb;
        private Transform player;
        private Vector2 randomPatrolPoint;
        [SerializeField]
        private LayerMask backgroundLayer;
        [SerializeField]
        private LayerMask collideObjectsLayer;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            player = FindObjectOfType<PlayerAttack>().transform;
            currentState = EnemyState.Patrolling;
            PickRandomPatrolPoint();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            switch (currentState)
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
            if (Vector2.Distance(transform.position, randomPatrolPoint) < 0.1f)
            {
                PickRandomPatrolPoint();
            }

            _rb.position = Vector2.MoveTowards(_rb.position, randomPatrolPoint, _patrolSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) <= _detectionRadius)
            {
                currentState = EnemyState.Chasing;
            }
        }

        private void PickRandomPatrolPoint()
        {
            int attempts = 10; // limit the number of attempts to find a clear point

            for (int i = 0; i < attempts; i++)
            {
                Vector2 potentialPoint = (Vector2)transform.position +
                                         new Vector2(Random.Range(-patrolRange, patrolRange),
                                             Random.Range(-patrolRange, patrolRange));

                // Check if the point is within the background layer
                Collider2D backgroundOverlap = Physics2D.OverlapPoint(potentialPoint, backgroundLayer);

                if (backgroundOverlap != null) 
                {
                    // Check for obstacles between the current position and the potential point
                    RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, potentialPoint - (Vector2)transform.position, Vector2.Distance(transform.position, potentialPoint), collideObjectsLayer);

                    if (hit.collider == null)
                    {
                        randomPatrolPoint = potentialPoint;
                        return;
                    }
                }
            }
        }



        private void Chase()
        {
            _rb.position = Vector2.MoveTowards(_rb.position, player.position, _chaseSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) <= _attackRadius)
            {
                currentState = EnemyState.Attacking;
            }
            else if (Vector2.Distance(transform.position, player.position) > _detectionRadius)
            {
                currentState = EnemyState.Patrolling;
            }
        }

        private void Attack()
        {
            if (timeSinceLastAttack >= attackCooldown)
            {
                IDamagable playerDamagable = player.GetComponent<IDamagable>();
                if (playerDamagable != null)
                {
                    playerDamagable.TakeDamage(damage);
                }
                else
                {
                   
                }
                
                timeSinceLastAttack = 0f;
            }

            currentState = Vector2.Distance(transform.position, player.position) > _attackRadius ? EnemyState.Chasing : EnemyState.Patrolling;
        }
    }
}
