using UnityEngine;

/*
 * Player stats
 */

public class PlayerStats : MonoBehaviour
{
    //Les variables static sont gardées d'une scène à une autre
    public static int money;
    public int startMoney = 400;

    public static int lives;
    public int startLives;

    public static int rounds;

    private void Start()
    {
        rounds = 0;
        money = startMoney;
        lives = startLives;
    }

}
