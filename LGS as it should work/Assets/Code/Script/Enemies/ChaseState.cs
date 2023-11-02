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
            if (enemy.player == null) return;

            // Update the destination to be the player's current 2D position
            navigation.SetDestination(new Vector2(enemy.player.position.x, enemy.player.position.y));

            // Optionally, switch back to patrol state if the player is no longer in range
            if (!enemy.CanSeePlayer() && !enemy.CheckIfPlayerInRange())
            {
                // You must pass the waypoints when transitioning to PatrolState
                enemy.ChangeState(new PatrolState(enemy, navigation, enemy.PatrolWaypoints)); // Make sure PatrolWaypoints is accessible
            }
            else if (enemy.CanAttackPlayer())
            {
                // Transition to attack state
                enemy.TransitionToAttack(); // This method should handle the transition to AttackState
            }
        }

        public void Exit()
        {
            // Chase state exited
            // You might want to unset the 'isChasing' flag or end an animation here
        }
    }
}