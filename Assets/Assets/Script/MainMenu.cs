using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private GameObject optionCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject levelSelectorCanvas;


    [SerializeField] private GameObject btnContainer;
    private GameObject[] levelGO;
    private Button[] levelButtons;


    #region LevelSelector

    private void Awake()
    {
        levelGO = new GameObject[btnContainer.transform.childCount];
        for (int i = 0; i < btnContainer.transform.childCount; i++)
        {
            levelGO[i] = btnContainer.transform.GetChild(i).gameObject;
        }

        levelButtons = new Button[levelGO.Length];
        for (int i = 0; i < levelGO.Length; i++)
        {
            levelButtons[i] = levelGO[i].GetComponent<Button>();
            Debug.Log(levelButtons[i]);
        }
    }

    private void Start()
    {
        //Récupère le numéro associé à unlockedLevels, soit la valeur correspondante aux niveaux débloqués
        //Si le champ unlockedLevels n'est pas trouvé (aka on lance le jeu pour la 1ere fois) on prend la valeur par défaut de 1
        //Pour débloquer le 1er niveau
        int unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 1);

        if (unlockedLevels > levelButtons.Length)
        {
            Debug.LogError("probleme unlocked levels > nb levels; on débloque tout");
            unlockedLevels = levelButtons.Length;
        }
        Debug.Log("unlocked levels: " + unlockedLevels);

        for (int i = unlockedLevels; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }

    }

    public void Select(string levelName)
    {
        sceneFader.FadeTo(levelName);
    }
    public void Select(int sceneID)
    {
        sceneFader.FadeTo(sceneID);
    }
    #endregion

    #region MenuTransitions
    public void Play()
    {
        creditsCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        levelSelectorCanvas.SetActive(true);
        
        //sceneFader.FadeTo(levelSelector);
    }

    public void OptionsCanva()
    {
        creditsCanvas.SetActive(false);
        optionCanvas.SetActive(true);
        levelSelectorCanvas.SetActive(false);
    }

    public void CreditsBtn()
    {
        levelSelectorCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void BackToMainMenu()
    {
        levelSelectorCanvas.SetActive(false);
        optionCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

}
