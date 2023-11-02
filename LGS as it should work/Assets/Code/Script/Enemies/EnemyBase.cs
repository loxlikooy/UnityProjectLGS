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
        public Vector2[] PatrolWaypoints => patrolWaypoints;
        public Transform playerTransform; // Assign this via the Unity inspector or find it at runtime
        public float detectionRange = 5.0f; // The range within which the enemy will start chasing the player
        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
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
        public abstract void MoveTo(Vector2 point);
        public abstract bool CanSeePlayer();
        public abstract bool CanAttackPlayer();
        
        protected bool IsPlayerInRange()
        {
            if (playerTransform == null) return false;

            return Vector2.Distance(transform.position, playerTransform.position) <= detectionRange;
        }
        
        public bool CheckIfPlayerInRange()
        {
            return IsPlayerInRange();
        }
        
        // Somewhere in EnemyBase.cs
        public void TransitionToAttack()
        {
            EnemyNavigation navigation = GetComponent<EnemyNavigation>(); // Or however you get your navigation component
            IEnemyState attackState = new AttackState(this, navigation); // Make sure you have a constructor that matches this in AttackState
            ChangeState(attackState);
        }

    }
}