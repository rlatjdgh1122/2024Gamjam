using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlanetEnum
{
    Earth, //지구
    Mars, //화성
    Jupiter, //목성
    Saturn, //토성
    Uranus, //천왕성
    Neptune, //해왕성
    EarthCloser, //지구에 일정이상 가까워질때
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
