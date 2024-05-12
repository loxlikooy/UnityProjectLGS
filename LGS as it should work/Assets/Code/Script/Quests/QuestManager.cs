using System.Collections.Generic;
using System.Linq;
using Code.Script;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> _quests = new List<Quest>();
    [SerializeField] private List<string> completedQuests = new List<string>();
    [SerializeField] private EXP exp;
    [SerializeField] private TextMeshProUGUI questText;
    private Quest _currentActiveQuest;
    
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
        
        AddQuest(new Quest("Explore the Forest", 20f));
        AddQuest(new Quest("Retrieve the Ancient Relic", 30f));
        AddQuest(new Quest("test", 30f));
        LoadCompletedQuests();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowNextActiveQuest();
        }
    }

    private void AddQuest(Quest quest)
    {
        _quests.Add(quest);
    }
    
    private static void LoadCompletedQuests()
    {
        foreach (Quest quest in Instance._quests)
        {
            if (PlayerPrefs.HasKey("CompletedQuests"))
            {
                string[] completedQuestNames = PlayerPrefs.GetString("CompletedQuests").Split(',');
                foreach (string questName in completedQuestNames)
                {
                    if (questName == quest.QuestName)
                    {
                        quest.Complete();
                        break;
                    }
                }
            }
        }
    }



    public Quest GetQuestByName(string questName)
    {
        return _quests.FirstOrDefault(q => q.QuestName == questName);
    }

    public void CompleteQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !quest.IsCompleted && quest.IsActive)
        {
            quest.Complete();
            exp.AddExp(quest.QuestExpValue);
            completedQuests.Add(questName); // Добавляем завершенный квест в список завершенных квестов
            SaveCompletedQuests(); // Сохраняем список завершенных квестов
        }
    }


    public void ActivateQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !completedQuests.Contains(questName))
        {
            _currentActiveQuest = quest;
            _currentActiveQuest.Activate(); // Активация квеста
            UpdateQuestText($"Current Quest: {questName}");
        }
    }


    private void ShowNextActiveQuest()
    {
        if (_currentActiveQuest == null)
        {
            UpdateQuestText("You have no quests");
            return;
        }

        int currentIndex = _quests.IndexOf(_currentActiveQuest);
        for (int i = currentIndex + 1; i <= _quests.Count; i++)
        {
            if (i>= _quests.Count)
            {
                i = 0;
            }
            if (!_quests[i].IsCompleted && _quests[i].IsActive)
            { 
               _currentActiveQuest = _quests[i];
                UpdateQuestText($"Current Quest: {_quests[i].QuestName}");
                return;
            }
        }
    }

    private void UpdateQuestText(string newText)
    {
        questText.text = newText;
    }
    
    
    public List<Quest> GetQuests()
    {
        return _quests;
    }
    
    private void SaveCompletedQuests()
    {
        PlayerPrefs.SetString("CompletedQuests", string.Join(",", completedQuests.ToArray()));
    }

}
