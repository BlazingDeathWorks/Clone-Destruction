using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class SimpleLevelGenerator : MonoBehaviour
{
    //SerializedFields
    [SerializeField]
    int iterations = 1;
    [SerializeField]
    int walkPerIteration = 1;

    [SerializeField]
    private Tilemap wallTilemap = null;
    [SerializeField]
    private Tilemap walkableTilemap = null;
    [SerializeField]
    private TileBase tile = null;
    [SerializeField]
    Vector2Int startPos;

    [SerializeField]
    GameObject playerObject = null;
    [SerializeField]
    EscapePod escapePodObject = null;
    [SerializeField]
    AstarPath astarPath = null;
    [SerializeField]
    FadeInOut fadeInOut = null;

    //Config Generator
    HashSet<Vector2Int> WalkablePath { get; set; } = new HashSet<Vector2Int>();
    Vector2Int currentPos;
    int errorsInARow = 0;

    //Direction Paramas
    Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) };
    const int DIRECTIONSIZE = 4;

    private void Start()
    {
        GenerateWalkables();
        GenerateWalls();
        SearchEscapePodPos();
        InstantiatePlayer();
        StartCoroutine(SimpleRescanAstar());
    }

    #region Walkable Generator
    private void GenerateWalkables()
    {
        for (int i = 0; i < DIRECTIONSIZE; i++)
        {
            CreateWalkPath();
        }
    }

    private void CreateWalkPath()
    {
        currentPos = startPos;
        for (int j = 0; j < iterations; j++)
        {
            int steps = 0;
            Vector2Int randomDirection = GenerateRandomDirection();
            do
            {
                Vector3Int setTilePos = (Vector3Int)currentPos + (Vector3Int)randomDirection;
                currentPos = currentPos + randomDirection;
                if (WalkablePath.Add(currentPos))
                {
                    walkableTilemap.SetTile(setTilePos, tile);
                    steps++;
                    ResetErrors();
                }
                else
                {
                    errorsInARow++;
                    j--;
                    steps = walkPerIteration;
                    currentPos = currentPos - randomDirection;
                }
            } while (steps < walkPerIteration);
            if (errorsInARow < DIRECTIONSIZE) continue;
            ResetErrors();
            RestartPos();
        }
    }
    #endregion

    #region Walls Generator
    private void GenerateWalls()
    {
        List<Vector3Int> wallableTilePos = new List<Vector3Int>();
        for (int x = walkableTilemap.cellBounds.xMin - 1; x < walkableTilemap.cellBounds.xMax + 1; x++)
        {
            for (int y = walkableTilemap.cellBounds.yMin - 1; y < walkableTilemap.cellBounds.yMax + 1; y++)
            {
                var currentTile = new Vector3Int(x, y, 0);
                if (walkableTilemap.HasTile(currentTile)) continue;
                wallableTilePos.Add(currentTile);
            }
        }
        foreach(Vector3Int wallTilePos in wallableTilePos)
        {
            wallTilemap.SetTile(wallTilePos, tile);
        }
    }
    #endregion

    #region Instantiate Player
    private void InstantiatePlayer()
    {
        const float offsetValue = 0.5f; 
        Vector3 walkableTilePosOffset = new Vector3(offsetValue, offsetValue);
        List<Vector3Int> walkableTilePos = new List<Vector3Int>();
        for (int x = walkableTilemap.cellBounds.xMin; x < walkableTilemap.cellBounds.xMax; x++)
        {
            for (int y = walkableTilemap.cellBounds.yMin; y < walkableTilemap.cellBounds.yMax; y++)
            {
                Vector3Int currentTile = new Vector3Int(x, y, 0);
                if (!walkableTilemap.HasTile(currentTile)) continue;
                walkableTilePos.Add(currentTile);
            }
        }
        Vector3 finalPos;
        Vector3 randomWalkablePosition = walkableTilemap.CellToWorld(walkableTilePos[Random.Range(0, walkableTilePos.Count)]);
        finalPos = randomWalkablePosition + walkableTilePosOffset;
        Mathf.Clamp(finalPos.x, randomWalkablePosition.x + offsetValue, randomWalkablePosition.x - offsetValue);
        Mathf.Clamp(finalPos.y, randomWalkablePosition.y + offsetValue, randomWalkablePosition.y - offsetValue);
        playerObject.transform.position = finalPos;
    }
    #endregion

    #region Instantiate Escape Pods
    private void SearchEscapePodPos()
    {
        Vector3 walkableTilePosOffset = new Vector3(0.5f, 0.5f);

        var walkableTilemapMin = walkableTilemap.cellBounds.xMin;
        var walkableTilemapMax = walkableTilemap.cellBounds.xMax;

        List<Vector3Int> walkableTilePos = new List<Vector3Int>();

        for (int y = walkableTilemap.cellBounds.yMin; y < walkableTilemap.cellBounds.yMax; y++)
        {
            Vector3Int currentTile = new Vector3Int(walkableTilemapMin, y, 0);
            if (walkableTilemap.HasTile(currentTile)) walkableTilePos.Add(currentTile);
        }

        InstantiateEscapePods(walkableTilePosOffset, walkableTilePos[0], walkableTilePos[walkableTilePos.Count - 1]);
        walkableTilePos = new List<Vector3Int>();

        for (int y = walkableTilemap.cellBounds.yMin; y < walkableTilemap.cellBounds.yMax; y++)
        {
            Vector3Int currentTile = new Vector3Int(walkableTilemapMax - 1, y, 0);
            if (walkableTilemap.HasTile(currentTile)) walkableTilePos.Add(currentTile);
        }

        InstantiateEscapePods(walkableTilePosOffset, walkableTilePos[0], walkableTilePos[walkableTilePos.Count - 1]);
    }

    private void InstantiateEscapePods(Vector3 positionsOffset, params Vector3Int[] positions)
    {
        foreach(Vector3Int position in positions)
        {
            Instantiate(escapePodObject, walkableTilemap.CellToWorld(position) + positionsOffset, Quaternion.identity);
        }
    }
    #endregion

    #region Generate Random Direction
    private Vector2Int GenerateRandomDirection()
    {
        return directions[Random.Range(0, DIRECTIONSIZE)];
    }
    #endregion

    #region Rescan Astar
    private IEnumerator SimpleRescanAstar()
    {
        if (astarPath != null)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(astarPath, new Vector3(0, 0, 0), Quaternion.identity);
        }
        fadeInOut.FadeIn();
    }
    #endregion

    #region Errors in a Row
    private void ResetErrors()
    {
        errorsInARow = 0;
    }

    private void RestartPos()
    {
        List<Vector3Int> walkableTilePos = new List<Vector3Int>();
        for(int x = walkableTilemap.cellBounds.xMin; x < walkableTilemap.cellBounds.xMax; x++)
        {
            for(int y = walkableTilemap.cellBounds.yMin; y < walkableTilemap.cellBounds.yMax; y++)
            {
                Vector3Int currentTile = new Vector3Int(x, y, 0);
                if (!walkableTilemap.HasTile(currentTile)) continue;
                walkableTilePos.Add(currentTile);
            }
        }
        currentPos = (Vector2Int)walkableTilePos[Random.Range(0, walkableTilePos.Count)];
    }
    #endregion

    #region For demo
    /*private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            RestartPos();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int steps = 0;
            Vector2Int randomDirection = GenerateRandomDirection();
            do
            {
                Vector3Int setTilePos = (Vector3Int)currentPos + (Vector3Int)randomDirection;
                currentPos = currentPos + randomDirection;
                if (WalkablePath.Add(currentPos))
                {
                    walkableTilemap.SetTile(setTilePos, tile);
                    steps++;
                    ResetErrors();
                }
                else
                {
                    errorsInARow++;
                    steps = walkPerIteration;
                    currentPos = currentPos - randomDirection;
                }
            } while (steps < walkPerIteration);
            if (errorsInARow < DIRECTIONSIZE) return;
            ResetErrors();
            RestartPos();
        }
    }*/
    #endregion
}
