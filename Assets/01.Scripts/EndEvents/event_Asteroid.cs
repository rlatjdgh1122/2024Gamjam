using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_Asteroid : MonoBehaviour
{
    private float _moveSpeed;

    private void Awake()
    {
        _moveSpeed = Random.Range(20, 26);
    }

    private void Update()
    {
        transform.Translate(new Vector3(-1, -1, 0) * Time.deltaTime * _moveSpeed);
    }
}
