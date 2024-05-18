using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    private string seed;
    [SerializeField] private bool useRandomSeed = true;

    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;

    [SerializeField] private int roomCount;
    [SerializeField] private int roomMinSize;
    [SerializeField] private int roomMaxSize;
    
    [SerializeField] private int corridorMinWidth;
    [SerializeField] private int corridorMaxWidth;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private List<EnemyConfigurationSO> enemyConfigurations;

    private Map map;
    private List<Room> rooms;
    private System.Random random;
    private Room playerRoom;

    private void Start()
    {
        InitializeRandomSeed();
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            GenerateMap();
        }
    }

    private void InitializeRandomSeed()
    {
        if (useRandomSeed)
        {
            random = new System.Random();
            seed = random.Next(0, int.MaxValue).ToString();
        }
        else
        {
            random = new System.Random(seed.GetHashCode());
        }
    }

    private void GenerateMap()
    {
        map = new Map(width, height, random);
        int[,] mapData = map.GenerateMap(randomFillPercent);
        rooms = new List<Room>();

        GenerateRoomsAndCorridors(mapData);
        EnsureAllRoomsConnected(mapData);
        DrawMap(mapData);
        SpawnPlayer();
        PlaceEnemies(mapData);
    }

    private void ConnectRooms(Room roomA, Room roomB, int[,] mapData)
    {
        int x1 = roomA.CenterX;
        int y1 = roomA.CenterY;
        int x2 = roomB.CenterX;
        int y2 = roomB.CenterY;

        if (random.Next(0, 2) == 0)
        {
            CreateNonLinearCorridor(x1, y1, x2, y2, mapData);
        }
        else
        {
            CreateNonLinearCorridor(y1, x1, y2, x2, mapData);
        }
    }

    private void CreateNonLinearCorridor(int startX, int startY, int endX, int endY, int[,] mapData)
    {
        int x = startX;
        int y = startY;

        while (x != endX || y != endY)
        {
            if (x != endX)
            {
                x += (endX > x) ? 1 : -1;
            }
            else if (y != endY)
            {
                y += (endY > y) ? 1 : -1;
            }

            int corridorWidth = random.Next(corridorMinWidth, corridorMaxWidth);
            int corridorWidthHalf = corridorWidth / 2;

            for (int wx = -corridorWidthHalf; wx <= corridorWidthHalf; wx++)
            {
                for (int wy = -corridorWidthHalf; wy <= corridorWidthHalf; wy++)
                {
                    if (map.IsInMapRange(x + wx, y + wy))
                    {
                        mapData[x + wx, y + wy] = 0;
                    }
                }
            }

            // Add some randomness to the corridor path
            if (random.Next(0, 100) < 30)
            {
                if (x != endX)
                {
                    x += (endX > x) ? 1 : -1;
                }
                else if (y != endY)
                {
                    y += (endY > y) ? 1 : -1;
                }
            }
        }
    }

    private void GenerateRoomsAndCorridors(int[,] mapData)
    {
        // Создаем первую комнату, которую гарантированно добавляем в список
        CreateRoom(mapData, true);
    
        for (int i = 1; i < roomCount; i++)
        {
            CreateRoom(mapData, false);
        }

        for (int i = 1; i < rooms.Count; i++)
        {
            ConnectRooms(rooms[i - 1], rooms[i], mapData);
        }

        EnsureAllRoomsConnected(mapData);
    }

    private void CreateRoom(int[,] mapData, bool forceAdd)
    {
        int roomWidth = random.Next(roomMinSize, roomMaxSize);
        int roomHeight = random.Next(roomMinSize, roomMaxSize);

        int roomX = random.Next(1, width - roomWidth - 1);
        int roomY = random.Next(1, height - roomHeight - 1);

        Room newRoom = new Room(roomX, roomY, roomWidth, roomHeight, random);

        if (forceAdd || !RoomIntersects(newRoom))
        {
            rooms.Add(newRoom);
            newRoom.Carve(mapData);
        }
    }

    private void EnsureAllRoomsConnected(int[,] mapData)
    {
        // Создаем список с неприсоединенными комнатами
        List<Room> disconnectedRooms = new List<Room>(rooms);

        while (disconnectedRooms.Count > 1)
        {
            Room roomA = disconnectedRooms[0];
            Room roomB = FindClosestRoom(roomA, disconnectedRooms);

            if (roomB != null)
            {
                ConnectRooms(roomA, roomB, mapData);
                disconnectedRooms.Remove(roomB);
            }
            disconnectedRooms.Remove(roomA);
        }
    }

    private Room FindClosestRoom(Room room, List<Room> roomList)
    {
        Room closestRoom = null;
        float closestDistance = float.MaxValue;

        foreach (Room otherRoom in roomList)
        {
            if (otherRoom == room) continue;

            float distance = Vector2.Distance(new Vector2(room.CenterX, room.CenterY), new Vector2(otherRoom.CenterX, otherRoom.CenterY));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestRoom = otherRoom;
            }
        }
        return closestRoom;
    }


    private bool RoomIntersects(Room room)
    {
        foreach (Room r in rooms)
        {
            if (room.Intersects(r))
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnPlayer()
    {
        playerRoom = rooms[random.Next(rooms.Count)];
        Vector3 playerPos = new Vector3(-width / 2 + playerRoom.CenterX, -height / 2 + playerRoom.CenterY, 0);
        Instantiate(playerPrefab, playerPos, Quaternion.identity, transform);
    }

    private void PlaceEnemies(int[,] mapData)
    {
        foreach (Room room in rooms)
        {
            if (room == playerRoom)
            {
                continue; // Skip the player's room
            }

           

            foreach (var config in enemyConfigurations)
            {
                int enemyCount = random.Next(config.minCount, config.maxCount + 1);
                for (int i = 0; i < enemyCount; i++)
                {
                    int enemyX = random.Next(room.X + 1, room.X + room.Width - 1);
                    int enemyY = random.Next(room.Y + 1, room.Y + room.Height - 1);

                    Vector3 enemyPos = new Vector3(-width / 2 + enemyX, -height / 2 + enemyY, 0);

                    

                    GameObject enemyInstance = Instantiate(config.enemyPrefab, enemyPos, Quaternion.identity, transform);
                    
                }
            }
        }

        // Place walls in corridors
        List<Vector2Int> emptyTiles = map.GetAllEmptyTiles();
        foreach (var tile in emptyTiles)
        {
            if (random.Next(0, 100) < 5) // 5% chance to place a wall
            {
                Vector3 wallPos = new Vector3(-width / 2 + tile.x, -height / 2 + tile.y, 0);
                Instantiate(wallPrefab, wallPos, Quaternion.identity, transform);
            }
        }
    }

    private void DrawMap(int[,] mapData)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(-width / 2 + x, -height / 2 + y, 0);
                GameObject tile = mapData[x, y] == 1 ? wallPrefab : floorPrefab;
                InstantiateTile(tile, pos);
            }
        }
    }

    private void InstantiateTile(GameObject prefab, Vector3 position)
    {
        GameObject tile = Instantiate(prefab, position, Quaternion.identity, transform);
       
        tile.SetActive(true);
    }
}
