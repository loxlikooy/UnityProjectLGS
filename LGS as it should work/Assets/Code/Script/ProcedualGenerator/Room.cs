using UnityEngine;

public class Room
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; }
    public int Height { get; }

    public int CenterX => X + Width / 2;
    public int CenterY => Y + Height / 2;

    private readonly System.Random random;

    public Room(int x, int y, int width, int height, System.Random random)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        this.random = random;
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
                if (random.NextDouble() < 0.1) // 20% chance to make a part of the room irregular
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
                    map[x, y] = 0;
                }
            }
        }
    }
}