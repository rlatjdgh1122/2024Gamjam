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

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //주겨
            Debug.Log("플레이어 충돌");
        }
    }

    public void OnAnimationStopEvent()
    {
        Debug.Log("dasda");
        StartCoroutine(AniStopCorou());
    }

    private IEnumerator AniStopCorou()
    {
        _animator.speed = 0.0f;

        float curTime = 0;

        while (curTime <= punchStopTime)
        {
            Vector3 lookDirection = PlayerManager.Instance.Player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = targetRotation;
            curTime += Time.deltaTime;

            yield return null;
        }
        _animator.speed = 1.0f;
    }
}
