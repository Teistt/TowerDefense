using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField]
    private Button buy;
    [SerializeField]
    private Image turretSprite;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;

    public Button BuyButton => buy;
    
    public void InitalizeItem(TurretBP bp)
    {
        turretSprite.sprite = bp._icon;
        nameText.text = bp.turretName;
        priceText.text = "$" + bp._cost;
    }
}
