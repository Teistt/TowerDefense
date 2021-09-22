using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Game Manager
 */

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isInfinite=false;
    //La valeur d'une variable static est gardée d'une scène à une autre, il faudra donc l'init à false en void start()
    public static bool gameIsOver;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winOverUI;

    //public string nextLevel = "Level02";
    [SerializeField] private int levelToUnlock = 2;

    //public SceneFader sceneFader;

    private void Start()
    {
        if (!isInfinite)
        {
            levelToUnlock = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log("level to unlock" + levelToUnlock);
        }

        gameIsOver = false;
    }

    private void Update()
    {
        if (gameIsOver)
        {
            return;
        }

        if (PlayerStats.lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);

        //If we are in infinite mode, we recover wave number data in infinite waveSpawner component
        //And then we pass it in gameover component to display it
        if (isInfinite)
        {
            int round=gameObject.GetComponent<InfiniteWaveSpawner>()._waveIndex;
            gameOverUI.GetComponent<GameOver>().WaveNumberRecap(round);
        }
    }
    

    public void WinLevel()
    {
        if (isInfinite)
        {
            Debug.LogError("Cannot win, game is infinite");
            return;
        }

        Debug.Log("win!");
        if (levelToUnlock> PlayerPrefs.GetInt("unlockedLevels", 1))
        {
            PlayerPrefs.SetInt("unlockedLevels", levelToUnlock);
        }
        winOverUI.SetActive(true);
    }
}
