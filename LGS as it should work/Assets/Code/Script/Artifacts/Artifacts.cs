using System;
using Code.Script;
using UnityEngine;

public class Artifacts : MonoBehaviour
{
    public static event Action OnArtifactWolfPickedUp;
    public static event Action OnArtifactQumyzPickedUp;
    
    private ComponentGetter componentGetter;
    [SerializeField] private string nameOfQuest;
    [SerializeField] private bool turnOnVampiricOnPickup;
    [SerializeField] private bool wolfsShouldNotAttack;
    [SerializeField] private bool additionalDashRange;
    [SerializeField] private bool qumyzHpRegen;
    [SerializeField] private bool applyBurningEffect;
    [SerializeField] private bool isLetterArtifact; // New flag for letter artifact
    [SerializeField] private float floatFrequency = 1f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool headBuff;

    private Vector3 startPos;
    private float rotationDirection = 1f;
    
    private void Start()
    {
        componentGetter = FindObjectOfType<ComponentGetter>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float newY = startPos.y + Mathf.Abs(Mathf.Sin(Time.time * floatFrequency));
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        transform.Rotate(0, rotationDirection * rotationSpeed * Time.deltaTime, 0);

        if (transform.rotation.eulerAngles.y >= 45f && rotationDirection > 0)
        {
            rotationDirection = -1f;
        }
        else if (transform.rotation.eulerAngles.y <= -45f && rotationDirection < 0)
        {
            rotationDirection = 1f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Input.GetKey(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(nameOfQuest))
            { 
                QuestManager.Instance.CompleteQuest(nameOfQuest);
            }

            if (turnOnVampiricOnPickup)
            {
                componentGetter.PlayerAttackComponent.TurnOnVampiric();
            }

            if (wolfsShouldNotAttack)
            {
                OnArtifactWolfPickedUp?.Invoke();
            }

            if (additionalDashRange)
            {
                componentGetter.PlayerDash.IncreaseDashRange(0.5f);
            }

            if (qumyzHpRegen)
            {
                componentGetter.HealthComponent.HealthRegen(30);
                OnArtifactQumyzPickedUp?.Invoke();
            }

            if (applyBurningEffect)
            {
                componentGetter.PlayerAttackComponent.TurnOnBurn();
            }

            if (headBuff)
            {
                componentGetter.PlayerAttackComponent.IncreaseDamage(40);  
                componentGetter.HealthComponent.IncreaseHealth(20f);
            }

            if (isLetterArtifact)
            {
                PlayerPrefs.SetInt("HasLetterArtifact", 1); // Save the artifact state
            }
            
            Destroy(gameObject); 
        }
    }
}
