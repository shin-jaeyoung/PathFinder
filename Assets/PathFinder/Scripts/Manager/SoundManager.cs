using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BGMData
{
    public SceneType sceneType;
    public AudioClip audioClip;
}
public class SoundManager
{
    private AudioSource bgmPlayer;
    private Dictionary<SceneType, AudioClip> bgmDictionary = new Dictionary<SceneType, AudioClip>();
    private AudioMixer mixer;

    public void Init(AudioSource source, List<BGMData> bgmList, AudioMixer audioMixer)
    {
        bgmPlayer = source;
        mixer = audioMixer;

        foreach (var data in bgmList)
        {
            if (data.audioClip == null) continue;
            if (!bgmDictionary.ContainsKey(data.sceneType))
            {
                bgmDictionary.Add(data.sceneType, data.audioClip);
            }
        }
    }

    public void OnSceneLoaded(SceneType sceneType)
    {
        if (bgmDictionary.TryGetValue(sceneType, out AudioClip clip))
        {
            PlayBGM(clip);
        }
        else
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = null;
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        if (bgmPlayer.clip == clip) return;
        Debug.Log("BGM재생");
        bgmPlayer.Stop();
        bgmPlayer.clip = clip;
        bgmPlayer.loop = true;
        bgmPlayer.Play();
    }
    public void SetVolume(string parameterName, float sliderValue)
    {
        float volume = Mathf.Log10(Mathf.Max(0.0001f, sliderValue)) * 20;
        mixer.SetFloat(parameterName, volume);
    }
}
