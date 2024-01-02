using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToForward : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }
}
