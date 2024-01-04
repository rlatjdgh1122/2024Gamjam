using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : PoolableMono
{
    [SerializeField] float speed = 5f;

    private void Awake()
    {
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
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-3);
            Debug.Log("플레이어 충돌처리 해라");
        }
        //PoolManager.Instance.Push(this);
    }

    public override void Init()
    {
        
    }

    public IEnumerator SetDir(Vector3 dir)
    {
        float elapsedTime = 0;

        Vector3 startPos = transform.position;

        float randomDistance = 10;

        float startTime = Time.time;

        while (Time.time - startTime < 5f)
        {
            float t = (Time.time - startTime) / 5f;
            transform.position = Vector3.Lerp(startPos, dir * randomDistance, t);
            yield return null; // 다음 프레임까지 대기
        }

        //StartCoroutine(DestroyTomato());
    }
}
