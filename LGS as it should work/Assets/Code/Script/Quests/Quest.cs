public class Quest
{
    public string QuestName { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsActive { get; private set; } // Новое свойство
    public float QuestExpValue { get; private set; }

    public Quest(string questName, float questExpValue)
    {
        QuestName = questName;
        IsCompleted = false;
        IsActive = false; // Инициализируем как неактивный
        QuestExpValue = questExpValue;
    }

    public void Complete()
    {
        IsCompleted = true;
        IsActive = false; // После завершения квест становится неактивным
    }

    public void Activate()
    {
        IsActive = true; // Активация квеста
    }
}