using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    private Player player;

    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private float distance;
    private int coins;

    [SerializeField] private Image heartEmpty;
    [SerializeField] private Image heartFull;

    private void Start() {
        player = GameManager.Instance.player;
        InvokeRepeating("UpdateInfo" , 0, .2f);
    }

    private void UpdateInfo() {
        distance = GameManager.Instance.distance;
        coins = GameManager.Instance.coins;

        if (distance > 0)
            distanceText.text = distance.ToString("#,#");
        if (coins > 0)
            coinsText.text = coins.ToString("#,#");

        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
    }
}
