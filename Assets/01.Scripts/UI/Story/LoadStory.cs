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
    [SerializeField] private TextMeshProUGUI _skipTex;
    [SerializeField] private float _delay;

    public List<Story> storyList = new();

    private string text;

    private bool isSkipText;

    private int _idx = 0;

    private Coroutine _coroutine;

    private void Start()
    {
        _coroutine = StartCoroutine(TextOuput(0));
        _skipTex.DOFade(0f, 2f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FinishStory();
        }
        else if (Input.anyKeyDown)
        {
            if (isSkipText)
            {
                isSkipText = false;

                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(TextOuput(_idx));
            }
            else
            {
                isSkipText = true;
            }
        }
    }

    private IEnumerator TextOuput(int idx)
    {
        for (int i = idx; i < storyList.Count; ++i)
        {
            _curSprite.sprite = storyList[i].Img;
            _curTex.text = storyList[i].StoryContent;
            text = _curTex.text;
            _curTex.text = " ";
            StartCoroutine(TextPrint(storyList[i].Delay));

            yield return new WaitForSeconds(storyList[i].StoryContent.Length * storyList[i].Delay + _delay);
        }

        FinishStory();
    }

    IEnumerator TextPrint(float delay)
    {
        int count = 0;

        while (count != text.Length)
        {
            if (isSkipText)
            {
                _curTex.SetText(" " + text);
                break;
            }

            if (count < text.Length)
            {
                _curTex.text += text[count].ToString();
                count++;
            }

            yield return new WaitForSeconds(delay);
        }
        _idx++;
    }

    private void FinishStory()
    {
        _panel.DOFade(1, 2f).OnComplete(() => SceneManager.LoadScene(SceneName.InGame));
    }
}
