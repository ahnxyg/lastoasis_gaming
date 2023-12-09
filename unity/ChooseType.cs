using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseType : MonoBehaviour
{
    public static ChooseType Instance = null;
    public string type = "Type1";
    public string enemyType;
    public string abilityName = "Nuke";
    public AudioSource aud;
    public int level;

    private void Start()
    {
        int randomType = Random.Range(1, 4);
        abilityName = "Nuke";

        // 선택된 무작위 값에 따라서 처리를 수행
        switch (randomType)
        {
            case 1:
                enemyType = "Type1";
                break;
            case 2:
                enemyType = "Type2";
                break;
            case 3:
                enemyType = "Type3";
                break;
            default:
                Debug.LogError("Invalid randomType");
                break;
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}
