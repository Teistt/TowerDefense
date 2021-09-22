using System.Collections;
using UnityEngine;
using TMPro;


/*
 * Manage Wves differents waves
 * Number of waves is parametable
 * 
 * Enemy's composition and time of each waves is parametable
 * 
 */

public class WaveSpawner : MonoBehaviour
{
    public static int _enemiesAlive = 0;

    public Wave[] waves;
    public GameManager gameManager;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float timeBeforeFirstWave = 15f;

    [SerializeField] private TextMeshProUGUI livesTextPro;
    [SerializeField] private TextMeshProUGUI remainRoundsTextPro;
    [SerializeField] private TextMeshProUGUI waveTimeTextPro;
    [SerializeField] private TextMeshProUGUI moneyTextPro;

    private float countdown = 2f;
    private int waveIndex = 0;

    private void Awake()
    {
        _enemiesAlive = 0;
        countdown = timeBeforeFirstWave;
    }

    private void Update()
    {
        waveTimeTextPro.text = string.Format("{0:00.0}", countdown);
        moneyTextPro.text = "$" + PlayerStats.money.ToString();

        int nbWave = waves.Length - waveIndex;
        if (nbWave!=0)
        {
            remainRoundsTextPro.text = nbWave.ToString() + " waves remaining";
        }
        else
        {
            remainRoundsTextPro.text = "Last wave!!";
        }
        
        if (waveIndex == waves.Length && _enemiesAlive <= 0)
        {
            gameManager.WinLevel();
            this.enabled = false;
        }

        if (PlayerStats.lives > 1)
        {
            livesTextPro.text = PlayerStats.lives + " LIVES";
        }
        else if(PlayerStats.lives ==1)
        {
            livesTextPro.text = PlayerStats.lives + " LIVE";
        }
        else
        {
            livesTextPro.text = "LOST";
        }

        if (_enemiesAlive > 0)
        {
            return;
        }


        if (countdown <= 0f)
        {
            countdown = waves[waveIndex].timeNextWave;
            PlayerStats.rounds++;
            StartCoroutine(SpawnWave());
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.rounds++;
        Wave wave = waves[waveIndex];

        foreach (GameObject enemy in wave.enemies)
        {
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        if (waveIndex < waves.Length)
        {
            waveIndex++;
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        _enemiesAlive++;
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
