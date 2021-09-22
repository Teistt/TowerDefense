using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    private Node target;
    public Text upgradeCost; 
    public Button upgradeButton;

    public Text sellAmount;

    private Color baseTextColor;
    public Color nonAvailableTextColor;


    private void Awake()
    {
        baseTextColor = upgradeCost.color;
    }
    public void SetTarget(Node _target)
    {
         
        target = _target;

        transform.position = target.GetBuildPosition();
        if (!target.isUpgraded)
        {
            upgradeCost.text = "-$"+target.turretBP._upgradeCost;
            if(PlayerStats.money>= target.turretBP._upgradeCost)
            {
                upgradeCost.color = baseTextColor;
                upgradeButton.interactable = true;
            }
            else
            {

                upgradeCost.color = nonAvailableTextColor;
                upgradeButton.interactable = false;
            }
        }
        else
        {
            upgradeCost.color = nonAvailableTextColor;
            upgradeCost.text = "<b>No more ameliorations</b>";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "+$" + target.turretBP.GetSellAmount(target.isUpgraded);
        ui.SetActive(true);
    }
    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
    
}
