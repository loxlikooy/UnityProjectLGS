using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 2)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float health;
    public float damage;
    public Sprite enemySprite;
    // Other attributes like speed, attack patterns, etc.
}
