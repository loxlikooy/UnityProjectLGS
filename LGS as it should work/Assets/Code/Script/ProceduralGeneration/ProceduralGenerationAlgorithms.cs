
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        var previousPosition = startPosition;
        path.Add(previousPosition);

        for (int i = 0; i <walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;

            // Fill in gaps for diagonal moves
            var direction = newPosition - previousPosition;
            if (Mathf.Abs(direction.x) > 1 || Mathf.Abs(direction.y) > 1)
            {
                Vector2Int intermediatePosition = previousPosition + new Vector2Int(Mathf.Clamp(direction.x, -1, 1), Mathf.Clamp(direction.y, -1, 1));
                path.Add(intermediatePosition);
            }
        }

        return path;
    }
}

private static void FillGap(HashSet<Vector2Int> path, Vector2Int from, Vector2Int to)
{
    Vector2Int diff = to - from;
    if (Mathf.Abs(diff.x) > 1 || Mathf.Abs(diff.y) > 1)
    {
        Vector2Int step = new Vector2Int(Mathf.Clamp(diff.x, -1, 1), Mathf.Clamp(diff.y, -1, 1));
        Vector2Int intermediatePosition = from + step;
        path.Add(intermediatePosition);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>()
    {
        new Vector2Int(0, 1), // UP
        new Vector2Int(1, 0), // RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) // LEFT
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}