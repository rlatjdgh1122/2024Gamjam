using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Image _panel;
    [SerializeField] private TextMeshProUGUI _curTex;
    [SerializeField] private float _delay;

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

            yield return new WaitForSeconds(storyList[i].StoryContent.Length * storyList[i].Delay + _delay);
        }

        _panel.DOFade(1, 2f).OnComplete(() => SceneManager.LoadScene(SceneName.InGame));

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
