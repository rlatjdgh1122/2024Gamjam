using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAliens : MonoBehaviour
{
    private Transform _target;
    [SerializeField] Transform _firePos;
    [SerializeField] Tomato _bullet;
    private float distanceToTarget;

    private Vector3 dir;

    private bool canshoot = true;

    private void Start()
    {
        _target = PlayerManager.Instance.Player.transform;
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if(canshoot)
        {
            StartCoroutine(EnemySpawn());
        }
    }

    private void FixedUpdate()
    {
        dir = _target.position - transform.position;
        Vector3 moveDir = new Vector3(dir.x, transform.position.y, dir.z);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3);
    }

    private IEnumerator EnemySpawn()
    {
        Tomato bullet = PoolManager.Instance.Pop(_bullet.name) as Tomato;
        bullet.transform.position = _firePos.position;
        bullet.SetDir(dir.normalized);

        canshoot = false;
        yield return new WaitForSeconds(2f);
        canshoot = true;
    }
}
