using UnityEngine;

public class QuestInteraction : MonoBehaviour
{
    // Reference to the player character or any object representing the player
    [SerializeField] private GameObject player;
    
    // The distance at which the player can interact with the NPC
    private float _interactionDistance = 3f;

    // Reference to the quest manager
    public QuestManager questManager;

    // Name of the quest to activate
    public string questName;

    private bool isInRange = false;

    private void Update()
    {
        // Check if the player is in range and the interaction key is pressed
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartQuest();
            Debug.Log("started");
        }
    }

    // Check if the player is within interaction distance
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isInRange = true;
            Debug.Log(isInRange);
        }
    }

    // Check if the player exits the interaction distance
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isInRange = false;
        }
    }

    // Method to start the quest
    private void StartQuest()
    {
        if (questManager != null && !string.IsNullOrEmpty(questName))
        {
            Quest quest = questManager.GetQuestByName(questName);
            if (quest != null)
            {
                questManager.ActivateQuest(questName);
            }
            else
            {
                Debug.LogWarning("Quest not found: " + questName);
            }
        }
        else
        {
            Debug.LogWarning("QuestManager or QuestName is not assigned!");
        }
    }
}