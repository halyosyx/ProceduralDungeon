using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HallwayGenerator : RandomWalkGenerator
{
    [SerializeField]
    private int corridorLen = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        HallwayGeneration();
    }

    private void HallwayGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> roomPosition = new HashSet<Vector2Int>();


        CreateHallway(floorPositions, roomPosition);
        HashSet<Vector2Int> roomPositions = CreateRooms(roomPosition);

        floorPositions.UnionWith(roomPositions);

        tileMapGraphics.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapGraphics);
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> roomPosition)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(roomPosition.Count * roomPercent);

        // Sort room positions in random order
        List<Vector2Int> roomToCreate = roomPosition.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPos in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPos);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;


    }

    private void CreateHallway(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> roomPosition)
    {
        var currentPosition = startPosition;
        roomPosition.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var hallway = ProceduralAlgorithms.RandomWalkCorridor(currentPosition, corridorLen);
            currentPosition = hallway[hallway.Count - 1];
            roomPosition.Add(currentPosition);
            floorPositions.UnionWith(hallway);

        }
    }
}
