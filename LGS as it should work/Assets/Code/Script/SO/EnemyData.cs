using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("EXP Gainer")]
    public float enemyExpValue;
    [Header("Stats")]
    public float enemyHealth;
    public float enemyDamage;

    [Header("Movement Settings")]
    public float enemyPatrolSpeed;
    public float enemyChaseSpeed;
    public float enemyPatrolRange;

    [Header("Detection & Attack Settings")]
    public float enemyDetectionRadius;
    public float enemyAttackRadius;
    public float enemyAttackCooldown;

    [Header("Layers")]
    public LayerMask enemyBackgroundLayer;
    public LayerMask enemyCollideObjectsLayer;
}