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
    private bool isQuestActivated = false; // Флаг для проверки активации квеста
    private string questToActivate; // Имя квеста, который нужно активировать

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
        // Проверяем, был ли активирован квест, и если да, то активируем его
        if (isQuestActivated)
        {
            CompleteQuest(questToActivate);
            isQuestActivated = false; // Сбрасываем флаг активации квеста
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

    // Метод для активации квеста извне (например, вызывается из другого скрипта)
    public void ActivateQuest(string questName)
    {
        Quest quest = GetQuestByName(questName);
        if (quest != null && !quest.IsCompleted)
        {
            Debug.Log($"Quest activated: {questName}");
            Debug.Log($"Experience added: {quest.QuestExpValue}");
            // Update quest text to display the activated quest's name
            UpdateQuestText($"Current Quest: {questName}");

            // Additional actions upon quest activation can be added here
        }
        else
        {
            Debug.LogWarning($"Cannot activate quest: {questName}. Quest not found or already completed.");
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
        // Добавляем новые квесты при старте игры
        AddQuest(new Quest("Explore the Forest", 20f));
        AddQuest(new Quest("Retrieve the Ancient Relic", 30f));
    }
}
