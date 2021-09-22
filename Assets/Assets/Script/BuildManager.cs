using UnityEngine;

public class BuildManager : MonoBehaviour
{
    /*
     * This script manage the building of nodes
     */


    #region Singleton
    //On créé un BM pour y lier ce BM
    public static BuildManager instance;

    //Singleton: permet d'accèder au build manager depuis n'importe où sans avoir à le définir à chaque fois
    private void Awake()
    {
        //On vérifie si un build manager a déjà été instantié
        if (instance != null)
        {
            //Si oui, erreur
            Debug.LogError("double build manager");
            return;
        }
        //Sinon on instantie ce BM
        instance = this;
    }
    #endregion

    private TurretBP turretToBuild;
    private Node selectedNode;


    public NodeUI nodeUI;

    [SerializeField] private AudioManager sounds;

    //La tourelle a construire
    public bool canBuild {  get { return turretToBuild != null; } }
    public bool hasMoney { get { return PlayerStats.money >= turretToBuild._cost; } }

    private void Start()
    {
        sounds = FindObjectOfType<AudioManager>();
    }

    public void SelectTurretToBuild(TurretBP turret)
    {
        //Si on sélectionne un BP de tourelle dans le shop on la renseigne ici
        turretToBuild = turret;
        //Et on déselectionne l'éventuel node sélectionné
        DeselectNode();
    }

    public void SelectNode(Node node)
    {
        //Si on clique sur un node et que celui ci contient une tourelle (cf Node)
        //Alors on vérifie que le node envoyé par Node est le même que celui sélectionné ici
        //aka le node avec l'UI; alors on le déselectionne et on quitte
        if (node == selectedNode)
        {
            //Debug.Log("node est déselec");
            DeselectNode();
            return;
        }

        //Sinon on le sélectionne
        selectedNode = node;
        //Si node sélectionné, on déselectionne la tourelle choisie dans le shop pour éviter
        //tout missclick
        turretToBuild = null;
        //On positionne le nodeUI sur le node sélectionné
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public TurretBP GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void PlaySoundIntermed(string soundName)
    {
        Debug.Log("Playing " + soundName + "sound effect");
        sounds.Play(soundName);
    }

}
