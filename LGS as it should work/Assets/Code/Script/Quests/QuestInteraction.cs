using UnityEngine;

public class QuestInteraction : MonoBehaviour
{
    // Reference to the player character or any object representing the player
    [SerializeField] private GameObject player;
    
    // Reference to the quest manager
    [SerializeField] private QuestManager questManager;

    // Name of the quest to activate
    [SerializeField] private string questName;

    private bool _isInRange = false;

    private void Update()
    {
        
        if (_isInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartQuest();
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            _isInRange = true;
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            _isInRange = false;
        }
    }

    
    private void StartQuest()
    {
        if (questManager != null && !string.IsNullOrEmpty(questName))
        {
            Quest quest = questManager.GetQuestByName(questName);
            if (quest != null && !quest.IsCompleted) // Проверяем, не завершен ли квест
            {
                questManager.ActivateQuest(questName);
            }
        }
    }
}