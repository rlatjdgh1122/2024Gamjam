using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Backhole : MonoBehaviour
{
    public LayerMask mask;
    [Header("�÷��̾� ���� �� ����")]
    public int size = 30; //���� ũ�� 
    public int strength = 20; //��

    [SerializeField] private Collider[] colls;
    private void FixedUpdate()
    {
        colls = Physics.OverlapSphere(transform.position, size, mask);
        if (colls.Length > 0)
        {
            foreach (Collider col in colls)
            {

                var rb = col.GetComponent<Rigidbody>(); //���ɸ��� ���� �����Ұ���
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
        //������Ŵ
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
