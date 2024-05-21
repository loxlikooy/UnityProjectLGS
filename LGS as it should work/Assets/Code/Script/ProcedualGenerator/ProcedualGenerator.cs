using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    [SerializeField] private int maxObjectCount;
    [SerializeField] private int minObjectCount;

    [SerializeField] private int corridorMinWidth;
    [SerializeField] private int corridorMaxWidth;

    [SerializeField] private Tilemap collidableTilemap;
    [SerializeField] private Tilemap nonCollidableTilemap;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase[] corridorTiles;
    [SerializeField] private TileBase[] roomTiles;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private List<EnemyConfigurationSO> enemyConfigurations;

    [SerializeField] private List<GameObject> objectPrefabs;
    [SerializeField] private GameObject chestPrefab;

    private Map map;
    private List<Room> rooms;
    private System.Random random;
    private Room playerRoom;
    private int[,] mapData;

    private const float CellSize = 0.64f;

    private void Start()
    {
        InitializeRandomSeed();
        GenerateMap();
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
        mapData = map.GenerateMap(randomFillPercent);
        rooms = new List<Room>();

        GenerateRoomsAndCorridors(mapData);
        EnsureAllRoomsConnected(mapData);
        DrawMap(mapData);
        SpawnPlayer();
        PlaceEnemies(mapData);
        PlaceObjectsAndChestsInRooms();
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
                        mapData[x + wx, y + wy] = -1;
                    }
                }
            }

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

        GameObject roomObject = new GameObject("Room");
        Room newRoom = roomObject.AddComponent<Room>();
        newRoom.Initialize(roomX, roomY, roomWidth, roomHeight, random, objectPrefabs, minObjectCount, maxObjectCount, chestPrefab);

        if (forceAdd || !RoomIntersects(newRoom))
        {
            rooms.Add(newRoom);
            newRoom.Carve(mapData);
            newRoom.PlaceChest(); // Place the chest inside the room
            roomObject.transform.SetParent(transform);
        }
    }





    private void EnsureAllRoomsConnected(int[,] mapData)
    {
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
        Vector3 playerPos = new Vector3((playerRoom.CenterX - width / 2f) * CellSize, (playerRoom.CenterY - height / 2f) * CellSize, 0);
        playerPos = new Vector3(playerRoom.CenterX * CellSize, playerRoom.CenterY * CellSize, 0);
        Instantiate(playerPrefab, playerPos, Quaternion.identity, transform);
    }

    private void PlaceEnemies(int[,] mapData)
    {
        List<Vector2Int> corridorPositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (IsCorridorTile(x, y))
                {
                    corridorPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        foreach (var config in enemyConfigurations)
        {
            int enemyCount = random.Next(config.minCount, config.maxCount + 1);

            for (int i = 0; i < enemyCount; i++)
            {
                if (corridorPositions.Count == 0)
                {
                    break;
                }

                int index = random.Next(corridorPositions.Count);
                Vector2Int pos = corridorPositions[index];

                corridorPositions.RemoveAt(index);

                if (random.Next(0, 100) < 20)
                {
                    Vector3 enemyPos = new Vector3(pos.x * CellSize, pos.y * CellSize, 0);
                    Instantiate(config.enemyPrefab, enemyPos, Quaternion.identity, transform);
                }
            }
        }
    }

    private void PlaceObjectsAndChestsInRooms()
    {
        foreach (Room room in rooms)
        {
            room.PlaceObjectsInRoom(transform);
        }
    }

    private void DrawMap(int[,] mapData)
    {
        collidableTilemap.ClearAllTiles();
        nonCollidableTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (mapData[x, y] == 1)
                {
                    collidableTilemap.SetTile(pos, wallTile);
                }
                else if (mapData[x, y] == -1)
                {
                    TileBase randomCorridorTile = corridorTiles[random.Next(corridorTiles.Length)];
                    nonCollidableTilemap.SetTile(pos, randomCorridorTile);
                }
            }
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            Room room = rooms[i];
            TileBase roomTile = roomTiles[i % roomTiles.Length];
            for (int x = room.X; x < room.X + room.Width; x++)
            {
                for (int y = room.Y; y < room.Y + room.Height; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    nonCollidableTilemap.SetTile(pos, roomTile);
                }
            }
        }
    }

    private bool IsCorridorTile(int x, int y)
    {
        return mapData[x, y] == -1;
    }
}
