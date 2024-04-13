using UnityEngine;

public class QuestExamples : MonoBehaviour
{
    private void Start()
    {
        // Create an instance of the QuestManager
        QuestManager questManager = QuestManager.Instance;

        // Create and add quests
        Quest quest1 = new Quest("Убить дракона");
        questManager.AddQuest(quest1);

        Quest quest2 = new Quest("Найти сокровища");
        questManager.AddQuest(quest2);

        Quest quest3 = new Quest("Исследовать темный лес");
        questManager.AddQuest(quest3);
        
        // Activate quests if needed
        // questManager.ActivateQuest("Убить дракона");
    }
}