using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public int CenterX => X + Width / 2;
    public int CenterY => Y + Height / 2;
    public float CellSize = 0.64f;

    private System.Random random;
    private List<GameObject> objectPrefabs;
    private int minObjectCount;
    private int maxObjectCount;
    private GameObject chestPrefab;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void Initialize(int x, int y, int width, int height, System.Random random, List<GameObject> objectPrefabs, int minObjectCount, int maxObjectCount, GameObject chestPrefab)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        this.random = random;
        this.objectPrefabs = objectPrefabs;
        this.minObjectCount = minObjectCount;
        this.maxObjectCount = maxObjectCount;
        this.chestPrefab = chestPrefab;

        gameObject.name = $"Room ({X}, {Y})";
    }

    public bool Intersects(Room other)
    {
        return X < other.X + other.Width && X + Width > other.X &&
               Y < other.Y + other.Height && Y + Height > other.Y;
    }

    public void Carve(int[,] map)
    {
        for (int x = X; x < X + Width; x++)
        {
            for (int y = Y; y < Y + Height; y++)
            {
                if (random.NextDouble() < 0.1) // 10% chance to make a part of the room irregular
                {
                    if (random.Next(0, 2) == 0)
                    {
                        map[x, y] = 1; // Create a bump
                    }
                    else
                    {
                        map[x, y] = 0; // Create a dent
                    }
                }
                else
                {
                    map[x, y] = 0; // Отметка, что это комната
                }
            }
        }
    }

    public void PlaceObjectsInRoom(Transform parentTransform)
    {
        int objectCount = random.Next(minObjectCount, maxObjectCount);
        for (int i = 0; i < objectCount; i++)
        {
            int objectX = random.Next(X + 1, X + Width - 1);
            int objectY = random.Next(Y + 1, Y + Height - 1);

            Vector3 objectPos = new Vector3(objectX * CellSize, objectY * CellSize, 0);
            GameObject objectPrefab = objectPrefabs[random.Next(objectPrefabs.Count)];
            Instantiate(objectPrefab, objectPos, Quaternion.identity, parentTransform);
        }
    }

    public void PlaceChest()
    {
        Vector3 chestPos = new Vector3(CenterX * CellSize, CenterY * CellSize, 0);
        Instantiate(chestPrefab, chestPos, Quaternion.identity, transform); // Ensure chest is a child of the room
    }

    public void AddEnemy(GameObject enemy)
    {
        spawnedEnemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
       
    }

    public bool AllEnemiesDefeated()
    {
        return spawnedEnemies.Count == 0;
    }
}
