public class Quest
{
    public string QuestName { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsActive { get; private set; }
    public float QuestExpValue { get; private set; }

    public Quest(QuestData questData)
    {
        QuestName = questData.questName;
        IsCompleted = false;
        IsActive = false;
        QuestExpValue = questData.questExpValue;
    }

    public void Complete()
    {
        IsCompleted = true;
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}