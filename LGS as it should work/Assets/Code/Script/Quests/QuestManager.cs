using System.Collections.Generic;
using System.Linq;
using Code.Script;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> _quests = new List<Quest>();
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
    }

    private void Update()
    {
        // Проверяем нажатие кнопки TAB и показываем следующий активный квест
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowNextActiveQuest();
        }
    }

    private void AddQuest(Quest quest)
    {
        _quests.Add(quest);
    }

    public Quest GetQuestByName(string questName)
    {
        return _quests.FirstOrDefault(q => q.QuestName == questName);
    }

    public void CompleteQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !quest.IsCompleted)
        {
            quest.Complete();
            exp.AddExp(quest.QuestExpValue);
            
        }
    }

    public void ActivateQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !quest.IsCompleted)
        {
            _currentActiveQuest = quest;
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
            if (!_quests[i].IsCompleted)
            { 
               _currentActiveQuest = _quests[i];
                UpdateQuestText($"Current Quest: {_quests[i].QuestName}");
                return;
            }
        }
    }

    [SerializeField] private void UpdateQuestText(string newText)
    {
        questText.text = newText;
    }
    
    private void Start()
    {
        AddQuest(new Quest("Explore the Forest", 20f));
        AddQuest(new Quest("Retrieve the Ancient Relic", 30f));
    }
}
