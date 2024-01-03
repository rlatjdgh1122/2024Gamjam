using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturnEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Saturn, EnterSaturnAreaEvent);
    }

    private void EnterSaturnAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Saturn - 1);
        Debug.Log("토성 지나감");

    }
}
