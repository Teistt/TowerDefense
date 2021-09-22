using UnityEngine;
using UnityEngine.UI;

/*
 * Manage life's Enemy and slow hazard from laser turret
 * 
 */

public class Enemy : MonoBehaviour
{
    public float startHealth = 100f;
    public GameObject healthUI;
    public Image healthBar;
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float health;

    public int moneyReward = 50;

    public GameObject deathEffect;
    public GameObject rewardGO;

    private bool hasDied = false;

    private void Start()
    {
        health = startHealth;
        speed = startSpeed;
    }
    public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
    }

    public void TakeDamage(float amount)
    {
        //on vérifie que l'enemy est mort car si il continue à prendre des dégats
        //alors que le GO est en train d'être détruit la fonction die est appelée plusieurs fois
        //Aka le laser beam
        if (hasDied)
        {
            return;
        }

        healthUI.SetActive(true);
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        hasDied = true;
        PlayerStats.money += moneyReward;
        WaveSpawner._enemiesAlive--;
        InfiniteWaveSpawner._enemiesAlive--;
        GameObject d = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(d, 1f);
        GameObject txtGO = (GameObject)Instantiate(rewardGO, transform.position, Quaternion.identity);
        RewardMoneyBehviour txt = txtGO.GetComponent<RewardMoneyBehviour>();
        txt.setTxt(moneyReward);
        Destroy(gameObject);
    }
}
