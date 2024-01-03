using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    Vector3 dir;
    GameObject target;

    private void Start()
    {
        target = GameObject.Find("Target");
        dir = target.transform.position - transform.position;
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        StartCoroutine(DestroyTomato());
    }

    IEnumerator DestroyTomato()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == target)
        {
            Debug.Log("Ãæµ¹");
        }
    }
}
