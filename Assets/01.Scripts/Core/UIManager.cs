using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SpawnManager is not NULL");
        }

        Instance = this;
    }

    public List<StageUI> StageUIs;
    public DieReasonUI DieReasonUI;
    public PlanetTextUI PlanetUI;
}
