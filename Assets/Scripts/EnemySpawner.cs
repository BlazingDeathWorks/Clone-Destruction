using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy enemyToSpawn = null;
    [SerializeField]
    Tilemap tilemapToSpawnEnemies = null;
    private int enemiesSpawnPerWave = 1;
    private int enemiesKilledPerWave = 0;
    const int waveWaitTime = 3;

    private void Awake()
    {
        Invoke("SpawnFirstWave", waveWaitTime);
    }

    private void SpawnFirstWave()
    {
        SpawnEnemies();
    }

    private void SpawnNextWave()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyToSpawn == null) return;
        for(int i = 0; i < enemiesSpawnPerWave; i++)
        {
            InstantiateEnemy();
        }
    }

    private void InstantiateEnemy()
    {
        const float offsetValue = 0.5f;
        Vector3 walkableTilePosOffset = new Vector3(offsetValue, offsetValue);
        List<Vector3Int> walkableTilePos = new List<Vector3Int>();

        for (int x = tilemapToSpawnEnemies.cellBounds.xMin; x < tilemapToSpawnEnemies.cellBounds.xMax; x++)
        {
            for (int y = tilemapToSpawnEnemies.cellBounds.yMin; y < tilemapToSpawnEnemies.cellBounds.yMax; y++)
            {
                Vector3Int currentTile = new Vector3Int(x, y, 0);
                if (!tilemapToSpawnEnemies.HasTile(currentTile)) continue;
                walkableTilePos.Add(currentTile);
            }
        }

        Vector3 finalPos;
        Vector3 randomWalkablePosition = tilemapToSpawnEnemies.CellToWorld(walkableTilePos[Random.Range(0, walkableTilePos.Count)]);

        finalPos = randomWalkablePosition + walkableTilePosOffset;
        Mathf.Clamp(finalPos.x, randomWalkablePosition.x + offsetValue, randomWalkablePosition.x - offsetValue);
        Mathf.Clamp(finalPos.y, randomWalkablePosition.y + offsetValue, randomWalkablePosition.y - offsetValue);

        Enemy enemyInstance = Instantiate(enemyToSpawn, finalPos, Quaternion.identity);
        SetEnemyParent(enemyInstance);
    }

    public void AddToKillPerWave()
    {
        enemiesKilledPerWave++;
        if(enemiesKilledPerWave >= enemiesSpawnPerWave)
        {
            Invoke("SpawnNextWave", waveWaitTime);
            enemiesSpawnPerWave++;
            enemiesKilledPerWave = 0;
        }
    }

    private void SetEnemyParent(Enemy instance)
    {
        instance.transform.parent = transform;
    }
}
