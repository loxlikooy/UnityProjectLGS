using System;
using Unity.VisualScripting;
using UnityEngine;

public class QuestInteraction : MonoBehaviour
{
    // Reference to the player character or any object representing the player
    [SerializeField] private GameObject player;
    
    // Reference to the quest manager
    [SerializeField] private QuestManager questManager;

    // Name of the quest to activate
    [SerializeField] private string questName;
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == player && Input.GetKey(KeyCode.E))
        {
            StartQuest();
            DestroyChildObjects();
        }
    }
    
    private void DestroyChildObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
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