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

    [SerializeField]
    private float empytTest = 0;

    protected override void RunProceduralGeneration()
    {
        HallwayGeneration();
    }

    private void HallwayGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();


        CreateHallway(floorPositions, potentialRoomPositions);
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = SearchDeadEnds(floorPositions);

        GenerateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        tileMapGraphics.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapGraphics);
    }

    private void GenerateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
        foreach (var position in deadEnds)
        {
            if (roomPositions.Contains(position) == false)
            {
                var roomPosition = RunRandomWalk(randomWalkParameters, position);
                roomPositions.UnionWith(roomPosition);
            }
        }
    }

    private List<Vector2Int> SearchDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;

            foreach (var direction in Direct2D.cardinalDirectionList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }

            if (neighboursCount == 1)
            {
                deadEnds.Add(position);
                
            }
        }
        return deadEnds;
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
