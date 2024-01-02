using System.Collections;
using UnityEngine;

public class BuringSystem : MonoBehaviour
{
    private CharacterController characterController;
    private bool wasGroundedLastFrame;
    private bool isGrounded => characterController.isGrounded;


    [SerializeField]
    private float maxBurningValue;
    private float burningValue;
    public float BurningValue
    {
        get
        {
            return burningValue;
        }
        set
        {
            burningValue = Mathf.Clamp(value, 0, maxBurningValue);
        }
    }

    private Coroutine burningCoroutine;

    private void Start()
    {
        characterController = transform.parent.GetComponent<CharacterController>();
        wasGroundedLastFrame = isGrounded;
    }

    private void Update()
    {
        if (wasGroundedLastFrame && !isGrounded)
        {
            burningCoroutine = StartCoroutine(BuringCorou());
        }

        if (isGrounded && burningCoroutine != null)
        {
            StopCoroutine(burningCoroutine);
        }

        wasGroundedLastFrame = isGrounded;
    }

    private IEnumerator BuringCorou()
    {
        while(BurningValue <= 10.0f)
        {
            BurningValue += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        burningCoroutine = null;
    }

    public void DecreaseBurningValue(float minusValue)
    {
        BurningValue -= minusValue;
    }
}
