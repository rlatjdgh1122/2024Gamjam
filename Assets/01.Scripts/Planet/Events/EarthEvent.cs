using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject _MoonMan;

    [SerializeField]
    private Transform _moonManSpawnPoint;

    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Earth, EnterEarthAreaEvent);
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.O))
        //{
        //    PlanetEventManager.Instance.InvokePlanetEventHandler(PlanetEnum.Earth);
        //}
    }

    private void EnterEarthAreaEvent()
    {
        SpawnManager.Instance.Spawn(PlanetEnum.Earth);
        Debug.Log("지구 지나감");

        GameObject moonMan = Instantiate(_MoonMan, _moonManSpawnPoint.position, Quaternion.Euler(0,180,0));

    }
}
