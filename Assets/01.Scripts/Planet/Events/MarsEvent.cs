using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Mars, EnterMarsAreaEvent);
    }

    private void EnterMarsAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Mars);
        Debug.Log("화성 지나감");
    }
}
