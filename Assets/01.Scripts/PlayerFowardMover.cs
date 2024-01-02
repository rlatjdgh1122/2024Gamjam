using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFowardMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    public bool IsEnable = true;

    private void Update()
    {
        if (IsEnable)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
        }  
    }
}
