using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("SoundManager Is Not Null");

        Instance = this;
    }

    public void Clip()
    {

    }
}
