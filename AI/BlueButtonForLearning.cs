using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueButtonForLearning : MonoBehaviour, ActionsButton
{
    [HideInInspector]
    public InsufficientBalance[] summonFunctions
    {
        get; set;
    }

    public BlueButtonForLearning()
    {
        summonFunctions = new InsufficientBalance[14];
        summonFunctions[0] = buttonControlsBlue.blueWating;
        summonFunctions[1] = buttonControlsBlue.blueSummonOne;
        summonFunctions[2] = buttonControlsBlue.blueSummonTwo;
        summonFunctions[3] = buttonControlsBlue.blueSummonThree;
        summonFunctions[4] = buttonControlsBlue.blueSummonFour;
        summonFunctions[5] = buttonControlsBlue.blueSummonFive;
        summonFunctions[6] = buttonControlsBlue.blueSummonSix;
        summonFunctions[7] = buttonControlsBlue.blueSummonSeven;
        summonFunctions[8] = buttonControlsBlue.blueSummonEight;
        summonFunctions[9] = buttonControlsBlue.blueSummonNine;
        summonFunctions[10] = buttonControlsBlue.dmgUpBlue;
        summonFunctions[11] = buttonControlsBlue.defUpBlue;
        summonFunctions[12] = buttonControlsBlue.blueNuke;
        summonFunctions[13] = buttonControlsBlue.blueMindControl;
    }

    public void Start()
    {
        //this.summonFunctions[12]("Type1");
        //BuildFunctions();
        //summonFunctions[2]("Type1");
        //summonFunctions[3]("Type1");
        //summonFunctions[1]("Type3");


        //if (Regame.instance.resetCount > 1)
        //{
        //    return;
        //}
    }

    public void BuildFunctions()
    {
        //    summonFunctions = new OverFlowed[14];
        //    summonFunctions[0] = buttonControlsBlue.blueWating;
        //    summonFunctions[1] = buttonControlsBlue.blueSummonOne;
        //    summonFunctions[2] = buttonControlsBlue.blueSummonTwo;
        //    summonFunctions[3] = buttonControlsBlue.blueSummonThree;
        //    summonFunctions[4] = buttonControlsBlue.blueSummonFour;
        //    summonFunctions[5] = buttonControlsBlue.blueSummonFive;
        //    summonFunctions[6] = buttonControlsBlue.blueSummonSix;
        //    summonFunctions[7] = buttonControlsBlue.blueSummonSeven;
        //    summonFunctions[8] = buttonControlsBlue.blueSummonEight;
        //    summonFunctions[9] = buttonControlsBlue.blueSummonNine;
        //    summonFunctions[10] = buttonControlsBlue.dmgUpBlue;
        //    summonFunctions[11] = buttonControlsBlue.defUpBlue;
        //    summonFunctions[12] = buttonControlsBlue.blueNuke;
        //    summonFunctions[13] = buttonControlsBlue.blueMindControl;

        //    //summonFunctions[2]("Type1");
        //    //summonFunctions[2](t);
        //}
    }
}

class buttonControlsBlue
{
    public static int blueWating(string t)
    {
        return 0;
    }

    public static int blueNuke(string t)
    {
        NukePower.instance[0].Nuke(2);
        return 0;
    }

