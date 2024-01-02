using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Story
{
    [TextArea] public string StoryContent;
    public Sprite Img;
}

public class LoadStory : MonoBehaviour
{
    [SerializeField] private Image _curSprite;
    public List<Story> storyList = new();

    private void Update()
    {
        for(int i = 0; i < storyList.Count; ++i)
        {
            _curSprite.sprite = storyList[i].Img;
        }
    }
}
