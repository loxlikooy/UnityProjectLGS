using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Code.Script;

public class ChestInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private List<EnemyConfigurationSO> enemyConfigurations; // List of enemy configurations
    [SerializeField] private int minEnemiesToSpawn = 1;
    [SerializeField] private int maxEnemiesToSpawn = 3;
    [SerializeField] private List<ArtifactConfigSO> artifactConfigs; // List of artifact configurations
    [SerializeField] private Sprite closedChestSprite;
    [SerializeField] private Sprite openChestSprite;
    [SerializeField] private GameObject doorPrefab; // Prefab for the door

    private Room room;
    private bool enemiesSpawned = false;
    private bool artifactDropped = false;
    private SpriteRenderer spriteRenderer;
    private bool _doorSpawned;

    private static int totalChests = 0;
    private static int interactedChests = 0;

    private void Awake()
    {
        totalChests++;
        Debug.Log(totalChests);
    }

    private void Start()
    {
        room = GetComponentInParent<Room>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedChestSprite;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        if (!Input.GetKey(interactionKey)) return;

        if (!enemiesSpawned)
        {
            SpawnEnemiesInRoom();
            enemiesSpawned = true;
        }
        else if (enemiesSpawned && room.AllEnemiesDefeated() && !artifactDropped)
        {
            spriteRenderer.sprite = openChestSprite;
            artifactDropped = true;
            interactedChests++;
            if (!CheckForDoorSpawn())
            {
                StartCoroutine(DropArtifactWithDelay());
            }
            Destroy(gameObject, 1.5f); 
        }
    }

    private void SpawnEnemiesInRoom()
    {
        if (room == null || enemyConfigurations == null || enemyConfigurations.Count == 0)
        {
            return;
        }

        int enemyCount = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            int objectX = Random.Range(room.X + 1, room.X + room.Width - 1);
            int objectY = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            Vector3 enemyPos = new Vector3(objectX * room.CellSize, objectY * room.CellSize, 0);

            // Select a random enemy configuration
            EnemyConfigurationSO randomEnemyConfig = enemyConfigurations[Random.Range(0, enemyConfigurations.Count)];

            GameObject enemy = Instantiate(randomEnemyConfig.enemyPrefab, enemyPos, Quaternion.identity, room.transform);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.InitializeComponents(); // Explicitly call initialization if needed
                
                // Add the enemy to the list of enemies in PlayerAttack
                PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();
                if (playerAttack != null)
                {
                    playerAttack.AddEnemy(enemyScript);
                }
            }
        }
    }

    private IEnumerator DropArtifactWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        DropArtifact();
    }

    private void DropArtifact()
    {
        if (artifactConfigs == null || artifactConfigs.Count == 0)
        {
            return;
        }

        foreach (var config in artifactConfigs)
        {
            if (Random.value <= config.dropChance / 100f)
            {
                Vector3 artifactPos = new Vector3(room.CenterX * room.CellSize, room.CenterY * room.CellSize, 0);
                Instantiate(config.artifactPrefab, artifactPos, Quaternion.identity, room.transform);
                break;
            }
        }
    }

    private bool CheckForDoorSpawn()
    {
        // If all chests have been interacted with or there's a 10% chance, spawn the door
        if (!_doorSpawned && (interactedChests == totalChests || Random.value <= 0.1f))
        {
            SpawnDoor();
            _doorSpawned = true;
            return true;
        }
        return false;
    }

    private void SpawnDoor()
    {
        if (doorPrefab != null && room != null)
        {
            Vector3 doorPos = new Vector3(room.CenterX * room.CellSize, room.CenterY * room.CellSize, 0);
            Instantiate(doorPrefab, doorPos, Quaternion.identity, room.transform);
        }
    }
}
