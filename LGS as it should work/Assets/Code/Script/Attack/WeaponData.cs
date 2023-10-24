using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public Sprite weaponSprite;
    // Other attributes like range, attack speed, etc.
}
