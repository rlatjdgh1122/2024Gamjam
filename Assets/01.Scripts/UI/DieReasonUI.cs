using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DieReasonType
{
    LoseKeyBoardBattle,
    HitLocket,
    HitMeteor,
    HitMoonMan,
    HitEarth,
    FireDead,
    SmalleDead
    //������ ó�±�� ��ʹ°� �߰� �ؾߴ襤
}

[Serializable]
public struct DieRasonInFo
{
    public Sprite DieReasonIMG;
    public string DieReasonText;
}

public class DieReasonUI : MonoBehaviour
{
    [SerializeField]
    private List<DieRasonInFo> _dieReasonInfoList = new List<DieRasonInFo>();

    private Dictionary<DieReasonType, DieRasonInFo> _dieReasonDic = new Dictionary<DieReasonType, DieRasonInFo>();

    [SerializeField]
    private Image dieReasonIMG;
    [SerializeField]
    private TextMeshProUGUI dieReasonTMP;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        int idx = 0;
        foreach(DieReasonType dieReason in Enum.GetValues(typeof(DieReasonType)))
        {
            _dieReasonDic.Add(dieReason, _dieReasonInfoList[idx++]);
        }
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateIMG(DieReasonType dieReason)
    {
        _canvasGroup.DOFade(1, 1f);

        try
        {
            dieReasonIMG.sprite = _dieReasonDic[dieReason].DieReasonIMG;
            dieReasonTMP.SetText(_dieReasonDic[dieReason].DieReasonText);
        }
        catch
        {
            Debug.LogError("Cant Change Die ReasonInfo Sprite");
        }
    }
}
