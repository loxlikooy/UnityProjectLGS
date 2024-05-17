using System.Collections.Generic;
using System.Linq;
using Code.Script;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<QuestData> questDataList;
    private readonly List<Quest> _quests = new List<Quest>();
    private readonly List<string> _completedQuests = new List<string>();
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
            return;
        }

        foreach (QuestData questData in questDataList)
        {
            AddQuest(new Quest(questData));
        }
        Load_completedQuests();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
        if (quest == null) return;
        _quests.Add(quest);
    }
    
    private void Load_completedQuests()
    {
        if (!PlayerPrefs.HasKey("_completedQuests")) return;
        
        string[] completedQuestNames = PlayerPrefs.GetString("_completedQuests").Split(',');
        foreach (string questName in completedQuestNames)
        {
            Quest quest = GetQuestByName(questName);
            if (quest != null)
            {
                quest.Complete();
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
            _completedQuests.Add(questName);
            Save_completedQuests();
        }
        else
        {
            Debug.LogWarning($"Quest '{questName}' is either already completed or not active.");
        }
    }

    public void ActivateQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !_completedQuests.Contains(questName))
        {
            _currentActiveQuest = quest;
            _currentActiveQuest.Activate();
            UpdateQuestText($"Current Quest: {questName}");
        }
        else
        {
            Debug.LogWarning($"Quest '{questName}' is either already completed or not found.");
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

        // Check if there are any active quests
        bool hasActiveQuest = _quests.Any(q => q.IsActive && !q.IsCompleted);
        if (!hasActiveQuest)
        {
            UpdateQuestText("You have no active quests");
            return;
        }

        for (int i = currentIndex + 1; i <= _quests.Count; i++)
        {
            if (i >= _quests.Count)
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
        if (questText != null)
        {
            questText.text = newText;
        }
    }
    
    public List<Quest> GetQuests()
    {
        return _quests;
    }
    
    private void Save_completedQuests()
    {
        PlayerPrefs.SetString("_completedQuests", string.Join(",", _completedQuests));
    }
}
