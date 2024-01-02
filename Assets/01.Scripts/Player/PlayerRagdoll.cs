using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField]
    private Transform ragdoll;

    public float value = 10f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return null;
            Vector3 randomVec = new Vector3(Mathf.Sin(Time.time) * value,
                                            Mathf.Cos(Time.time) * value,
                                            Mathf.Sin(Time.time) * value);
            ragdoll.Rotate(randomVec, 3f);
        }
    }
}
