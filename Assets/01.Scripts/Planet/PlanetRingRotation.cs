using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRingRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _dir;

    private void Update()
    {
        transform.Rotate(new Vector3(0, _dir, 0) * Time.deltaTime * _rotationSpeed);
    }
}
