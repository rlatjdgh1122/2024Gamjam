using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UranusScreen : MonoBehaviour
{
    [SerializeField] private Material _iceMat;
    [SerializeField] private float _iceSpeed;
    public float Temperature { get; private set; }

    public static UranusScreen Instance;

    private float _maxValue = 50;

    public bool ice = false;

    private void Awake()
    {
        Instance = this;
        Temperature = _maxValue;

        _iceMat.SetInt("_Freezing", 0);
    }

    public void ResetProperty()
    {
        Temperature = _maxValue;
    }

    private void Update()
    {
        _iceMat.SetInt("_Freezing", ice ? 1 : 0);

        if (ice)
        {
            Temperature -= Time.deltaTime * _iceSpeed;

            Temperature = Mathf.Clamp(Temperature, 3.5f, _maxValue);

            _iceMat.SetFloat("_Sides", Temperature);
        }
    }
}
