using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeTimer : PoolableMono
{
    public float lifeTime = 2f;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        Init();
    }
    public void Setting(Transform trm, float size)
    {
        transform.position = trm.position;
        transform.localScale = new Vector3(size, size, size);
    }
    public void Setting(Vector3 pos, float size)
    {
        transform.position = pos;
        transform.localScale = new Vector3(size, size, size);
    }
    public override void Init()
    {
        ps.time = 0f;
        ps.Play();
        StartCoroutine(DestroyCorou());
    }
    private IEnumerator DestroyCorou()
    {
        yield return new WaitForSeconds(lifeTime);
        PoolManager.Instance.Push(this);
    }
}
