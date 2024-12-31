using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;


    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGame;

    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Volume sliders")]
    [SerializeField] private UIVolumeSlider[] slider;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image ingameMuteIcon;

    private void Start() {
        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].SetupSlider();
        }

        SwitchMenuTo(mainMenu);

        lastScoreText.text = "Last score: " + PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highScoreText.text = "High score: " + PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }

    public void SwitchMenuTo(GameObject uiMenu) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        uiMenu.SetActive(true);

        AudioManager.Instance.PlaySFX(8);

        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }

    public void MuteGame() {
        gameMuted = !gameMuted;

        if (gameMuted) {
            muteIcon.color = new Color(1, 1, 1, .5f);
            AudioListener.volume = 0;
        } else {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }

    public void StartGame() {
        muteIcon = ingameMuteIcon;

        if(gameMuted)
            muteIcon.color = new Color(1, 1, 1, .5f);

        GameManager.Instance.UnlockPlayer();
    }
    public void PauseGame() {
        if(gamePaused) {
            Time.timeScale = 1;
            gamePaused = false;
        } else {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGame() {
        GameManager.Instance.RestartLevel();
    }
    public void OpenEndGameUI() {
        SwitchMenuTo(endGame);
    }
}
