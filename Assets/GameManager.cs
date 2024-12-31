using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public UIMain ui;
    public Player player;

    [Header("Color info")]
    public Color platformColor;

    [Header("Score info")]
    public float distance;
    public int coins;
    public float score;
    private void Awake() {
        Instance = this;
        Time.timeScale = 1;

        //LoadColor();
    }

    public void SaveColor(float r, float g, float b) {


        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorG", g);
        PlayerPrefs.SetFloat("ColorB", b);
    }

    private void LoadColor() {
        PlayerPrefs.GetInt("Coin");

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Color newColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                   PlayerPrefs.GetFloat("ColorG"),
                                   PlayerPrefs.GetFloat("ColorB"),
                                   PlayerPrefs.GetFloat("ColorA",1));

        sr.color = newColor;
    }

    private void Update() {
        if (player.transform.position.x > distance)
            distance = player.transform.position.x;        
    }
    public void UnlockPlayer() => player.playerStartToRun = true;
    public void RestartLevel() {
        SceneManager.LoadScene(0);
    }
    public void Save() {
        int savedCoins = PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", savedCoins + coins);

         score = distance * coins;
        PlayerPrefs.SetFloat("LastScore", score);

        if(PlayerPrefs.GetFloat("HighScore") < score)
            PlayerPrefs.SetFloat(("HighScore"), score);
    }

    public void EndGame() {
        Save();
        ui.OpenEndGameUI();
    }
}
