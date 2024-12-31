using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lastScore;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI coins;

    private void Start() {

        if (GameManager.Instance.distance <= 0)
            return;

        if (GameManager.Instance.coins <= 0)
            return;

        lastScore.text = "Score: " + GameManager.Instance.score.ToString("#,#");
        bestScore.text = "Highscore: " + PlayerPrefs.GetFloat("HighScore").ToString("#,#");
        coins.text = "+" + GameManager.Instance.coins.ToString("#,#");
    }
}
