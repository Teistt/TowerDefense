using UnityEngine;

public class BuildManager : MonoBehaviour
{
    /*
     * This script manage the building of nodes
     */


    #region Singleton
    //On cr�� un BM pour y lier ce BM
    public static BuildManager instance;

    //Singleton: permet d'acc�der au build manager depuis n'importe o� sans avoir � le d�finir � chaque fois
    private void Awake()
    {
        //On v�rifie si un build manager a d�j� �t� instanti�
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
        //Si on s�lectionne un BP de tourelle dans le shop on la renseigne ici
        turretToBuild = turret;
        //Et on d�selectionne l'�ventuel node s�lectionn�
        DeselectNode();
    }

    public void SelectNode(Node node)
    {
        //Si on clique sur un node et que celui ci contient une tourelle (cf Node)
        //Alors on v�rifie que le node envoy� par Node est le m�me que celui s�lectionn� ici
        //aka le node avec l'UI; alors on le d�selectionne et on quitte
        if (node == selectedNode)
        {
            //Debug.Log("node est d�selec");
            DeselectNode();
            return;
        }

        //Sinon on le s�lectionne
        selectedNode = node;
        //Si node s�lectionn�, on d�selectionne la tourelle choisie dans le shop pour �viter
        //tout missclick
        turretToBuild = null;
        //On positionne le nodeUI sur le node s�lectionn�
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
