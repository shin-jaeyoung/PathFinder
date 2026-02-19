using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] 
    private Slider masterSlider;
    [SerializeField] 
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;
    private void Start()
    {
        masterSlider.minValue = 0.0001f;
        masterSlider.maxValue = 1f;

        bgmSlider.minValue = 0.0001f;
        bgmSlider.maxValue = 1f;

        sfxSlider.minValue = 0.0001f;
        sfxSlider.maxValue = 1f;

        masterSlider.value = 1f;
        bgmSlider.value = 0.75f;
        sfxSlider.value = 0.75f;

        masterSlider.onValueChanged.AddListener(val =>
            GameManager.instance.SoundManager.SetVolume("MasterVol", val));

        bgmSlider.onValueChanged.AddListener(val =>
            GameManager.instance.SoundManager.SetVolume("BGMVol", val));
        sfxSlider.onValueChanged.AddListener(val =>
            GameManager.instance.SoundManager.SetVolume("SFXVol", val));
    }
    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.RemoveAllListeners();
    }
}
