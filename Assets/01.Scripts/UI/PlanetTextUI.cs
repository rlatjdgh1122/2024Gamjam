using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

[System.Serializable]
public struct TextList
{
    public PlanetEnum planetType;
    public string planetTxt;
    public Color textCol;
    public string warningTxt;
}
public class PlanetTextUI : MonoBehaviour
{
    public TextMeshProUGUI planetText;
    public TextMeshProUGUI warningText;

    public List<TextList> textList = new List<TextList>();

    private Dictionary<PlanetEnum, TextList> textDic = new Dictionary<PlanetEnum, TextList>();


    private Coroutine corou = null;
    private void Awake()
    {
        textList.ForEach(p =>
        {
            textDic.Add(p.planetType, new TextList
            {
                planetTxt = p.planetTxt,
                textCol = p.textCol,
                warningTxt = p.warningTxt
            });
        });
    }

    public void SetWarningText(PlanetEnum type)
    {
        var texts = textDic[type];
        if (corou != null)
        {
            StopCoroutine(corou);
        }
        corou = StartCoroutine(AnimateText(texts));
    }

    private IEnumerator AnimateText(TextList texts)
    {
        planetText.text = texts.planetTxt;
        planetText.color = texts.textCol;

        warningText.text = texts.warningTxt;
        warningText.rectTransform.localScale = Vector3.one;

        warningText.DOFade(1f, 0.3f);
        yield return planetText.DOFade(1f, 0.3f).WaitForCompletion();

        Tween yoyoTween = warningText.transform.DOScale(1.3f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(1.3f);

        yield return new WaitForSeconds(1f);

        warningText.DOFade(0f, 0.3f);
        yield return planetText.DOFade(0f, 0.3f).WaitForCompletion();

        yoyoTween.Kill();
    }
}