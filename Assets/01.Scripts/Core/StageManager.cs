using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SpawnManager is not NULL");
        }

        Instance = this;
    }

    public int CurrentStage = 0;

    public void MoveToNextStage()
    {
        UIManager.Instance.StageUIs[CurrentStage].EnableUI();
    }
}
