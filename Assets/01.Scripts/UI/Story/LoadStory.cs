using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class Story
{
    [TextArea] public string StoryContent;
    public float Delay;
    public Sprite Img;
}

public class LoadStory : MonoBehaviour
{
    [SerializeField] private Image _curSprite;
    [SerializeField] private TextMeshProUGUI _curTex;

    public List<Story> storyList = new();

    private string text;

    private void Start()
    {
        StartCoroutine(TextOuput());
    }

    private IEnumerator TextOuput()
    {
        for (int i = 0; i < storyList.Count; ++i)
        {
            _curSprite.sprite = storyList[i].Img;
            _curTex.text = storyList[i].StoryContent;
            text = _curTex.text;
            _curTex.text = " ";

            StartCoroutine(textPrint(storyList[i].Delay));

            yield return new WaitForSeconds(storyList[i].StoryContent.Length * 0.125f + 2f);
        }
    }

    IEnumerator textPrint(float delay)
    {
        int count = 0;

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                _curTex.text += text[count].ToString();
                count++;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
