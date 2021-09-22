using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


/*
 * Variant Infinite Waves Spawner differents waves
 * Number of waves is infinite
 * 
 * Enemy's composition and time of each waves is parametable
 * 
 */

public class InfiniteWaveSpawner : MonoBehaviour
{
    public static int _enemiesAlive = 0;

    //private Wave waves;
    public GameManager gameManager;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float timeNextWave = 15f;
    [SerializeField] private float rate = 1f;

    [SerializeField] private GameObject[] enemiesPrefab;

    [SerializeField] private TextMeshProUGUI livesTextPro;
    [SerializeField] private TextMeshProUGUI remainRoundsTextPro;
    [SerializeField] private TextMeshProUGUI waveTimeTextPro;
    [SerializeField] private TextMeshProUGUI moneyTextPro;

    [SerializeField] private List<GameObject> waveEnemies;

    private float countdown = 2f;
    [SerializeField] private int waveIndex = 0;

    public int _waveIndex { get { return waveIndex; } }

    //ax^2+bx+c to define enemy number's according to wave numner
    private float aConst = 0.0608f;
    private float bConst = 1.1753f;
    private float cConst = 2.5785f;


    private void Awake()
    {
        waveIndex = PlayerStats.rounds;
        _enemiesAlive = 0;
        countdown = timeNextWave;
    }

    private void Update()
    {
        waveTimeTextPro.text = string.Format("{0:00.0}", countdown);
        moneyTextPro.text = "$" + PlayerStats.money.ToString();

        remainRoundsTextPro.text = waveIndex.ToString() + " waves survived";
        

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
            countdown = timeNextWave;
            waveIndex++;
            CreateNextWave();
            
            rate = waveIndex/2f;
            if (rate > 5) rate = 5;
            StartCoroutine(SpawnWave());
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
    }

    private void CreateNextWave()
    {
        /*
         * Planned for enemiesPrefab[] filled as follow
         * 0 EnemySimple
         * 1 EnemyFast
         * 2 EnemySlow
         * 3 EnemySimpleStrong
         * 4 EnemyFastStrong
         * 5 EnemySlowStrong
         */

        waveEnemies.Clear();
        int enemiesNumber = (int)(aConst*waveIndex* waveIndex+bConst* waveIndex+cConst);
        Debug.Log("enemy number:" + enemiesNumber);

        if (waveIndex <= 3)
        {
            //Just as many normal enemies as waves passed
            for (int i = 0; i < enemiesNumber; i++)
            {
                waveEnemies.Add(enemiesPrefab[0]);
            }
        }

        else if (waveIndex <= 6)
        {
            //Random between normal and fast enemies
            for (int i = 0; i < enemiesNumber; i++)
            {
                waveEnemies.Add(enemiesPrefab[Random.Range(0, 2)]);
            }
            waveEnemies = waveEnemies.OrderBy(t => t.name).ToList();
            waveEnemies.Reverse();
        }
        else if(waveIndex<=10)
        {
            //Random between normal slow and fast enemies
            for (int i = 0; i < enemiesNumber; i++)
            {
                waveEnemies.Add(enemiesPrefab[Random.Range(0, 3)]);
            }
            waveEnemies=waveEnemies.OrderBy(t => t.name).ToList();
            waveEnemies.Reverse();
        }
        else if(waveIndex<=15)
        {
            //Random between normalStrong slow and fast enemies
            for (int i = 0; i < enemiesNumber; i++)
            {
                waveEnemies.Add(enemiesPrefab[Random.Range(1, 4)]);
            }
            waveEnemies=waveEnemies.OrderBy(t => t.name).ToList();
            waveEnemies.Reverse();
        }
        else if(waveIndex<=20)
        {
            //Random between normalStrong faststrong and slow enemies
            for (int i = 0; i < enemiesNumber; i++)
            {
                waveEnemies.Add(enemiesPrefab[Random.Range(2, 5)]);
            }
            waveEnemies=waveEnemies.OrderBy(t => t.name).ToList();
            waveEnemies.Reverse();
        }
        else
        {
            //Random between normalStrong slowstrong and faststrong enemies
            for (int i = 0; i < enemiesNumber; i++)
            {
                waveEnemies.Add(enemiesPrefab[Random.Range(3, enemiesPrefab.Length)]);
            }
            waveEnemies=waveEnemies.OrderBy(t => t.name).ToList();
            waveEnemies.Reverse();
        }
    }

    IEnumerator SpawnWave()
    {
        foreach (GameObject enemy in waveEnemies)
        {
            Debug.Log(rate);
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(1f / rate);
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        _enemiesAlive++;
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
