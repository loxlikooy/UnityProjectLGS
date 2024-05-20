using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private EnemyConfigurationSO enemyConfiguration;
    [SerializeField] private int minEnemiesToSpawn = 1;
    [SerializeField] private int maxEnemiesToSpawn = 3;

    private Room room;

    private void Start()
    {
        room = GetComponentInParent<Room>();
        if (room == null)
        {
            Debug.LogError("ChestInteraction: Room component not found in parent hierarchy.");
        }
        else
        {
            Debug.Log("ChestInteraction: Room component found.");
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player is within chest interaction range.");
            if (Input.GetKeyDown(interactionKey))
            {
                Debug.Log("Interaction key pressed.");
                SpawnEnemiesInRoom();
            }
        }
    }

    private void SpawnEnemiesInRoom()
    {
        if (room == null)
        {
            Debug.LogError("ChestInteraction: Room component is null, cannot spawn enemies.");
            return;
        }

        int enemyCount = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            int objectX = Random.Range(room.X + 1, room.X + room.Width - 1);
            int objectY = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            Vector3 enemyPos = new Vector3(objectX * room.CellSize, objectY * room.CellSize, 0);
            Instantiate(enemyConfiguration.enemyPrefab, enemyPos, Quaternion.identity, room.transform);
        }

        Debug.Log($"{enemyCount} enemies spawned in the room with the chest.");
    }
}
