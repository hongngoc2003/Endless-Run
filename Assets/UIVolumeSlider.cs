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

    private void Awake() {
        SetupSlider();
    }

    // Hàm thiết lập Slider
    public void SetupSlider() {
        // Đọc giá trị từ PlayerPrefs (giá trị mặc định là 0.5)
        float savedValue = PlayerPrefs.GetFloat(audioPara, 0.5f);

        // Gán giá trị cho Slider và đồng bộ với AudioMixer
        slider.minValue = .001f;
        slider.value = savedValue;
        audioMixer.SetFloat(audioPara, Mathf.Log10(savedValue) * multiplier);

        // Lắng nghe sự kiện thay đổi giá trị
        slider.onValueChanged.AddListener(ChangeSliderValue);
    }

    // Hàm xử lý khi thay đổi giá trị Slider
    private void ChangeSliderValue(float value) {
        // Cập nhật giá trị trong AudioMixer
        audioMixer.SetFloat(audioPara, Mathf.Log10(value) * multiplier);

        // Lưu giá trị vào PlayerPrefs
        PlayerPrefs.SetFloat(audioPara, value);
    }
}
