using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFowardMover : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 100f;
    private float _speed = 0;
    public float MoveSpeed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = Mathf.Clamp(value, 1f, _maxSpeed);
        }
    }

    public bool IsEnable = true;
    public bool IsSpeed = true;

    private void Start()
    {
        MoveSpeed = 1;
    }
    public void ApplySpeed(int value)
    {
        IsSpeed = true;
        MoveSpeed -= value;
    }
    private void Update()
    {
        if (IsSpeed)
        {
            MoveSpeed += 10f * Time.deltaTime;

            if (MoveSpeed >= _maxSpeed)
            {
                IsSpeed = false;
            }
        }


        if (IsEnable)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        }
    }
}
