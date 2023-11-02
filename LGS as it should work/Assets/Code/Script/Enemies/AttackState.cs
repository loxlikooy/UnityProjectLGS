namespace Code.Script
{
    public class AttackState : IEnemyState
    {
        private EnemyBase enemy;
        private EnemyNavigation navigation; // You need a reference to the navigation

        // Include a constructor to pass in the necessary components
        public AttackState(EnemyBase enemy, EnemyNavigation navigation)
        {
            this.enemy = enemy;
            this.navigation = navigation;
        }

        public void Enter(EnemyBase enemy)
        {
            this.enemy = enemy;
            // Initialize attacking behavior
            // You can also initialize navigation here if needed
        }

        public void Execute()
        {
            // Attack logic
            if (!enemy.CanAttackPlayer())
            {
                // Change to chase state with the necessary arguments
                enemy.ChangeState(new ChaseState(enemy, navigation));
            }
        }

        public void Exit()
        {
            // Cleanup if needed
        }
    }
}