using UnityEngine;

namespace Code.Script
{
    public class ChaseState : IEnemyState
    {
        private EnemyBase enemy;
        private EnemyNavigation navigation;

        // Constructor that accepts two parameters
        public ChaseState(EnemyBase enemy, EnemyNavigation navigation)
        {
            this.enemy = enemy;
            this.navigation = navigation;
        }

        public void Enter(EnemyBase enemy)
        {
            // Chase state entered
            // You might want to set an 'isChasing' flag or start an animation here
        }

        public void Execute()
        {
            if (enemy.playerTransform == null)
            {
                Debug.LogWarning("Player reference not set in enemy.");
                return;
            }

            Debug.Log("Chasing player at position: " + enemy.playerTransform.position);

            // Move the enemy towards the player's current position
            enemy.MoveTo(enemy.playerTransform.position);
            navigation.SetDestination(new Vector2(enemy.playerTransform.position.x, enemy.playerTransform.position.y));

            // Check if the player is no longer in sight or range to switch back to patrol
            if (!enemy.CanSeePlayer() && !enemy.CheckIfPlayerInRange())
            {
                // Make sure to pass the waypoints array to the PatrolState constructor
                enemy.ChangeState(new PatrolState(enemy, navigation, enemy.PatrolWaypoints));
            }
            else if (enemy.CanAttackPlayer())
            {
                // If the enemy can attack the player, transition to the attack state
                enemy.TransitionToAttack();
            }
        }


        public void Exit()
        {
            // Chase state exited
            // You might want to unset the 'isChasing' flag or end an animation here
        }
    }
}