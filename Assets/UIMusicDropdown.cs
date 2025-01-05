using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMusicDropdown : MonoBehaviour {
    [SerializeField] private TMP_Dropdown dropdown; // Gắn Dropdown vào đây trong Inspector
    [SerializeField] private AudioManager audioManager; // Gắn AudioManager vào đây trong Inspector

    private void Start() {
        SetupDropdownOptions();
        dropdown.onValueChanged.AddListener(OnMusicSelected); // Lắng nghe sự kiện chọn nhạc
    }

    private void SetupDropdownOptions() {
        // Xóa các tùy chọn cũ nếu có
        dropdown.ClearOptions();

        // Tạo danh sách các tên nhạc từ AudioManager
        List<string> musicNames = new List<string>();
        for (int i = 0; i < audioManager.BGMCount; i++) {
            musicNames.Add(audioManager.bgm[i].gameObject.name); // Đặt tên bài nhạc như "Track 1", "Track 2", ...
        }

        // Thêm danh sách vào Dropdown
        dropdown.AddOptions(musicNames);
    }

    private void OnMusicSelected(int index) {
        audioManager.PlayBGM(index); // Gọi AudioManager để phát nhạc theo chỉ số được chọn
    }
}
