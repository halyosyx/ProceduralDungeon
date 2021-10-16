using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapGraphics tileMapGraphics = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tileMapGraphics.Clear();
        
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();

    
}
