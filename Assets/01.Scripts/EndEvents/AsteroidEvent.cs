using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidEvent : MonoBehaviour
{
    [SerializeField]
    private List<event_Asteroid> _asteroids;

    [SerializeField]
    private Transform _target;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) <= 900)
        {
            MoveAsteroids();
        }    
    }

    public void MoveAsteroids()
    {
        _asteroids.ForEach(a => a.gameObject.SetActive(true));
    }
}
