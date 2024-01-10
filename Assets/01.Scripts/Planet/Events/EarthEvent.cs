using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEvent : MonoBehaviour
{
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Earth, EnterEarthAreaEvent);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Earth);
        }
    }

    private void EnterEarthAreaEvent()
    {

        UIManager.Instance.PlanetUI.SetWarningText(PlanetEnum.Earth);
    }
}
