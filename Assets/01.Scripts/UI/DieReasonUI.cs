using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DieReasonType
{
    LoseKeyBoardBattle,
    HitLocket,
    HitMeteor,
    HitMoonMan,

}

public class DieReasonUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> _dieReasonImageList = new List<Image>();

    private Dictionary<DieReasonType, Image> _dieReasonDic = new Dictionary<DieReasonType, Image>();

    [SerializeField]
    private Image dieReasonIMG;

    private void Awake()
    {
        foreach(DieReasonType dieReason in Enum.GetValues(typeof(DieReasonType)))
        {
            _dieReasonDic.Add(dieReason, dieReasonIMG);
        }
    }

    public void UpdateIMG(DieReasonType dieReason)
    {
        try
        {
            dieReasonIMG.sprite = _dieReasonDic[dieReason].sprite;
        }
        catch
        {
            Debug.LogError("Cant Change Die Reason Image Sprite");
        }
    }
}
