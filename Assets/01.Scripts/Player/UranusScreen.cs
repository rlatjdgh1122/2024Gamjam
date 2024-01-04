using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UranusScreen : MonoBehaviour
{
    public static UranusScreen Instance;

    [SerializeField] private BurningSystem _burningSystem;
    [SerializeField] private Material _iceMat;

    [SerializeField] private float _meltingSpeed;
    [SerializeField] private float _freezingSpeed;

    public float Temperature { get; private set; }
    public bool Ice { get; set; }
    public bool IceEventEnd { get; set; }
    public bool IceDeath { get; private set; }

    private float _maxValue = 50;

    private void Awake()
    {
        Instance = this;
        Temperature = _maxValue;

        _iceMat.SetInt("_Freezing", 0);
    }

    private void Update()
    {
        _iceMat.SetInt("_Freezing", Ice ? 1 : 0);

        if (Ice)
        {
            if (!_burningSystem.CanFire)
            {
                Temperature -= Time.deltaTime * _freezingSpeed;
            }
            else
            {
                Temperature += Time.deltaTime * _meltingSpeed;
            }
            Debug.Log(Temperature);

            Temperature = Mathf.Clamp(Temperature, 3.5f, _maxValue);

            _iceMat.SetFloat("_Sides", Temperature);
        }

        if(IceEventEnd)
        {
            Temperature += Time.deltaTime * _meltingSpeed;
            Temperature = Mathf.Clamp(Temperature, 3.5f, _maxValue);

            _iceMat.SetFloat("_Sides", Temperature);

            if(Temperature == _maxValue)
            {
                Ice = false;
            }
        }

        if(Temperature <= 3.5f)
        {
            IceDeath = true;
        }
    }
}
