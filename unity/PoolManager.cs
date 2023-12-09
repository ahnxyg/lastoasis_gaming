using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public List<GameObject> prefabsT1 = new List<GameObject>();
    public List<GameObject> prefabsT2 = new List<GameObject>();
    public List<GameObject> prefabsT3 = new List<GameObject>();
    public List<GameObject> selectedPrefabsBlue = new List<GameObject>();
    public SpawnData[] mobStats;
    public TextMeshProUGUI[] mobCounts;


    public Dictionary<string, int> stack;

    public string type;

    void Awake()
    {
        instance = this;
        int randomType = Random.Range(1, 4);
        switch (randomType)
        {
            case 1:
                type = "Type1";
                break;
            case 2:
                type = "Type2";
                break;
            case 3:
                type = "Type3";
                break;
            default:
                Debug.LogError("Invalid randomType");
                break;
        }
    }
    private void Start()
    {
        stack = new Dictionary<string, int>();
        if (GameManager.instance.currentSceneName == "MainGameplay") type = ChooseType.Instance.type;
        Debug.Log(type);
        /*if (GameManager.instance.currentSceneName == "MainGameplay") type = ChooseType.Instance.type;
        else
        {
            int randomType = Random.Range(1, 4);
            switch (randomType)
            {
                case 1:
                    type = "Type1";
                    break;
                case 2:
                    type = "Type2";
                    break;
                case 3:
                    type = "Type3";
                    break;
                default:
                    Debug.LogError("Invalid randomType");
                    break;
            }
        }
        Debug.Log(type);*/
    }

    public void OnButtonClick(int buttonID)
    {
        if (type == "Type1")
        {
            //if (buttonID >= 0 && buttonID < prefabsT1.Count)
            //{

                GameObject prefab = prefabsT1[buttonID]; // 버튼 ID에 해당하는 프리팹을 가져옴
                int cost = prefab.GetComponent<MobController>().cost;
                

                if (GameManager.instance.resource >= cost)
                {
                    GameManager.instance.resource -= cost;
                    
                    string countT = mobCounts[buttonID].text.ToString().Substring(7);
                    int count = int.Parse(countT);
                    mobCounts[buttonID].text = "count: " + (count+1).ToString();
                    string spriteName = prefab.name;
                    bool keyExists = stack.ContainsKey(spriteName);
                    if (!keyExists)
                    {
                        stack.Add(spriteName, 1);
                    }
                    else
                    {

                        stack[spriteName] += 1;
                    }
                    selectedPrefabsBlue.Add(prefab); // 프리팹을 selectedPrefabs 리스트에 추가
                }
                
            //}
        }
        else if (type == "Type2")
        {
            //if (buttonID >= 0 && buttonID < prefabsT2.Count)
            //{
                GameObject prefab = prefabsT2[buttonID]; // 버튼 ID에 해당하는 프리팹을 가져옴
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.resource >= cost)
                {
                    GameManager.instance.resource -= cost;

                    string countT = mobCounts[buttonID].text.ToString().Substring(7);
                    int count = int.Parse(countT);
                    mobCounts[buttonID].text = "count: " + (count + 1).ToString();

                    string spriteName = prefab.name;
                    bool keyExists = stack.ContainsKey(spriteName);
                    if (!keyExists)
                    {
                        stack.Add(spriteName, 1);
                    }
                    else
                    {

                        stack[spriteName] += 1;
                    }
                    selectedPrefabsBlue.Add(prefab); // 프리팹을 selectedPrefabs 리스트에 추가
                }
            //}
        }
        else
        {
            //if (buttonID >= 0 && buttonID < prefabsT3.Count)
            //{
                GameObject prefab = prefabsT3[buttonID]; // 버튼 ID에 해당하는 프리팹을 가져옴
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.resource >= cost)
                {
                    GameManager.instance.resource -= cost;
                    string countT = mobCounts[buttonID].text.ToString().Substring(7);
                    int count = int.Parse(countT);
                    mobCounts[buttonID].text = "count: " + (count + 1).ToString();
                    string spriteName = prefab.name;
                    bool keyExists = stack.ContainsKey(spriteName);
                    if (!keyExists)
                    {
                        stack.Add(spriteName, 1);
                    }
                    else
                    {

                        stack[spriteName] += 1;
                    }
                    selectedPrefabsBlue.Add(prefab); // 프리팹을 selectedPrefabs 리스트에 추가
                }
            //}
        }
    }
}