using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlanetEnum
{
    Earth,
    Mars,
    Jupiter,
    Saturn,
    Uranus,
    Neptune
}

public class PlanetEventManager : MonoBehaviour
{
    public static PlanetEventManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("PlanetEventManager is multiple");
        }
        Instance = this;
    }

    private Dictionary<PlanetEnum, Action> _enterPlanetEventDic = new Dictionary<PlanetEnum, Action>();

    public void AddEvent(PlanetEnum planet, Action action)
    {
        _enterPlanetEventDic.Add(planet, action);
    }

    public void InvokePlanetEventHandler(PlanetEnum planet)
    {
        _enterPlanetEventDic[planet]?.Invoke();
    }
}
