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
        SpawnManager.Instance.Spawn(PlanetEnum.Mars - 1);

        UIManager.Instance.PlanetUI.SetWarningText(PlanetEnum.Mars);
    }
}
