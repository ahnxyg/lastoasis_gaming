using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnController : MonoBehaviour
{
    public GameObject[] Buttons;
    public GameObject summonBtn;
    public GameObject stackBtn;
    public GameObject upgradeBtn;

    public GameObject SummonUI;
    public GameObject upgradeUI;
    public GameObject dmgUI;

    public GameObject[] abilityButtons;
    public Vector2 abilityPos;

    public void Start()
    {
        summonBtn.SetActive(false);
        foreach(GameObject btn in Buttons)
        {
            btn.SetActive(false);
        }

        

    }
    public void ExpandMenu()
    {
        foreach (GameObject btn in Buttons)
        {
            btn.SetActive(!btn.activeSelf);
        }
        if(summonBtn.activeSelf)
        {
            summonBtn.SetActive(false);
        }
        if(dmgUI.activeSelf)
        {
            dmgUI.SetActive(false);
        }
        if(upgradeUI.activeSelf)
        {
            upgradeUI.SetActive(false);
        }
    }

    public void ExpandSummon()
    {
        if (upgradeUI.activeSelf)
        {
            upgradeUI.SetActive(false);
        }
        if (dmgUI.activeSelf)
        {
            dmgUI.SetActive(false);
        }
        summonBtn.SetActive(!summonBtn.activeSelf);
        
        
    }

    public void ExpandDMG()
    {
        if (dmgUI.activeSelf)
        {
            dmgUI.SetActive(false);
        }
        else
        {
            if (upgradeUI.activeSelf)
            {
                upgradeUI.SetActive(false);
            }
            if (SummonUI.activeSelf)
            {
                SummonUI.SetActive(false);
            }
            dmgUI.SetActive(true);
        }
    }

    public void ExpandUpgrade()
    {
        if (upgradeUI.activeSelf)
        {
            upgradeUI.SetActive(false);
        }
        else
        {
            if(dmgUI.activeSelf)
            {
                dmgUI.SetActive(false);
            }
            if (SummonUI.activeSelf)
            {
                SummonUI.SetActive(false);
            }
            upgradeUI.SetActive(true);
        }
    }
}
