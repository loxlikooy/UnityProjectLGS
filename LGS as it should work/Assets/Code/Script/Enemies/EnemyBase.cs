using UnityEngine;

namespace Code.Script
{
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        public float health;
        public float speed;

        protected Rigidbody2D rb;
        protected Animator animator;

        private IEnemyState currentState;
        
        [SerializeField]
        private Vector2[] patrolWaypoints;
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private EnemyNavigation navigation;

        
        protected virtual void Start()
        {
            navigation = GetComponent<EnemyNavigation>();
            if (navigation == null)
            {
                Debug.LogError("EnemyNavigation component not found!");
                return;
            }
    
            float patrolRadius = 5.0f; // or whatever value suits your game
            IEnemyState patrolState = new PatrolState(this, navigation, patrolRadius);
            ChangeState(patrolState);
        }

        protected virtual void Update()
        {
            currentState?.Execute();
        }

        public virtual void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0) Die();
        }

        protected virtual void Die()
        {
            // If you want to play a death animation, start it here
            // animator.SetTrigger("Die");

            // Disable the enemy GameObject
            gameObject.SetActive(false);

            // Alternatively, if you want to destroy the GameObject completely, use:
            // Destroy(gameObject);
        }

        public void ChangeState(IEnemyState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter(this);
        }

        // Abstract methods that must be implemented by inheriting classes
        public abstract void MoveTo(Vector3 point);
        public abstract bool CanSeePlayer();
        public abstract bool CanAttackPlayer();
        
        // Somewhere in EnemyBase.cs
        public void TransitionToAttack()
        {
            EnemyNavigation navigation = GetComponent<EnemyNavigation>(); // Or however you get your navigation component
            IEnemyState attackState = new AttackState(this, navigation);
            ChangeState(attackState);
        }

    }
}