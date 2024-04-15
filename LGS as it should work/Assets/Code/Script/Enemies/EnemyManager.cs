public static class EnemyManager
{
    private static int chasingEnemies = 0;

    public static void StartChasing()
    {
        chasingEnemies++;
        UpdateMusic();
    }

    public static void StopChasing()
    {
        if (chasingEnemies > 0) chasingEnemies--;
        UpdateMusic();
    }

    private static void UpdateMusic()
    {
        if (chasingEnemies > 0)
        {
            MusicController.Instance.PlayChaseMusic();
        }
        else
        {
            MusicController.Instance.PlayPatrolMusic();
        }
    }
}