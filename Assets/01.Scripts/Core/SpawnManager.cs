using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public SpawnObstacle[] obstacleSpawnPoints;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("SpawnManager is not NULL");
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < obstacleSpawnPoints.Length; ++i)
            {
                obstacleSpawnPoints[i].Spawn();
            }
        }
        
    }
}
