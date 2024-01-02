using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToForward : MonoBehaviour
{
    [SerializeField] private float MaxSpeed;

    private float _speed;
    private float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = Mathf.Clamp(value, 1f, MaxSpeed);
        }
    }

    public void Apply(float speed)
    {

    }
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }
}
