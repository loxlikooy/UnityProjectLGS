using UnityEngine;
using System.Collections.Generic;
using Code.Script;

public class ChestInteraction : MonoBehaviour
{
   
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private EnemyConfigurationSO enemyConfiguration;
    [SerializeField] private int minEnemiesToSpawn = 1;
    [SerializeField] private int maxEnemiesToSpawn = 3;
    [SerializeField] private List<GameObject> artifactPrefabs; // Список префабов артефактов

    private Room room;
    private bool enemiesSpawned = false;
    private bool artifactDropped = false;

    private void Start()
    {
        room = GetComponentInParent<Room>();
    }
    
    
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;
        
            if (!Input.GetKeyDown(interactionKey)) return;
            
                if (!enemiesSpawned)
                {
                    SpawnEnemiesInRoom();
                    enemiesSpawned = true;
                }
                else if (enemiesSpawned && room.AllEnemiesDefeated() && !artifactDropped)
                {
                    DropArtifact();
                    artifactDropped = true;
                }
            
        
    }

    private void SpawnEnemiesInRoom()
    {
        if (room == null || enemyConfiguration == null)
        {
            return;
        }

        int enemyCount = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            int objectX = Random.Range(room.X + 1, room.X + room.Width - 1);
            int objectY = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            Vector3 enemyPos = new Vector3(objectX * room.CellSize, objectY * room.CellSize, 0);
            GameObject enemy = Instantiate(enemyConfiguration.enemyPrefab, enemyPos, Quaternion.identity, room.transform);

            
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.InitializeComponents(); // Explicitly call initialization if needed
                
                // Добавляем врага в список врагов у PlayerAttack
                PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();
                if (playerAttack != null)
                {
                    playerAttack.AddEnemy(enemyScript);
                }
            }
        }
    }

    private void DropArtifact()
    {
        GameObject artifactPrefab = artifactPrefabs[Random.Range(0, artifactPrefabs.Count)];
        Vector3 artifactPos = new Vector3(room.CenterX * room.CellSize, room.CenterY * room.CellSize, 0);
        Instantiate(artifactPrefab, artifactPos, Quaternion.identity, room.transform);
    }
}
