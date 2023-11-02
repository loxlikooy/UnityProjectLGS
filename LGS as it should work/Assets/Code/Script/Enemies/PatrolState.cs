using UnityEngine;

namespace Code.Script
{
    // PatrolState.cs
    public class PatrolState : IEnemyState
    {
        private EnemyBase enemy;
        private EnemyNavigation navigation;
        private float patrolRadius = 5.0f; // Set this to the radius within which you want the waypoints to be generated

        public PatrolState(EnemyBase enemy, EnemyNavigation navigation, float patrolRadius)
        {
            this.enemy = enemy;
            this.navigation = navigation;
            this.patrolRadius = patrolRadius;
        }

        public void Enter(EnemyBase enemy)
        {
            // Rest of the code...
            navigation.SetDestination(GetRandomWaypoint());
        }

        public void Execute()
        {
            // If the enemy sees the player, transition to chase state
            if (enemy.CanSeePlayer() && enemy.CheckIfPlayerInRange())
            {
                enemy.ChangeState(new ChaseState(enemy, navigation));
            }
            else if (navigation.HasReachedDestination())
            {
                navigation.SetDestination(GetRandomWaypoint());
            }
        }

        public void Exit()
        {
            // Cleanup if needed when exiting patrol state
        }

        private Vector2 GetRandomWaypoint()
        {
            // Generate a random point within a circle of patrolRadius around the enemy
            Vector2 randomDirection = Random.insideUnitCircle * patrolRadius;
            Vector2 currentPos = new Vector2(enemy.transform.position.x, enemy.transform.position.y);

            Vector2 randomPoint = (Vector2)enemy.transform.position + randomDirection;

            // Optional: You might want to check if the generated point is walkable or not
            // If you have a grid system or certain areas where the enemy can't go, 
            // you should incorporate that logic here.

            return randomPoint;
        }
        public PatrolState(EnemyBase enemy, EnemyNavigation navigation, Vector2[] waypoints)
        {
            // Assign the parameters to the class fields
        }

    }
}