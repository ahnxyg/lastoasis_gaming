using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    static public UpgradeManager instance;
    public float dmgUp;
    public float redDmgUp;
    public float defUp;
    public float redDefUp;
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI redDmgText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI redDefText;
    public int dmgLvl;
    public int redDmgLvl;
    public int defLvl;
    public int redDefLvl;
    public int[] dmgCost = new int[] { 200, 250, 300, 400, 500, 700, 900 };
    public int[] defCost = new int[] { 300, 350, 400, 500, 600, 800, 1000 };

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        try
        {
            dmgText.text = dmgCost[dmgLvl].ToString();
            defText.text = defCost[defLvl].ToString();
            redDmgText.text = dmgCost[redDmgLvl].ToString();
            redDefText.text = defCost[redDefLvl].ToString();
        }
        catch
        {

        }
        dmgLvl = 0;
        redDmgLvl = 0;
        defLvl = 0;
        redDefLvl = 0;
        dmgUp = 0f;
        redDmgUp = 0f;
        defUp = 0f; 
        redDefUp = 0f;
    }

    public void dmgBtn()
    {
        if (GameManager.instance.resource > dmgCost[dmgLvl] && dmgLvl < 7)
        {
            GameManager.instance.resource -= dmgCost[dmgLvl];
            dmgLvl++;
            dmgUp++;
            dmgText.text = dmgCost[dmgLvl].ToString();
        }
    }

    public void dmgBtnRed()
    {
        if(GameManager.instance.redResource > dmgCost[redDmgLvl] && dmgLvl < 7)
        {
            GameManager.instance.redResource -= dmgCost[redDmgLvl];
            redDmgLvl++;
            redDmgUp++;
            redDmgText.text = dmgCost[redDmgLvl].ToString();
        }
    }

    public void defBtn()
    {
        if(GameManager.instance.resource > defCost[defLvl] && defLvl < 7)
        {
            GameManager.instance.resource -= defCost[defLvl];
            defLvl++;
            defUp += 2;
            defText.text = defCost[defLvl].ToString();
        }
    }

    public void defBtnRed()
    {
        if(GameManager.instance.redResource > defCost[redDefLvl] && redDefLvl < 7)
        {
            GameManager.instance.redResource -= defCost[redDefLvl];
            redDefLvl++;
            redDefUp += 2;
            redDefText.text = defCost[redDefLvl].ToString();
        }
    }

}
