using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private bool gamePaused;

    [SerializeField] private GameObject mainMenu;

    private void Start() {
        SwitchMenuTo(mainMenu);
        Time.timeScale = 1;
    }

    public void SwitchMenuTo(GameObject uiMenu) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        uiMenu.SetActive(true);
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
