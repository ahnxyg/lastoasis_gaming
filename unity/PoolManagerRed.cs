using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManagerRed : MonoBehaviour
{
    public static PoolManagerRed instance;

    public List<GameObject> prefabsT1 = new List<GameObject>();
    public List<GameObject> prefabsT2 = new List<GameObject>();
    public List<GameObject> prefabsT3 = new List<GameObject>();
    public List<GameObject> selectedPrefabsRed = new List<GameObject>();

    private string type;

    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if (GameManager.instance.currentSceneName == "SceneType1") type = "Type1";
        else if (GameManager.instance.currentSceneName == "SceneType2") type = "Type2";
        else if (GameManager.instance.currentSceneName == "SceneType3") type = "Type3";
        else type = ChooseType.Instance.enemyType;
    }

    public void OnButtonClick(int buttonID)
    {
        if (type == "Type1")
        {
            if (buttonID >= 0 && buttonID < prefabsT1.Count)
            {
                GameObject prefab = prefabsT1[buttonID]; // 버튼 ID에 해당하는 프리팹을 가져옴
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.redResource >= cost)
                {
                    selectedPrefabsRed.Add(prefab); // 프리팹을 selectedPrefabs 리스트에 추가
                    GameManager.instance.redResource -= cost;
                    Debug.Log("Red Button " + buttonID + " clicked.");
                }
            }

            // 버튼이 클릭되었을 때 호출되는 함수
            // buttonID를 사용하여 어떤 프리팹을 할당할지 결정


            // 여기에서 allocedPrefab 리스트에 프리팹을 추가하는 로직을 추가하세요.
        }
        else if (type == "Type2")
        {
            if (buttonID >= 0 && buttonID < prefabsT2.Count)
            {
                GameObject prefab = prefabsT2[buttonID]; // 버튼 ID에 해당하는 프리팹을 가져옴
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.redResource >= cost)
                {
                    selectedPrefabsRed.Add(prefab); // 프리팹을 selectedPrefabs 리스트에 추가
                    GameManager.instance.redResource -= cost;
                    Debug.Log("Red Button " + buttonID + " clicked.");
                }
            }
            // 버튼이 클릭되었을 때 호출되는 함수
            // buttonID를 사용하여 어떤 프리팹을 할당할지 결정
        }
        else
        {
            if (buttonID >= 0 && buttonID < prefabsT3.Count)
            {
                GameObject prefab = prefabsT3[buttonID]; // 버튼 ID에 해당하는 프리팹을 가져옴
                int cost = prefab.GetComponent<MobController>().cost;
                if (GameManager.instance.redResource >= cost)
                {
                    selectedPrefabsRed.Add(prefab); // 프리팹을 selectedPrefabs 리스트에 추가
                    GameManager.instance.redResource -= cost;
                    Debug.Log("Red Button " + buttonID + " clicked.");
                }
            }
            // 버튼이 클릭되었을 때 호출되는 함수
            // buttonID를 사용하여 어떤 프리팹을 할당할지 결정
        }
    }
}