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
    //»´µû±¸ Ã³¸Â±â¶û »ç±Í´Â°Å Ãß°¡ ÇØ¾ß´ï¤¤
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

    private void Awake()
    {
        int idx = 0;
        foreach(DieReasonType dieReason in Enum.GetValues(typeof(DieReasonType)))
        {
            _dieReasonDic.Add(dieReason, _dieReasonInfoList[idx++]);
        }
    }

    public void UpdateIMG(DieReasonType dieReason)
    {
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
