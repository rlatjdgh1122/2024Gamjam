using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeptuneEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Neptune, EnterNeptuneAreaEvent);
    }

    private void EnterNeptuneAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Neptune - 1);
        Debug.Log("다음 행성 : 해왕성");
    }
}
