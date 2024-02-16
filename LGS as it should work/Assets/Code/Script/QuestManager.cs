using System.Collections.Generic;
using System.Linq;
using Code.Script;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> quests = new List<Quest>();
    [SerializeField]
    private EXP exp;
    [SerializeField] private TextMeshProUGUI questText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }

    public Quest GetQuestByName(string questName)
    {
        return quests.FirstOrDefault(q => q.QuestName == questName);
    }

    public void CompleteQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !quest.IsCompleted)
        {
            quest.Complete();
            exp.AddExp(quest.questExpValue);
            Debug.Log("опыт добавлен" + quest.questExpValue + exp);
        }
        
    }
    public void UpdateQuestText(string newText)
    {
        questText.text = newText;
    }
    public void EnableQuestText(bool isEnabled)
    {
        questText.enabled = isEnabled;
    }
    
    
}