using UnityEngine;

public class Satellite : SpawnObstacle
{
    public ParticleLifeTimer _particle;
    protected override void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        speed = MaxSpeed; //랜덤으로 스피드

        size = Random.Range(MinSize, MaxSize);
        transform.localScale = new Vector3(size, size, size);

        dir = -transform.forward;
        Vector3 directionToTarget = transform.position - Vector3.zero;
        //dir = directionToTarget;
        directionToTarget.y = 0;
        //dir.z = transform.position.z;


        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.right);

        float xAngle = lookRotation.eulerAngles.x;
        float yAngle = 270f;
        float zAngle = 90f;

        transform.Rotate(xAngle, yAngle, zAngle);

        damage = (int)size * (MaxDamage / MaxSize);
        lowerTem = (int)size * MaxLowerTem;
        lowerSpeed = (int)size * MaxLowerSpeed * 3;

    }
    protected override void Update()
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }
    public override void CollisonEvent(Collision player)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(lowerSpeed);

            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-damage);
            Debug.Log(damage);

            PlayerFollowCam.Instance.ShakeTest();
            var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
            obj.Setting(transform, size / 2f);

            PoolManager.Instance.Push(this);
        }
    }

    public override void Init()
    {

    }
}
