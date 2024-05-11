using Code.Script;
using UnityEngine;

public class Artifacts : MonoBehaviour
{
    [SerializeField] private ComponentGetter componentGetter;
    [SerializeField] private string nameOfQuest;
    [SerializeField] private bool turnOnVampiricOnPickup = false; // Новое поле

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.Instance.CompleteQuest(nameOfQuest);
            
            if (turnOnVampiricOnPickup)
            {
                componentGetter.PlayerAttackComponent.TurnOnVampiric();
            }
            
            Destroy(gameObject);
        }
    }
}