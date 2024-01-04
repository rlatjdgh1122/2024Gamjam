using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : PoolableMono
{
    [SerializeField] float speed = 5f;

    Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    IEnumerator DestroyTomato()
    {
        yield return new WaitForSeconds(30f);
        PoolManager.Instance.Push(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 충돌처리 해라");
        }
        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {

    }

    public void SetDir(Vector3 dir)
    {
        _rigidbody.velocity = dir * speed;
        StartCoroutine(DestroyTomato());
    }
}
