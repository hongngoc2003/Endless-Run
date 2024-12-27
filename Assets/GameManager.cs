using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public Color platformColor;
    public int coins;
    private void Awake() {
        Instance = this;
    }

    public void RestartLevel() => SceneManager.LoadScene(0);
}