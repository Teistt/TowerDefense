using UnityEngine;

[CreateAssetMenu(fileName = "Unnamed Turret Blueprint", menuName = "Turrets/New Turret Blueprint")]
public class TurretBP : ScriptableObject
{
    public string turretName;

    [SerializeField] private Sprite icon = null;

    [SerializeField] private GameObject prefab = null;
    [SerializeField] private GameObject upgradedPrefab = null;
    [SerializeField] private int cost;
    [SerializeField] private int upgradeCost;

    public Sprite _icon { get { return icon; } }
    public GameObject _prefab { get { return prefab; } }
    public GameObject _upgradedPrefab { get { return upgradedPrefab; } }
    public int _cost { get { return cost; } }
    public int _upgradeCost { get { return upgradeCost; } }


    public int GetSellAmount(bool upgraded)
    {
        //Si la tourelle n'est pas améliorée on rend la moitié du coup
        if (!upgraded)
        {
            return ((cost) / 2);
        }
        //Si elle est améliorée on rend la moitié du cout achat + upgrade
        else
        {
            return ((cost + upgradeCost) / 2);
        }

    }

    public float GetRange(bool upgraded)
    {
        Debug.Log("turret blueprint");

        if (!upgraded)
        {
            return prefab.GetComponent<Turret>().range;
        }
        else
        {
            return upgradedPrefab.GetComponent<Turret>().range;
        }
        
    }
}
