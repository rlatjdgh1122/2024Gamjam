using DG.Tweening;
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
    private Transform target;

    private Animator _animator;

    bool isReady = false;
    bool canPunch = false;

    Vector3 lookDirection;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= 1000f && !isReady)
        {
            StartCoroutine(PunchCoroutine());
            SoundManager.Instance.PlaySFXSound(SFX.MoonMan);
            isReady = true;
        }

        if (canPunch)
            PunchFoward();
    }

    private IEnumerator PunchCoroutine()
    {
        yield return new WaitForSeconds(beforePunchwaitTime);

        _animator.SetTrigger("PunchTrigger");

        yield return new WaitForSeconds(30f);

        Destroy(gameObject);
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
            // 타겟 방향으로 회전
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(new Vector3(targetDirection.x, 0, targetDirection.z));

            curTime += Time.deltaTime;

            yield return null;
        }

        lookDirection = new Vector3(target.transform.position.x, target.transform.position.y - 100f, target.transform.position.z);
        _animator.speed = .7f;
        canPunch = true;
    }


    public void PunchFoward()
    {
        transform.position = Vector3.MoveTowards(transform.position, lookDirection, 12f);
        //transform.DOMove(target.transform.position, 40f).SetEase(Ease.OutCirc);
    }
}
