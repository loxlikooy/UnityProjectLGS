using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Map
{
    private int Width { get; }
    private int Height { get; }
    private int[,] map;
    private System.Random random;
    

    public Map(int width, int height, System.Random random)
    {
        Width = width;
        Height = height;
        this.random = random;
        map = new int[width, height];
    }

    public int[,] GenerateMap(int randomFillPercent)
    {
        RandomFillMap(randomFillPercent);
        SmoothMapParallel();
        return map;
    }

    private void RandomFillMap(int randomFillPercent)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (IsBorder(x, y))
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = random.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }

    private void SmoothMapParallel()
    {
        int[,] newMap = new int[Width, Height];

        Parallel.For(0, Width, x =>
        {
            for (int y = 0; y < Height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    newMap[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    newMap[x, y] = 0;
            }
        });

        map = newMap;
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    public bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    private bool IsBorder(int x, int y)
    {
        return x == 0 || x == Width - 1 || y == 0 || y == Height - 1;
    }

    public int[,] GetMap()
    {
        return map;
    }

    public List<Vector2Int> GetAllEmptyTiles()
    {
        List<Vector2Int> emptyTiles = new List<Vector2Int>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (map[x, y] == 0)
                {
                    emptyTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptyTiles;
    }

    public bool IsConnected()
    {
        bool[,] visited = new bool[Width, Height];
        List<Vector2Int> roomTiles = new List<Vector2Int>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (map[x, y] == 0 && !visited[x, y])
                {
                    var room = FloodFill(x, y, visited);
                    if (room.Count > roomTiles.Count)
                    {
                        roomTiles = room;
                    }
                }
            }
        }

        foreach (var tile in roomTiles)
        {
            visited[tile.x, tile.y] = true;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (map[x, y] == 0 && !visited[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private List<Vector2Int> FloodFill(int startX, int startY, bool[,] visited)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Vector2Int tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (IsInMapRange(neighbourX, neighbourY) && !visited[neighbourX, neighbourY] && map[neighbourX, neighbourY] == 0)
                    {
                        visited[neighbourX, neighbourY] = true;
                        queue.Enqueue(new Vector2Int(neighbourX, neighbourY));
                    }
                }
            }
        }
        return tiles;
    }
}
