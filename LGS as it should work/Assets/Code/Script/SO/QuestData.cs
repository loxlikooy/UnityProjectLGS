using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    public string questName;
    public float questExpValue;
}