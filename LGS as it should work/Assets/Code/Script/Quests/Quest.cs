using UnityEngine;

public class Quest
{
    public string QuestName { get; private set; }
    public bool IsCompleted { get; private set; }
    public float QuestExpValue { get; private set; }

    public Quest(string questName, float questExpValue = 10f)
    {
        QuestName = questName;
        IsCompleted = false;
        QuestExpValue = questExpValue;
    }

    public void Complete()
    {
        IsCompleted = true;
        // Здесь можно добавить дополнительные действия при завершении квеста, например, выдачу награды.
        Debug.Log($"Quest {QuestName} completed!");
    }
}