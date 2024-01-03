using System.Collections;
using UnityEngine;

public class Moon : PoolableMono
{
    [SerializeField]
    private float moveSpeed = 10f;

    private Vector3 targetVec;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        StartCoroutine(DestroyCorou());
    }

    private IEnumerator DestroyCorou()
    {
        float curTime = 0;
        while (curTime < 10f)
        {
            curTime += Time.deltaTime;
            targetVec = PlayerManager.Instance.Player.transform.position - transform.position;
            transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * moveSpeed);
            yield return null;
        }

        PoolManager.Instance.Push(this);
    }
}
