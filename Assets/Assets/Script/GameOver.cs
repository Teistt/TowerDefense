using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI survivedRoundsTextPro;
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private string mainMenu = "MainMenu";

    public void Retry()
    {
        //Récupère le nom de la scène actuelle
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        sceneFader.FadeTo(mainMenu);
    }

    public void WaveNumberRecap(int round)
    {
        round--;
        if (round < 0) round = 0;

        survivedRoundsTextPro.text = round + " rounds survived!";
    }
}
