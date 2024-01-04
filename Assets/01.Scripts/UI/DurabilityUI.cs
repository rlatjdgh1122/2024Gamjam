using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityUI : MonoBehaviour
{
    private Image _fill;

    bool dead = false;

    private void Awake()
    {
        _fill = GetComponent<Image>();
        _fill.fillAmount = PlayerManager.Instance.GetDurabilitySystem.MaxValue;

        IncreaseValue();
    }
    public void IncreaseValue()
    {
        StartCoroutine(Corou());
        //_fill.DOFillAmount(PlayerManager.Instance.GetDurabilitySystem.DurabilityValue, 0.75f);
    }

    private IEnumerator Corou()
    {
        float timer = 0f;
        float startValue = _fill.fillAmount;
        while (timer < .75f)
        {
            timer += Time.deltaTime;
            _fill.fillAmount = Mathf.Lerp(startValue, PlayerManager.Instance.GetDurabilitySystem.DurabilityValue, timer / .75f);

            yield return null;
        }
    }
    private void Update()
    {
        if (PlayerManager.Instance.IsDie && !dead)
        {
            _fill.DOFillAmount(0, 0.5f);
            dead = true;
        }
    }
}
