using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukePower : MonoBehaviour
{
    public static NukePower[] instance = new NukePower[2];
    public static NukePower instance2;

    public bool isUsable;
    public bool isUsableRed;

    public int redKillCount = 0; // 레드가 죽인 수
    public float redTotalDmg = 0;

    public int blueKillCount = 0; // 블루가 죽인 수
    public float blueTotalDmg = 0;
    
    public GameObject[] Point1;
    public GameObject[] Point2;
    public GameObject[] Point3;
    public GameObject[] Point4;
    public GameObject[] Point5;
    public GameObject[] Point6;
    public GameObject[] Point7;
    public GameObject[] Point8;
    public GameObject[] Point9;
    public GameObject[] Point10;
    public GameObject explode;

    private float delayBetweenGroups = 0.2f; // 그룹 간의 간격

    public void Awake()
    {
        instance2 = this;
        if (this.gameObject.name == "Points")
        {
            instance[0] = this;
        }
        else if (this.gameObject.name == "PointsRed")
        {
            instance[1] = this;
        }
    }

    public void Nuke(int who)
    {
        if (!isUsable) return;

        StartCoroutine(DestroyAndInstantiateWithDelay(who));
        isUsable = false;
    }

    public void NukeR(int who)
    {
        if (!isUsableRed) return;

        StartCoroutine(DestroyAndInstantiateWithDelay(who));
        isUsableRed = false;
    }

    private IEnumerator DestroyAndInstantiateWithDelay(int temp)
    {
        if ((gameObject.name == "PointsRed" && temp == 0) || temp == 1)
        {
            foreach (GameObject p in Point10)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }

            yield return new WaitForSeconds(delayBetweenGroups); // 코루틴 간의 간격
            foreach (GameObject p in Point9)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point8)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point7)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point6)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point5)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point4)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point3)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point2)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point1)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
        }
        else if ((gameObject.name == "Points" && temp == 0) || temp == 2)
        {
            foreach (GameObject p in Point1)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }

            yield return new WaitForSeconds(delayBetweenGroups); // 코루틴 간의 간격
            foreach (GameObject p in Point2)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point3)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point4)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point5)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point6)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point7)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point8)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point9)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
            foreach (GameObject p in Point10)
            {
                Instantiate(explode, p.transform.position, p.transform.rotation);

            }
            yield return new WaitForSeconds(delayBetweenGroups);
        }
    }
}
