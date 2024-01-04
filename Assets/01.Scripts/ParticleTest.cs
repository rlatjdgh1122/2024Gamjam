using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    public GameObject obj;
    public ParticleSystem ps;
    public float delay = 1f;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            StartCoroutine(Corou());
        }
    }

    private IEnumerator Corou()
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(delay);
        ps.time = 0;
        ps.Play();
        obj.SetActive(false);
    }
}
