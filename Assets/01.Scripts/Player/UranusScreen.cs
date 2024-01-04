using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UranusScreen : MonoBehaviour
{
    public static UranusScreen Instance;

    [SerializeField] private BurningSystem _burningSystem;
    [SerializeField] private Material _iceMat;
    [SerializeField] private float _iceSpeed;

    public bool CanFire;

    public float Temperature { get; private set; }
    public bool Ice = false;
    public bool IceDeath = false;

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
            if (!CanFire)
            {
                Temperature -= Time.deltaTime * _iceSpeed;
            }
            else
            {
                Temperature += Time.deltaTime * _iceSpeed;
            }

            Temperature = Mathf.Clamp(Temperature, 3.5f, _maxValue);

            _iceMat.SetFloat("_Sides", Temperature);
        }

        if(Temperature <= 3.5f)
        {
            IceDeath = true;
        }
    }
}
