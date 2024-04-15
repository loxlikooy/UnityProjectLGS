using System.Collections.Generic;
using System.Linq;
using Code.Script;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> quests = new List<Quest>();
    [SerializeField] private EXP exp;
    [SerializeField] private TextMeshProUGUI questText;
    private Quest currentActiveQuest;
    
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
            exp.AddExp(quest.QuestExpValue);
            Debug.Log("опыт добавлен" + quest.QuestExpValue + exp);
        }
    }

    public void ActivateQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !quest.IsCompleted)
        {
            currentActiveQuest = quest;
            UpdateQuestText($"Current Quest: {questName}");
        }
        else
        {
            Debug.LogWarning($"Cannot activate quest: {questName}. Quest not found or already completed.");
        }
    }

    private void ShowNextActiveQuest()
    {
        if (currentActiveQuest == null)
        {
            UpdateQuestText("You have no quests");
            return;
        }

        int currentIndex = quests.IndexOf(currentActiveQuest);
        for (int i = currentIndex + 1; i <= quests.Count; i++)
        {
            if (i>= quests.Count)
            {
                i = 0;
            }
            if (!quests[i].IsCompleted)
            { 
                Debug.Log(quests[0]);
               currentActiveQuest = quests[i];
                UpdateQuestText($"Current Quest: {quests[i].QuestName}");
                return;
            }
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
    
    private void Start()
    {
        AddQuest(new Quest("Explore the Forest", 20f));
        AddQuest(new Quest("Retrieve the Ancient Relic", 30f));
    }
}