    public static int blueMindControl(string t)
    {
        mindControl.Instance.mindControlP();
        return 0;
    }
    public static int blueSummonOne(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            //prefab = PoolManagerRed.instance.prefabsT1[0];
            prefab = PoolManager.instance.prefabsT1[0];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            //prefab = PoolManagerRed.instance.prefabsT2[0];
            prefab = PoolManager.instance.prefabsT2[0];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            //prefab = PoolManagerRed.instance.prefabsT3[0];
            prefab = PoolManager.instance.prefabsT3[0];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int blueSummonTwo(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManager.instance.prefabsT1[1];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManager.instance.prefabsT2[1];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManager.instance.prefabsT3[1];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int blueSummonThree(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManager.instance.prefabsT1[2];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManager.instance.prefabsT2[2];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManager.instance.prefabsT3[2];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int blueSummonFour(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManager.instance.prefabsT1[3];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManager.instance.prefabsT2[3];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManager.instance.prefabsT3[3];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int blueSummonFive(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManager.instance.prefabsT1[4];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManager.instance.prefabsT2[4];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManager.instance.prefabsT3[4];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int blueSummonSix(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            //prefab = PoolManagerRed.instance.prefabsT1[5];
            prefab = PoolManager.instance.prefabsT1[5];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            //prefab = PoolManagerRed.instance.prefabsT2[5];
            prefab = PoolManager.instance.prefabsT2[5];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            //prefab = PoolManagerRed.instance.prefabsT3[5];
            prefab = PoolManager.instance.prefabsT3[5];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int blueSummonSeven(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            //prefab = PoolManagerRed.instance.prefabsT1[6];
            prefab = PoolManager.instance.prefabsT1[6];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            //prefab = PoolManagerRed.instance.prefabsT2[6];
            prefab = PoolManager.instance.prefabsT2[6];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            //prefab = PoolManagerRed.instance.prefabsT3[6];
            prefab = PoolManager.instance.prefabsT3[6];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int blueSummonEight(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            //prefab = PoolManagerRed.instance.prefabsT1[7];
            prefab = PoolManager.instance.prefabsT1[7];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            //prefab = PoolManagerRed.instance.prefabsT2[7];
            prefab = PoolManager.instance.prefabsT2[7];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            //prefab = PoolManagerRed.instance.prefabsT3[7];
            prefab = PoolManager.instance.prefabsT3[7];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public static int blueSummonNine(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            //prefab = PoolManagerRed.instance.prefabsT1[8];
            prefab = PoolManager.instance.prefabsT1[8];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            //prefab = PoolManagerRed.instance.prefabsT2[8];
            prefab = PoolManager.instance.prefabsT2[8];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            //prefab = PoolManagerRed.instance.prefabsT3[8];
            prefab = PoolManager.instance.prefabsT3[8];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.resource >= cost)
        {
            PoolManager.instance.selectedPrefabsBlue.Add(prefab);
            GameManager.instance.resource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int dmgUpBlue(string t)
    {
        //if (GameManager.instance.resource > UpgradeManager.instance.dmgCost[UpgradeManager.instance.dmgLvl] && UpgradeManager.instance.dmgLvl < 7)
        if (GameManager.instance.resource >= UpgradeManager.instance.dmgCost[UpgradeManager.instance.dmgLvl] && UpgradeManager.instance.dmgLvl < 7)
        {
            GameManager.instance.resource -= UpgradeManager.instance.dmgCost[UpgradeManager.instance.dmgLvl];
            UpgradeManager.instance.dmgLvl++;
            UpgradeManager.instance.dmgUp++;
            return 0;
        }
        else if (GameManager.instance.resource < UpgradeManager.instance.dmgCost[UpgradeManager.instance.dmgLvl] && UpgradeManager.instance.dmgLvl < 7)
        {
            return 1;
        }
        else
        {
            return 2;
        }

    }

    public static int defUpBlue(string t)
    {
        //if (GameManager.instance.resource > UpgradeManager.instance.defCost[UpgradeManager.instance.defLvl] && UpgradeManager.instance.defLvl < 7)
        if (GameManager.instance.resource >= UpgradeManager.instance.defCost[UpgradeManager.instance.defLvl] && UpgradeManager.instance.defLvl < 7)
        {
            GameManager.instance.resource -= UpgradeManager.instance.defCost[UpgradeManager.instance.defLvl];
            UpgradeManager.instance.defLvl++;
            UpgradeManager.instance.defUp++;

            return 0;
        }
        else if (GameManager.instance.resource < UpgradeManager.instance.defCost[UpgradeManager.instance.defLvl] && UpgradeManager.instance.defLvl < 7)
        {
            return 1;
        }
        else {
            return 2;
        }


    }
}