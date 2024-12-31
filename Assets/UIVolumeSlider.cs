using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string audioPara;
    [SerializeField] private float multiplier = 25;

    public void SetupSlider() {
        slider.onValueChanged.AddListener(ChangeSliderValue);
        slider.minValue = .001f;
        slider.value = PlayerPrefs.GetFloat(audioPara, slider.value);
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat(audioPara, slider.value);
    }
    private void ChangeSliderValue(float value) {
        audioMixer.SetFloat(audioPara, Mathf.Log10(value) * multiplier);
     }
}
