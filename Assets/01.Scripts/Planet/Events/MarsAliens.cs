using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAliens : MonoBehaviour
{
    private Transform _target;
    [SerializeField] Transform _firePos;
    [SerializeField] Tomato _bullet;
    private float distanceToTarget;

    [SerializeField]
    private float _attackSpeed;
    [SerializeField]
    private float _rotateSpeed;

    [SerializeField]
    private float fireDistance;

    private Vector3 dir;

    private bool canshoot = true;

    private void Start()
    {
        _target = PlayerManager.Instance.Player.transform;
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if(fireDistance > distanceToTarget && canshoot)
        {
            StartCoroutine(EnemySpawn());
        }

        dir = _target.position - _firePos.position;
        Vector3 moveDir = new Vector3(dir.x, _firePos.position.y, dir.z);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
    }

    private IEnumerator EnemySpawn()
    {
        Tomato bullet = PoolManager.Instance.Pop(_bullet.name) as Tomato;
        bullet.transform.position = _firePos.position;
        bullet.SetDir(dir.normalized);

        canshoot = false;
        yield return new WaitForSeconds(_attackSpeed);
        canshoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireDistance);
    }
}
