using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration", order = 1)]
public class EnemyConfigurationSO : ScriptableObject
{
    public GameObject enemyPrefab;
    public int minCount;
    public int maxCount;
}