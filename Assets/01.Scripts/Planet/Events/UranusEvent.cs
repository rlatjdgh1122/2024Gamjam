using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UranusEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Uranus, EnterUranusAreaEvent);
    }

    private void EnterUranusAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Uranus - 1);
        Debug.Log("다음 행성 : 천왕성");

    }
}
