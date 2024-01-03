using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MoonMan : MonoBehaviour
{
    [SerializeField]
    private float punchStopTime = 3f;

    [SerializeField]
    private float beforePunchwaitTime = 3f;

    [SerializeField]
    private float rotateSpeed = 1.0f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(PunchCoroutine());
    }

    private IEnumerator PunchCoroutine()
    {
        yield return new WaitForSeconds(beforePunchwaitTime);

        _animator.SetTrigger("PunchTrigger");

        yield return new WaitForSeconds(30f);

        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //주겨
            Debug.Log("플레이어 충돌");
        }
    }

    private void OnAnimationStopEvent()
    {
        StartCoroutine(AniStopCorou());
    }

    private IEnumerator AniStopCorou()
    {
        _animator.speed = 0.0f;

        float curTime = 0;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(PlayerManager.Instance.Player.transform.position);

        while (curTime <= punchStopTime)
        {
            Debug.Log(transform.rotation);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curTime);
            curTime += Time.deltaTime * rotateSpeed;
            yield return null;
        }
        _animator.speed = 1.0f;
    }
}
