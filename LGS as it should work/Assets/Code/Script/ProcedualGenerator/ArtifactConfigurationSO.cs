using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactConfig", menuName = "ScriptableObjects/ArtifactConfig", order = 1)]
public class ArtifactConfigSO : ScriptableObject
{
    public GameObject artifactPrefab;
    public float dropChance;
}