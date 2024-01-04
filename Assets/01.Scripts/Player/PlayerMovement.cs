using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] Target;
    private int curTargetIdx = 0;

    [SerializeField]
    private float startSpeed = 30f;
    [SerializeField]
    public float rotateSpeed = 100f;

    private Transform visualTrm;

    private float _speed = 0f;
    public float MoveSpeed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    private Coroutine setSpeed;
    public void SetSpeed(float duration, float value)
    {
        if (setSpeed != null)
            StopCoroutine(setSpeed);
        setSpeed = StartCoroutine(SetSpeedCorou(duration, value));
    }

    private IEnumerator SetSpeedCorou(float duration, float value)
    {
        MoveSpeed = value;
        yield return new WaitForSeconds(duration);
        MoveSpeed = startSpeed;
    }
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

        MoveSpeed = startSpeed;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // 이동
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(moveDirection * MoveSpeed * Time.deltaTime);

        Vector3 rotation = new Vector3(verticalInput, -horizontalInput, 0f) * rotateSpeed * Time.deltaTime;
        visualTrm.Rotate(rotation);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            visualTrm.rotation = Quaternion.Lerp(visualTrm.rotation, originQ, Time.deltaTime);
        }

        if (curTargetIdx <= 6)
        {
            if (-1 * (transform.position.z - Target[curTargetIdx].transform.position.z) < 10f) //높이만 거리를 잼
            {
                //Debug.Log(curTargetIdx);
                var type = (int)PlanetEnum.Neptune - curTargetIdx;
                if (type == -1)
                {
                    type = (int)PlanetEnum.EarthCloser;
                }
                PlanetEventManager.Instance.InvokePlanetEventHandler((PlanetEnum)type);
                ++curTargetIdx;
            }
        }
    }
}
