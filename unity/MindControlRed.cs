using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mindControlRed : MonoBehaviour
{
    public static mindControlRed Instance;
    public bool isUsable;
    public int redKillCount = 0;

    public void Awake()
    {
        Instance = this;
    }

    public void mindControlP()
    {
        if (!isUsable) return;

        // 모든 "red" 태그를 가진 객체를 찾음
        GameObject[] blueObjects = GameObject.FindGameObjectsWithTag("Blue");

        GameObject spawnedRed = GameObject.Find("SpawnedRed");

        // "red" 태그를 "blue"로 변경
        foreach (GameObject obj in blueObjects)
        {
            if (obj.layer == 7 && obj.name != "Wizard(Clone)" && obj.transform.position.x > 0)
            {
                redKillCount++;
                obj.gameObject.GetComponent<MobController>().selfTag = "Red";
                obj.tag = "Red";
                obj.transform.SetParent(spawnedRed.transform);
            }
        }

        isUsable = false;
    }
}
