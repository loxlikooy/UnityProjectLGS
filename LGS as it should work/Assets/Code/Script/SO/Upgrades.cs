using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;
    public UpgradeEffect effect;
}

public enum UpgradeEffect
{
    HealthIncrease,
    HealthRegen,
    DamageIncrease,
    DecreaseDashCooldown,
    SpeedIncrease,
    UpgradeAttackSpeed
}