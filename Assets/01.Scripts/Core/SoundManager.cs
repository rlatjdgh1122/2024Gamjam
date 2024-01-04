using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    Clap,
    Freeze,
    Fire,
    GetItem,
    HeavyExplosion,
    SmallExplosion,
    ImojiSound,
    MoonMan,
    Rocket,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] _sfxSounds; //���� ����Ʈ��
    [SerializeField] private AudioClip _backgroundSound; //��� ����

    Dictionary<string, AudioClip> _audioClipsDisc; //���� �������� ��ųʸ��� ����
    Dictionary<AudioClip, AudioSource> _audioClipToAudioSourceDisc; //���� �������� ��ųʸ��� ����
    AudioSource _bgmPlayer; //��� ���� �÷��̾�
    AudioSource _sfxPlayer; //���� ����Ʈ �÷��̾�

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SoundManager is multiple");
        }

        Instance = this;

        _bgmPlayer = transform.Find("BGMPlayer").GetComponent<AudioSource>();
        _sfxPlayer = transform.Find("SFXPlayer").GetComponent<AudioSource>();

        _audioClipsDisc = new Dictionary<string, AudioClip>();
        _audioClipToAudioSourceDisc = new();

        foreach (AudioClip clip in _sfxSounds)
        {
            GameObject audioSrObj = new GameObject(clip.name);
            audioSrObj.transform.parent = transform.Find("SFXCollection");
            AudioSource audioSr = audioSrObj.AddComponent<AudioSource>();

            audioSr.clip = clip;

            _audioClipsDisc.Add(clip.name, clip); //�迭�� �ִ� ���� ����Ʈ���� ��ųʸ��� ��� �߰�����
            _audioClipToAudioSourceDisc.Add(clip, audioSr);
        }
    }
    private void Start()
    {
        PlayBGMSound();
    }

    public void PlaySFXSound(SFX sfx) //���� ����Ʈ ��� �� ��ųʸ��� �ִ� Ŭ���� �̸����� Ư���Ͽ� ���
    {
        if (!_audioClipsDisc.ContainsKey(sfx.ToString()))
        {
            Debug.Log($"{sfx.ToString()} is not Contained at audioClipsDisc");
            return;
        }

        var clip = _audioClipsDisc[sfx.ToString()];
        _audioClipToAudioSourceDisc[clip].Play();
    }

    public void PauseSFXSound(SFX sfx)
    {
        var clip = _audioClipsDisc[sfx.ToString()];
        var audio = _audioClipToAudioSourceDisc[clip];
        StartCoroutine(FadeOut(audio));
    }

    IEnumerator FadeOut(AudioSource audio)
    {
        float startVolume = audio.volume; // ����� ���� ���� ����

        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / .4f; // �ð��� ���� ������ ������ ���ҽ�Ŵ
            yield return null;
        }

        audio.Stop(); // ������� ������Ŵ
        audio.volume = startVolume; // ���� �������� �ʱ�ȭ
    }

    public void PlayBGMSound()
    {
        _bgmPlayer.clip = _backgroundSound;
        _bgmPlayer.Play();
    }

    public void SetVolumeSFX(float volume)
    {
        _sfxPlayer.volume = volume;

        foreach (var item in _audioClipToAudioSourceDisc)
        {
            var sr = item.Value;
            sr.volume = volume;
        }
    }

    public void SetVolumeBGM(float volume)
    {
        _bgmPlayer.volume = volume;
    }

    public AudioSource GetAudioSource(string clip_name)
    {
        if (!_audioClipsDisc.ContainsKey(clip_name))
        {
            Debug.Log($"{clip_name} is not Contained at audioClipsDisc");
            return null;
        }

        var clip = _audioClipsDisc[clip_name];
        return _audioClipToAudioSourceDisc[clip];
    }
}