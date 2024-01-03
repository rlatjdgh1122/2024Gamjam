using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Jupiter, EnterJupiterAreaEvent);
    }

    private void EnterJupiterAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Jupiter - 1);
        Debug.Log("促澜 青己 : 格己");

    }
}
