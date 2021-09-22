using UnityEngine;
using System.Collections;
using TMPro;

/*
 * This script manage the Text effect when money is earned or spent
 * 
 */

public class RewardMoneyBehviour : MonoBehaviour
{
    private TextMeshProUGUI moneyRewardTextPro;
    public AnimationCurve curve;
    private Transform moneyRewardGO;
    private Color couleur=new Color();
    public float fadeTime=5f;
    [SerializeField]
    private float m_Speed;
    private int value=0;
    private bool valueToUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        moneyRewardGO = transform.GetChild(0);
        moneyRewardTextPro = moneyRewardGO.GetComponent<TextMeshProUGUI>();
        couleur = moneyRewardTextPro.color;
    }

    private void Update()
    {
        moneyRewardGO.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * m_Speed, Space.Self);

        if (valueToUpdate == false)
        {
            return;
        }

        valueToUpdate = false;
        if (value < 0)
        {
            couleur = new Color(0.3207547f, 0.06505874f, 0.06505874f, 1);
            
            moneyRewardTextPro.text = value + "$";
        }
        else
        {
            moneyRewardTextPro.text = "+"+value + "$";
        }

        StartCoroutine(FadeIn());
    }

    public void setTxt(int v)
    {
        valueToUpdate = true;
        
        //moneyRewardTextPro.text = v + "$";
        value = v;
    }

    IEnumerator FadeIn()
    {
        float t = fadeTime;
        
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t/fadeTime);
            //Debug.Log(couleur);
            moneyRewardTextPro.color = new Color(couleur.r, couleur.g, couleur.b, a);
            yield return 0;
        }
        Destroy(gameObject);
    }
}
