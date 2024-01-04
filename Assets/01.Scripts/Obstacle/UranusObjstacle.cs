using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UranusObjstacle : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-Random.Range(.05f,.15f));
            PlayerFollowCam.Instance.ShakeTest();
        }
    }
}
