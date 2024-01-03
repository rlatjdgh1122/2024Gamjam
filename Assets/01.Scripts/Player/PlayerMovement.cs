using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    public Transform Target;
    public float ReachedTime;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    public float rotateSpeed = 100f;

    private Transform visualTrm;

    float horizontalInput;
    float verticalInput;

    private bool canTimer = true;

    Quaternion originQ;

    private void Awake()
    {
        visualTrm = transform.Find("Visual").transform;
    }

    private void Start()
    {
        originQ = visualTrm.rotation;

        Debug.Log($"{Vector3.Distance(transform.position, Target.transform.position)}");
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // �̵�
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Vector3 rotation = new Vector3(verticalInput, -horizontalInput, 0f) * rotateSpeed * Time.deltaTime;
        visualTrm.Rotate(rotation);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            visualTrm.rotation = Quaternion.Lerp(visualTrm.rotation, originQ, Time.deltaTime);
        }


        if (Vector3.Distance(transform.position, Target.transform.position) < 10f)
        {
            canTimer = false;
        }
        
        if (canTimer)
        {
            ReachedTime += Time.deltaTime;
        }
    }
}
