using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EarthCloserEvent : MonoBehaviour
{

    public UnityEvent OnEvent;
    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.EarthCloser, EnterEarthCloserAreaEvent);
    }

    private void EnterEarthCloserAreaEvent()
    {
        PlayerFollowCam.Instance.FollowCam.m_Follow = null;
        OnEvent?.Invoke();
    }
}
