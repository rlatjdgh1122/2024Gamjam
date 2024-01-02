using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public int spawnCount = 10;
    public SpawnObstacle[] obstacleSpawnPoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SpawnManager is not NULL");
        }

        Instance = this;
    }
    public void Spawn()
    {
        var name = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Length)].name;
        var obj = PoolManager.Instance.Pop(name) as SpawnObstacle;
        obj.Spawn();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < obstacleSpawnPoints.Length; ++i)
            {
                for (int j = 0; j < obstacleSpawnPoints.Length; ++j)
                    obstacleSpawnPoints[i].Spawn();
            }
        }

    }
}
