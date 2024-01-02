using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * _moveSpeed);
    }
}
