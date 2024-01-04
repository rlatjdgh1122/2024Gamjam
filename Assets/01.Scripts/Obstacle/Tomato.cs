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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("EHd");
            SoundManager.Instance.PlaySFXSound(SFX.SmallExplosion);
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(1);
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-0.05f);
            PlayerFollowCam.Instance.ShakeTest();
            Debug.Log("�÷��̾� �浹ó�� �ض�");
        }

        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {

    }

    public IEnumerator SetDir(Vector3 dir)
    {
        float elapsedTime = 0;

        Vector3 startPos = transform.position;

        float randomDistance = 10;

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 5f;
            transform.position = Vector3.Lerp(startPos, startPos + (dir * randomDistance), t);
            yield return null;
        }

        StartCoroutine(DestroyTomato());
    }
}
