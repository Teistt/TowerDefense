using UnityEngine;

/*
 * Interface between shop canvas and communicating TurretBP to build Manager
 */


public class Shop : MonoBehaviour
{
    private BuildManager buildManager;

    [SerializeField] private GameObject itemUIPrefab=null;

    [SerializeField] private TurretBP[] blueprints;

    private void Start()
    {
        buildManager = BuildManager.instance;

        for (int i = 0; i < blueprints.Length; i++)
        {
            GameObject itemObj=Instantiate(itemUIPrefab,gameObject.transform);
            ShopItemUI ui = itemObj.GetComponent<ShopItemUI>();
            ui.InitalizeItem(blueprints[i]);
            int j = i;
            ui.BuyButton.onClick.AddListener(delegate{
                SelectTurret(blueprints[j]);
                
            });
        }
    }

    public void SelectTurret(TurretBP turretBP)
    {
        buildManager.SelectTurretToBuild(turretBP);
    }

}
