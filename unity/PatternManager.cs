using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public List<GameObject> prefabsT1 = new List<GameObject>();
    public List<GameObject> prefabsT2 = new List<GameObject>();
    public List<GameObject> prefabsT3 = new List<GameObject>();
    public List<GameObject> nowPrefabs = new List<GameObject>();

    public int[] cheap;
    public int[] mid;
    public int[] expensive;

    private string type;
    private int resource;
    private int randomCnt;
    private int targetResource;

    bool updateFlag;
    int lastP;

    public int blueObjectsInRedSide;

    public GameObject pointReds;

    private void OnEnable()
    {
        type = ChooseType.Instance.enemyType;
        resource = GameManager.instance.redResource;
    }

    private void Start()
    {
        updateFlag = true;
        lastP = 0;
        blueObjectsInRedSide = 0;

        if (type == "Type1")
        {
            nowPrefabs = new List<GameObject>(prefabsT1);

            cheap = new int[] { 0, 1 };
            mid = new int[] { 2, 3, 4, 5, 6 };
            expensive = new int[] { 7, 8 };
        }
        else if (type == "Type2")
        {
            nowPrefabs = new List<GameObject>(prefabsT2);

            cheap = new int[] { 0, 1, 2, 3, 4 };
            mid = new int[] { 6 };
            expensive = new int[] { 5, 7, 8 };
        }
        else
        {
            nowPrefabs = new List<GameObject>(prefabsT3);

            cheap = new int[] { 0, 1, 3 };
            mid = new int[] { 2, 4 };
            expensive = new int[] { 5, 6, 7, 8 };
        }
    }

    private void Update()
    {
        resource = GameManager.instance.redResource;
        if (pointReds.GetComponent<NukePower>().isUsableRed || pointReds.GetComponent<mindControl>().isUsableRed)
        {
            blueObjectsInRedSide = CountBlueObjectsInRedSide();

            if (blueObjectsInRedSide > 30)
            {
                if (pointReds.GetComponent<mindControl>().isUsableRed && pointReds.GetComponent<NukePower>().isUsableRed) {
                    pointReds.GetComponent<mindControl>().mindControlR();
                }
                else if (pointReds.GetComponent<mindControl>().isUsableRed && !pointReds.GetComponent<NukePower>().isUsableRed)
                {
                    pointReds.GetComponent<mindControl>().mindControlR();
                }
                else if(!pointReds.GetComponent<mindControl>().isUsableRed && pointReds.GetComponent<NukePower>().isUsableRed)
                {
                    if(blueObjectsInRedSide > 50) pointReds.GetComponent<NukePower>().NukeR(0);
                }
            }
        }

        if (!updateFlag) return;

        if (PoolManagerRed.instance.selectedPrefabsRed.Count < 8)
        {
            if (GameManager.instance.redCount > GameManager.instance.blueCount + 2)
            {
                int[] rand = new int[] { 0, 3 };
                randomCnt = rand[Random.Range(0, rand.Length)];
            }
            else randomCnt = 0;
        }
        else if (PoolManagerRed.instance.selectedPrefabsRed.Count < 16)
        {
            if (lastP != 3 && lastP != 4 && lastP != 5)
            {
                int[] rand = new int[] { 0, 1, 3, 4, 5 };
                randomCnt = rand[Random.Range(0, rand.Length)];
            }
            else
            {
                randomCnt = Random.Range(0, 2);
            }
        }
        else
        {
            if (lastP != 3 && lastP != 4 && lastP != 5)
            {
                randomCnt = Random.Range(0, 6);
            }
            else
            {
                randomCnt = Random.Range(0, 3);
            }
        }

        lastP = randomCnt;

        switch (randomCnt)
        {
            case 0:
                updateFlag = false;
                StartCoroutine(P0());
                break;
            case 1:
                updateFlag = false;
                StartCoroutine(P1());
                break;
            case 2:
                updateFlag = false;
                StartCoroutine(P2());
                break;
            case 3:
                if (GameManager.instance.redIncreaseLevel < 7)
                {
                    updateFlag = false;
                    StartCoroutine(P3());
                }
                break;
            case 4:
                if (UpgradeManager.instance.redDmgLvl < 7)
                {
                    updateFlag = false;
                    StartCoroutine(P4());
                }
                break;
            case 5:
                if (UpgradeManager.instance.redDefLvl < 7)
                {
                    updateFlag = false;
                    StartCoroutine(P5());
                }
                break;
            default:
                updateFlag = false;
                StartCoroutine(P0());
                break;
        }
    }

    IEnumerator P0() //~500 À¯´Ö
    {
        Debug.Log("P0 Selected");

        int bid = cheap[Random.Range(0, cheap.Length)];
        GameObject prefab = nowPrefabs[bid];
        targetResource = prefab.GetComponent<MobController>().cost;

        yield return new WaitUntil(() => resource >= targetResource);

        PoolManagerRed.instance.OnButtonClick(bid);

        updateFlag = true;
        Debug.Log("P0 End");
    }

    IEnumerator P1() //~1000À¯´Ö
    {
        Debug.Log("P1 Selected");

        int bid = mid[Random.Range(0, mid.Length)];
        GameObject prefab = nowPrefabs[bid];
        targetResource = prefab.GetComponent<MobController>().cost;

        yield return new WaitUntil(() => resource >= targetResource);

        PoolManagerRed.instance.OnButtonClick(bid);

        updateFlag = true;
        Debug.Log("P1 End");
    }

    IEnumerator P2() //1000~ À¯´Ö
    {
        Debug.Log("P2 Selected");

        int bid = expensive[Random.Range(0, expensive.Length)];
        GameObject prefab = nowPrefabs[bid];
        targetResource = prefab.GetComponent<MobController>().cost;

        yield return new WaitUntil(() => resource >= targetResource);

        PoolManagerRed.instance.OnButtonClick(bid);

        updateFlag = true;
        Debug.Log("P2 End");
    }

    IEnumerator P3() // °ñµå¾÷, ÃÖ´ë·¾ ¾Æ·¡¿¡¸¸ È£ÃâµÊ
    {
        Debug.Log("P3 Selected");

        targetResource = GameManager.instance.upgradeCostArr[GameManager.instance.redIncreaseLevel];

        yield return new WaitUntil(() => resource >= targetResource);

        GameManager.instance.RedGoldUpgrade();

        updateFlag = true;
        Debug.Log("P3 End");
    }

    IEnumerator P4() //°ø¾÷, ÃÖ´ë·¾ ¾Æ·¡¿¡¸¸ È£ÃâµÊ
    {
        Debug.Log("P4 Selected");

        targetResource = UpgradeManager.instance.dmgCost[UpgradeManager.instance.redDmgLvl];

        yield return new WaitUntil(() => resource >= targetResource);

        UpgradeManager.instance.dmgBtnRed();

        updateFlag = true;
        Debug.Log("P4 End");
    }

    IEnumerator P5() //¹æ¾÷, ÃÖ´ë·¾ ¾Æ·¡¿¡¸¸ È£ÃâµÊ
    {
        Debug.Log("P5 Selected");

        targetResource = UpgradeManager.instance.defCost[UpgradeManager.instance.redDefLvl];

        yield return new WaitUntil(() => resource >= targetResource);

        UpgradeManager.instance.defBtnRed();

        updateFlag = true;
        Debug.Log("P5 End");
    }

    public int CountBlueObjectsInRedSide()
    {
        GameObject[] blueObjects = GameObject.FindGameObjectsWithTag("Blue");

        int numberOfBlueObjects = 0;

        foreach (GameObject blueObject in blueObjects)
        {
            if (blueObject.transform.position.x > 0 && (blueObject.layer==7 || blueObject.layer==10))
            {
                numberOfBlueObjects++;
            }
        }

        return numberOfBlueObjects;
    }
}