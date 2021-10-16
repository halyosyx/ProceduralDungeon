using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapGraphics tilemapGraphics)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direct2D.cardinalDirectionList);
        foreach (var position in basicWallPositions)
        {
            tilemapGraphics.PaintSingleWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPosition = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPosition.Add(neighbourPosition);
                }
            }

        }

        return wallPosition;
    }
}