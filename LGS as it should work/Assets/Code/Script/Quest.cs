using UnityEngine;

public class Quest
{
    public string QuestName { get; private set; }
    public bool IsCompleted { get; private set; }
    public float questExpValue = 10f;

    public Quest(string questName)
    {
        QuestName = questName;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
        // Здесь можно добавить дополнительные действия при завершении квеста, например, выдачу награды.
        Debug.Log($"Quest {QuestName} completed!");
    }
}