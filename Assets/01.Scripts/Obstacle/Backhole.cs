using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Backhole : MonoBehaviour
{
    public LayerMask mask;
    [Header("플레이어 영향 줄 스탯")]
    public int size = 30; //범위 크기 
    public int strength = 20; //힘

    [SerializeField] private Collider[] colls;
    private void FixedUpdate()
    {
        colls = Physics.OverlapSphere(transform.position, size, mask);
        if (colls.Length > 0)
        {
            foreach (Collider col in colls)
            {

                var rb = col.GetComponent<Rigidbody>(); //렉걸리면 여기 수정할거임
                if (rb != null)
                {
                    var obj = col.gameObject;
                    var objPos = obj.transform.position;
                    var dir = (transform.position - objPos).normalized;
                    var dis = dir.magnitude;

                    rb.AddForce(dir * dis);
                }
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        //삭제시킴
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
