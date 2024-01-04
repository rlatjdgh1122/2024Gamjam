using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //2370
            transform.position = new Vector3(0, 0, 2370);
            PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Uranus);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //4450
            transform.position = new Vector3(0, 0, 4450);
            PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Saturn);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //6700
            transform.position = new Vector3(0, 0, 6700);
            PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Jupiter);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            //9980
            transform.position = new Vector3(0, 0, 9980);
            PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Mars);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //11710
            transform.position = new Vector3(0, 0, 11710);
            PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Earth);
        }
    }
}
