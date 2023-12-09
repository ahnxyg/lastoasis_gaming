using UnityEngine;

public class mindControl : MonoBehaviour
{
    public static mindControl Instance;
    public bool isUsable;
    public bool isUsableRed;
    
    public int blueKillCount=0;
    public int redKillCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void mindControlP()
    {
        if (!isUsable) return;

        // 모든 "red" 태그를 가진 객체를 찾음
        GameObject[] redObjects = GameObject.FindGameObjectsWithTag("Red");

        // "SpawnedBlue" 오브젝트를 찾음
        GameObject spawnedBlue = GameObject.Find("SpawnedBlue");

        // "red" 태그를 "blue"로 변경하고 "SpawnedBlue"의 자식으로 이동
        foreach (GameObject obj in redObjects)
        {
            if ((obj.layer == 7 || obj.layer ==10)&& obj.name != "Knight(Clone)" && obj.transform.position.x < 0)
            {
                blueKillCount++;
                obj.gameObject.GetComponent<MobController>().selfTag = "Blue";
                obj.tag = "Blue";
                obj.transform.SetParent(spawnedBlue.transform);
            }
        }

        isUsable = false;
    }

    public void mindControlR()
    {
        if (!isUsableRed) return;

        GameObject[] blueObjects = GameObject.FindGameObjectsWithTag("Blue");
        GameObject spawnedRed = GameObject.Find("SpawnedRed");

        // "red" 태그를 "blue"로 변경하고 "SpawnedBlue"의 자식으로 이동
        foreach (GameObject obj in blueObjects)
        {
            if ((obj.layer == 7 || obj.layer==10) && obj.name != "Wizard(Clone)" && obj.transform.position.x > 0)
            {
                redKillCount++;
                obj.gameObject.GetComponent<MobController>().selfTag = "Red";
                obj.tag = "Red";
                obj.transform.SetParent(spawnedRed.transform);
            }
        }

        isUsableRed = false;
    }
}
