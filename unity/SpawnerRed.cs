using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRed : MonoBehaviour
{
    public static SpawnerRed instance;

    public float minX = 25.0f; // x 최소값
    public float maxX = 30.0f; // x 최대값
    public float minY = -5.0f; // y 최소값
    public float maxY = 5.0f; // y 최대값

    public Transform parentObject;
    public GameObject shadowObject;

    public List<GameObject> selectedPrefabsRed = new List<GameObject>();

    public int mobCount;

    float spawnTime;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        spawnTime = GameManager.instance.spawnTime;
        // spawnTime 간격으로 SpawnPrefab 함수 호출 시작
        InvokeRepeating("SpawnPrefab", 0f, spawnTime);
    }

    private void Update()
    {
        mobCount = parentObject.childCount;
    }

    // 주기적으로 호출될 함수
    void SpawnPrefab()
    {
        selectedPrefabsRed = PoolManagerRed.instance.selectedPrefabsRed;
        foreach (GameObject prefab in selectedPrefabsRed)
        {
            if (prefab != null)
            {
                // 랜덤한 x, y 좌표 생성
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);

                // 소환 로직: 프리팹을 랜덤한 위치에 소환
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
                GameObject instantiatedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
                instantiatedPrefab.GetComponent<MobController>();
                
                instantiatedPrefab.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); //몬스터 크기 조정
                instantiatedPrefab.transform.SetParent(parentObject);

                //태그 변경
                instantiatedPrefab.tag = "Red";

                //그림자 소환
                GameObject shadow = Instantiate(shadowObject, instantiatedPrefab.transform.position, Quaternion.identity);
                shadow.transform.SetParent(instantiatedPrefab.transform);
            }
        }
    }
}