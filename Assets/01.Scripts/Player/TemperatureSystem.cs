using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSystem : MonoBehaviour
{
    [SerializeField] private Material _iceMat;
    [SerializeField] private Material _hotMat;

    public bool ice;
    public bool hot;

    private void Awake()
    {
        _iceMat.SetInt("_Freezing", ice ? 1 : 0);
        _hotMat.SetInt("_Freezing", hot ? 1 : 0);
    }

    public void ScreenToIce(float temperature)
    {
        _iceMat.SetInt("_Freezing", ice ? 1 : 0);

        temperature = Mathf.Clamp(temperature, 3.5f, 50);

        _iceMat.SetFloat("_Sides", temperature);
    }

    public void ScreenToHot(float temperature)
    {
        _hotMat.SetInt("_Freezing", hot ? 1 : 0);

        temperature = Mathf.Clamp(temperature, 3.5f, 50);

        _hotMat.SetFloat("_Sides", temperature);
    }
}
