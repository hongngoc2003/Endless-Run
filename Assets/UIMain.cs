using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private bool gamePaused;

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Start() {
        SwitchMenuTo(mainMenu);
        Time.timeScale = 1;

        lastScoreText.text = "Last score: " + PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highScoreText.text = "High score: " + PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }

    public void SwitchMenuTo(GameObject uiMenu) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        uiMenu.SetActive(true);

        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }

    public void StartGame() => GameManager.Instance.UnlockPlayer();
    public void PauseGame() {
        if(gamePaused) {
            Time.timeScale = 1;
            gamePaused = false;
        } else {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGame() => GameManager.Instance.RestartLevel();
}
