using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEvent : MonoBehaviour
{
    [SerializeField]
    private Moon _moon;

    Transform playerTrm;

    private void Start()
    {
        PlanetEventManager.Instance.AddEvent(PlanetEnum.Earth, EnterEarthAreaEvent);

        playerTrm = PlayerManager.Instance.Player.transform;
    }

    private void EnterEarthAreaEvent()
    {
        StartCoroutine(SpawnMoonCorou());
    }

    private IEnumerator SpawnMoonCorou()
    {
        for (int i = 0; i < 1; i++)
        {
            int ran = Random.Range(10, 170);
            float x = Mathf.Cos(ran * Mathf.Deg2Rad);
            Vector3 pos = transform.position + new Vector3(playerTrm.position.x + x, playerTrm.position.y, playerTrm.position.z + 5000f);

            Moon moon = PoolManager.Instance.Pop(_moon.name) as Moon;

            moon.transform.position = pos;

            yield return new WaitForSeconds(2f);
        }
    }
}
