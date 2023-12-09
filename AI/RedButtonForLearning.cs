using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedButtonForLearning : MonoBehaviour, ActionsButton
{
    [HideInInspector]
    public InsufficientBalance[] summonFunctions
    {
        get; set;
    }

    public RedButtonForLearning()
    {
        summonFunctions = new InsufficientBalance[14];

        summonFunctions[0] = buttonControls.redWait;
        summonFunctions[1] = buttonControls.summonOne;
        summonFunctions[2] = buttonControls.summonTwo;
        summonFunctions[3] = buttonControls.summonThree;
        summonFunctions[4] = buttonControls.summonFour;
        summonFunctions[5] = buttonControls.summonFive;
        summonFunctions[6] = buttonControls.summonSix;
        summonFunctions[7] = buttonControls.summonSeven;
        summonFunctions[8] = buttonControls.summonEight;
        summonFunctions[9] = buttonControls.summonNine;
        summonFunctions[10] = buttonControls.dmgUpRed;
        summonFunctions[11] = buttonControls.defUpRed;
        summonFunctions[12] = buttonControls.redNuke;
        summonFunctions[13] = buttonControls.redWait;
    }

    // Start is called before the first frame update
    public void Start()
    {
        //BuildFunctions();
        //summonFunctions[12]("Type1");
    }

    public void BuildFunctions()
    {
        //summonFunctions = new OverFlowed[14];

        //summonFunctions[0] = buttonControls.redWait;
        //summonFunctions[1] = buttonControls.summonOne;
        //summonFunctions[2] = buttonControls.summonTwo;
        //summonFunctions[3] = buttonControls.summonThree;
        //summonFunctions[4] = buttonControls.summonFour;
        //summonFunctions[5] = buttonControls.summonFive;
        //summonFunctions[6] = buttonControls.summonSix;
        //summonFunctions[7] = buttonControls.summonSeven;
        //summonFunctions[8] = buttonControls.summonEight;
        //summonFunctions[9] = buttonControls.summonNine;
        //summonFunctions[10] = buttonControls.dmgUpRed;
        //summonFunctions[11] = buttonControls.defUpRed;
        //summonFunctions[12] = buttonControls.redNuke;
        //summonFunctions[13] = buttonControls.redWait;

        //summonFunctions[0]("Type1");
        //summonFunctions[10](type);
    }


}

class buttonControls
{
    public static int redWait(string t)
    {
        return 0;
    }
    public static int redNuke(string t)
    {
        NukePower.instance[1].Nuke(1);
        return 0;
    }
    public static int summonOne(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[0];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[0];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[0];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonTwo(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[1];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[1];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[1];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonThree(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[2];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[2];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[2];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonFour(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[3];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[3];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[3];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonFive(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[4];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[4];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[4];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonSix(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[5];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[5];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[5];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonSeven(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[6];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[6];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[6];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonEight(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[7];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[7];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[7];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int summonNine(string type)
    {
        int cost;
        GameObject prefab;
        if (type == "Type1")
        {
            prefab = PoolManagerRed.instance.prefabsT1[8];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type2")
        {
            prefab = PoolManagerRed.instance.prefabsT2[8];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else if (type == "Type3")
        {
            prefab = PoolManagerRed.instance.prefabsT3[8];
            cost = prefab.GetComponent<MobController>().cost;
        }
        else
        {
            return 2;
        }

        if (GameManager.instance.redResource >= cost)
        {
            PoolManagerRed.instance.selectedPrefabsRed.Add(prefab);
            GameManager.instance.redResource -= cost;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static int dmgUpRed(string t)
    {
        //if (GameManager.instance.redResource > UpgradeManager.instance.dmgCost[UpgradeManager.instance.redDmgLvl] && UpgradeManager.instance.redDmgLvl < 7)
        if (GameManager.instance.redResource >= UpgradeManager.instance.dmgCost[UpgradeManager.instance.redDmgLvl] && UpgradeManager.instance.redDmgLvl < 7)
        {
            GameManager.instance.redResource -= UpgradeManager.instance.dmgCost[UpgradeManager.instance.redDmgLvl];
            UpgradeManager.instance.redDmgLvl++;
            UpgradeManager.instance.redDmgUp++;

            return 0;
        }
        if (GameManager.instance.redResource < UpgradeManager.instance.dmgCost[UpgradeManager.instance.redDmgLvl] && UpgradeManager.instance.redDmgLvl < 7)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public static int defUpRed(string t)
    {
        //if (GameManager.instance.redResource > UpgradeManager.instance.defCost[UpgradeManager.instance.redDefLvl] && UpgradeManager.instance.redDefLvl < 7)
        if (GameManager.instance.redResource >= UpgradeManager.instance.defCost[UpgradeManager.instance.redDefLvl] && UpgradeManager.instance.redDefLvl < 7)
        {
            GameManager.instance.redResource -= UpgradeManager.instance.defCost[UpgradeManager.instance.redDefLvl];
            UpgradeManager.instance.redDefLvl++;
            UpgradeManager.instance.redDefUp++;

            return 0;
        }
        else if (GameManager.instance.redResource < UpgradeManager.instance.defCost[UpgradeManager.instance.redDefLvl] && UpgradeManager.instance.redDefLvl < 7)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
