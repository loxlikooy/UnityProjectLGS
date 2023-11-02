namespace Code.Script
{
    public interface IEnemyState
    {
        void Enter(EnemyBase enemy);
        void Execute();
        void Exit();
    }
}