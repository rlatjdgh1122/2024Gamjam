using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Earth, EnterEarthAreaEvent);
    }

    private void EnterEarthAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Earth);
        Debug.Log("지구 지나감");

    }
}
