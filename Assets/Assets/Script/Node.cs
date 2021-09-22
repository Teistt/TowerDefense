using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    /*
     * This script change node's color depending on the mouse
     * Interact with BuildManager to build, upgrade or sell turrets
     * Interract with NodeSoundEffects to play turret's build, upgrade or sell sound effects
     * */

    private BuildManager buildManager;
    public GameObject buildParticles;
    public GameObject costGO;


    //Contient les données de la tour présente, par défaut null
    [HideInInspector] public GameObject turret;
    [HideInInspector] public TurretBP turretBP;
    [HideInInspector] public bool isUpgraded=false;

    private Renderer nodeRend;

    private Color startColor;
    public Color hoverColor;
    public Color notEnoughMoneyColor;

    public Vector3 posOffset;


    [SerializeField] private GameObject spherePrefab;
    private GameObject sphere;

    private void Start()
    {
        //On récupère le composant Renderer du node et sa couleur initiale
        nodeRend = GetComponent<Renderer>();
        startColor = nodeRend.material.color;

        buildManager = BuildManager.instance;
    }

    private void OnMouseOver()
    {
        //Test si il y a un autre GO entre la souris et le node, aka l'interface
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Si oui on reset la couleur du node et on exit
            nodeRend.material.color = startColor;
            return;
        }

        if (turretBP != null)
        {
            if (sphere == null)
            {
                sphere = Instantiate(spherePrefab, transform.position + posOffset, transform.rotation);
                sphere.transform.localScale = new Vector3(GetTurretRange(turretBP), GetTurretRange(turretBP), GetTurretRange(turretBP));
            }
        }

        if (!buildManager.canBuild)
        {
            return;
        }

        if (turretBP==null && buildManager.hasMoney)
        {
            TurretBP ProvTurretBP = buildManager.GetTurretToBuild();
            if (sphere == null)
            {
                sphere = Instantiate(spherePrefab, transform.position + posOffset, transform.rotation);
                sphere.transform.localScale = new Vector3(GetTurretRange(ProvTurretBP), GetTurretRange(ProvTurretBP), GetTurretRange(ProvTurretBP));
            }

            //On change la couleur du node si la souris passe sur le collider
            nodeRend.material.color = hoverColor;
        }
        else
        {
            //On change la couleur du node si la souris passe sur le collider
            nodeRend.material.color = notEnoughMoneyColor;
        }
    }


    private void OnMouseDown()
    {
        //Test si il y a un autre GO entre la souris et le node, aka l'interface
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("y a un GO");
            //Si oui on reset la couleur du node et on exit
            nodeRend.material.color = startColor;
            return;
        }

        //On vérifie si y a déjà une tour
        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.canBuild)
        {
            return;
        }

        BuildTurret(buildManager.GetTurretToBuild());
    }


    private void OnMouseExit()
    {
        if (sphere != null)
        {
            Destroy(sphere);
        }
        //On remet la couleur par défaut du node si la souris sort du collider
        nodeRend.material.color = startColor;
    }

    private void BuildTurret(TurretBP blueprint)
    {

        if (PlayerStats.money < blueprint._cost)
        {
            Debug.Log("argent trop cher");
            return;
        }

        PlayerStats.money -= blueprint._cost;
        turretBP = blueprint;

        GameObject _turret = (GameObject)Instantiate(blueprint._prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        GameObject effect = (GameObject)Instantiate(buildParticles, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 1f);

        GameObject txtGO = (GameObject)Instantiate(costGO, transform.position, Quaternion.identity);
        RewardMoneyBehviour txt = txtGO.GetComponent<RewardMoneyBehviour>();
        txt.setTxt(-blueprint._cost);

        buildManager.PlaySoundIntermed("BuildEffect");
        //Sinon:
        //Construction d'une tourelle
        //On accède à la fct getTurretToBuild grâce au singleton dans buildmanager
        //GameObject turretToBuild = buildManager.instance.GetTurretToBuild();
        //turret = (GameObject)Instantiate(turretToBuild, transform.position + new Vector3(0f, 0.5f, 0f), transform.rotation);
        //A noter l'offset de 0.5 en y de la tour
    }
    
    public void SellTurret()
    {
        PlayerStats.money += turretBP.GetSellAmount(isUpgraded);
        Destroy(turret);
        turretBP = null;
        isUpgraded = false;

        buildManager.PlaySoundIntermed("DestroyEffect");
    }

    public float GetTurretRange(TurretBP t)
    {
        Debug.Log("node");
        return t.GetRange(isUpgraded);
    }

    public void UpgradeTurret()
    {
        
        if (PlayerStats.money < turretBP._upgradeCost)
        {
            Debug.Log("argent trop cher, amélioration n'a pas de prix! pas de prix!");
            return;
        }

        PlayerStats.money -= turretBP._upgradeCost;

        Destroy(turret);

        GameObject _turret = (GameObject)Instantiate(turretBP._upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject effect = (GameObject)Instantiate(buildParticles, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 1f);


        GameObject txtGO = (GameObject)Instantiate(costGO, transform.position, Quaternion.identity);
        RewardMoneyBehviour txt = txtGO.GetComponent<RewardMoneyBehviour>();
        txt.setTxt(-turretBP._upgradeCost);

        isUpgraded = true;

        buildManager.PlaySoundIntermed("UpgradeEffect");
    }

    public Vector3 GetBuildPosition()
    {
        return (transform.position + posOffset);
    }
}
