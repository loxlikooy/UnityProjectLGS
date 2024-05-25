using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Player/Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    // Health stats
    public float maxHealth = 50f;
    public float currentHealth;

    // Attack stats
    public float playerDamage = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    public float attackVampire = 2f;
    public float attackBurn = 7f;
    public float burnDuration = 3f;
    public bool activeVampiric;
    public bool activateBurn;

    // Dash stats
    public float dashCooldown = 2.5f;
    public float dashRange = 0.8f;
    
    // Move stats
    public float moveSpeed = 50f;
}