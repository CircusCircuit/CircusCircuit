using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX
}

[System.Serializable]
public struct AudioClips
{
    //public string name; //�̸�
    public AudioClip clip; // ����� Ŭ��
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField]
    AudioSource[] audioSources
        = new AudioSource[System.Enum.GetValues(typeof(SoundType)).Length]; // ���� ���� ī��Ʈ
    [SerializeField] private List<AudioClips> BgmClip; // BGM
    [SerializeField] private List<AudioClips> SfxClip; // ȿ����

    public void Awake()
    {
        // Dont Destroy ����
        var obj = FindObjectsOfType<SoundManager>();
        if (obj.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // BGM loop ����
        audioSources[(int)SoundType.BGM].loop = true;

        instance = this;
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }

    public AudioClip GetAudioClip(string name, SoundType type = SoundType.SFX)
    {
        List<AudioClips> currentClipList = null;

        // SoundType Ȯ���ؼ� �����Ŭ�� ����Ʈ ����
        if (type == SoundType.BGM)
        {
            currentClipList = BgmClip;
        }
        else if (type == SoundType.SFX)
        {
            currentClipList = SfxClip;
        }
        else
        {
            Debug.LogWarning("Audio clip selection error!!!");
        }

        AudioClip currentClip = currentClipList.Find(x => x.clip.name.Equals(name)).clip;
        return currentClip;
    }

    public void Play(string name, float volume = 0.8f, SoundType type = SoundType.SFX)
    {
        AudioClip audioClip;
        try
        {
            audioClip = GetAudioClip(name, type);
        }
        catch
        {
            Debug.LogError("Can not get Audio Clip");
            return;
        }

        // Bgm
        if (type == SoundType.BGM)
        {
            AudioSource audioSource = audioSources[(int)SoundType.BGM];
            //Debug.Log(audioSource);
            if (audioSource.isPlaying) audioSource.Stop();

            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        // Sfx
        else if (type == SoundType.SFX)
        {
            AudioSource audioSource = audioSources[(int)SoundType.SFX];
            audioSource.volume = volume;
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogWarning("Error Playing an Audio Clip");
        }
    }
}