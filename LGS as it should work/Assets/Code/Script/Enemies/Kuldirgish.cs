using System.Collections;
using UnityEngine;

namespace Code.Script
{
    public class Kuldirgish : EnemyBase
    {
        [Header("Kuldirgish Specific")]
        public float attractionForce = 10f;
        public float attractionCooldown = 5f;
        private float nextAttractionTime = 0f;

        protected override void Start()
        {
            base.Start(); // Call to base class Start() if it's implemented
            nextAttractionTime = Time.time + attractionCooldown;
        }

        void Awake() {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        
        protected override void Update()
        {
            base.Update(); // Call to base class Update() if it's implemented
            Attract();
        }

        public override void MoveTo(Vector2 point)
        {
            // Implementation for how Kuldirgish moves to a point
            Vector2 direction = (point - (Vector2)transform.position).normalized;
            rb.velocity = direction * speed;
        }

        public override bool CanSeePlayer()
        {
            // Implementation for how Kuldirgish detects the player
            float detectionRange = 10.0f; // Example range value
            return Vector2.Distance(transform.position, playerTransform.position) <= detectionRange;
        }

        public override bool CanAttackPlayer()
        {
            // Implementation for how Kuldirgish decides it can attack the player
            float attackRange = 2.0f; // Example attack range value
            return Vector2.Distance(transform.position, playerTransform.position) <= attackRange;
        }

        
        protected override void Die()
        {
            // Start the death coroutine
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            // Play the death animation
            animator.SetTrigger("Die");

            // Wait for the length of the animation
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Then deactivate or destroy the enemy GameObject
            gameObject.SetActive(false);
            // Destroy(gameObject); // Use this instead if you want to completely remove the GameObject
        }
        
        private void Attract()
        {
            if (Time.time >= nextAttractionTime)
            {
                // Implement attraction logic here
                nextAttractionTime = Time.time + attractionCooldown;
            }
        }
    }
}