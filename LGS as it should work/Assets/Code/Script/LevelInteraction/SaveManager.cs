using UnityEngine;

namespace Code.Script
{
    public class SaveManager : MonoBehaviour
    {
        public static void SavePlayerStats(ComponentGetter componentGetter)
        {
            componentGetter.PlayerAttackComponent.SaveAttackStats();
            componentGetter.PlayerDash.SaveDashStats();
            componentGetter.HealthComponent.SaveHealth();
        }

        public static void SaveQuestStates()
        {
            foreach (Quest quest in QuestManager.Instance.GetQuests())
            {
                if (!quest.IsCompleted && quest.IsActive)
                {
                    PlayerPrefs.SetInt("Quest_Active_" + quest.QuestName, 1); // Save active quest state
                }
            }
            PlayerPrefs.Save();
        }

        public static void LoadQuestStates()
        {
            QuestManager questManager = QuestManager.Instance;
            foreach (Quest quest in questManager.GetQuests())
            {
                if (!quest.IsCompleted && PlayerPrefs.GetInt("Quest_Active_" + quest.QuestName, 0) == 1)
                {
                    questManager.ActivateQuest(quest.QuestName); // Activate the quest if it was active before
                }
            }
        }
    }
}