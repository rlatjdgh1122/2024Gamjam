using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometHit : MonoBehaviour
{
    GameObject Pluto;
    Vector3 dir;

    private void Start()
    {
        Pluto = GameObject.Find("Target");
        dir = Pluto.transform.position - transform.position;
    }

}
