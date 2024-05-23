using UnityEngine;

[CreateAssetMenu(fileName = "Permament Upgrade SO", menuName = "Permament Upgrade")]
public class PermamentUpgradeSO : ScriptableObject
{
    public int exp;
    public int timesUpgradedStrength;

    public void LevelGained()
    {
        exp++;
    }
}