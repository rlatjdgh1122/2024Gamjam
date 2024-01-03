using System;
using System.Collections;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField]
    private float waitTime = 3f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(PunchCoroutine());
    }

    private IEnumerator PunchCoroutine()
    {
        yield return new WaitForSeconds(waitTime);

        _animator.SetTrigger("PunchTrigger");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //주겨
            Debug.Log("플레이어 충돌");
        }
    }

}
