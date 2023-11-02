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

        protected override void Update()
        {
            base.Update(); // Call to base class Update() if it's implemented
            Attract();
        }

        public override void MoveTo(Vector3 point)
        {
            // Implement movement logic toward the point here
        }

        public override bool CanSeePlayer()
        {
            // Implement the logic to determine if the Kuldirgish can see the player here
            return false;
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



        

        public override bool CanAttackPlayer()
        {
            // Implement the logic to determine if the Kuldirgish can attack the player here
            return false;
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