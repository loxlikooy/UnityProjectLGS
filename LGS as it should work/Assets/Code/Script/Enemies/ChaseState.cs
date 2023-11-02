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
        }

        public void Execute()
        {
            // Attack logic
            if (!enemy.CanAttackPlayer())
            {
                // Assuming you have access to an EnemyNavigation instance here
                EnemyNavigation navigation = enemy.GetComponent<EnemyNavigation>();
        
                // Now provide the required arguments
                enemy.ChangeState(new ChaseState(enemy, navigation));
            }
        }


        public void Exit()
        {
            // Chase state exited
        }
    }
}