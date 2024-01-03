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
        SpawnManager.Instance.Spawn(PlanetEnum.Neptune);
        Debug.Log("해왕성 지나감");
    }
}
