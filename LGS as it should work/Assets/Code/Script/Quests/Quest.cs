using UnityEngine;

public class Quest
{
    public string QuestName { get; private set; }
    public bool IsCompleted { get; private set; }
    public float QuestExpValue { get; private set; }

    public Quest(string questName, float questExpValue)
    {
        QuestName = questName;
        IsCompleted = false;
        QuestExpValue = questExpValue;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}