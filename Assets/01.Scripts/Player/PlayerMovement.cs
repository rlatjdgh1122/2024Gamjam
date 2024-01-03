using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] Target;
    private int curTargetIdx = 0;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    public float rotateSpeed = 100f;

    private Transform visualTrm;

    float horizontalInput;
    float verticalInput;

    Quaternion originQ;


    private void Awake()
    {
        visualTrm = transform.Find("Visual").transform;
    }

    private void Start()
    {
        originQ = visualTrm.rotation;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // 이동
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Vector3 rotation = new Vector3(verticalInput, -horizontalInput, 0f) * rotateSpeed * Time.deltaTime;
        visualTrm.Rotate(rotation);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            visualTrm.rotation = Quaternion.Lerp(visualTrm.rotation, originQ, Time.deltaTime);
        }

        if (curTargetIdx <= 5)
        {
            if (-1 * (transform.position.z - Target[curTargetIdx].transform.position.z) < 10f) //높이만 거리를 잼
            {
                //Debug.Log(curTargetIdx);
                var type = (int)PlanetEnum.Neptune - curTargetIdx;
                PlanetEventManager.Instance.InvokePlanetEventHandler((PlanetEnum)type);
                ++curTargetIdx;
            }
        }
    }
}
