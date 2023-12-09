using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    //asdalksa
    public static GameManager instance;

    [Header("# Game Control")]
    public float gameTime;
    public int startResource;
    public int resource; //????
    public int resourceIncrease = 5; //???? ??????
    public bool isLive; //???? ?????????? true, ?????? false
    public float spawnTime;
    public Result uiResult;
    public string currentSceneName;
    public float timeValue;
    public Button timeBtn;
    public Text timeText;
    [Header("# Counter")]
    public int redCount;
    public int blueCount;
    public string type;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI redUpgradeText;
    [Header("# BlueUpgrade")]
    public int upgradeLevel;
    private int upgradeCost;
    private int initialUpgrade;
    [Header("# RedUpgrade")]
    public int redResource;
    public int redResourceIncrease = 5;
    public int redUpgradeLevel;
    public int redUpgradeCost;
    public Dictionary<string, int> totalDmg = new Dictionary<string, int>();
    public TextMeshProUGUI[] dmgTexts;

    public bool IsGameOver { set; get; } = false;  //게임이 졌는가의 여부
    public bool IsGameWon { set; get; } = false;  // 이겼는가의 여부

    private float rate = 1.6f;

    public int[] upgradeCostArr = new int[] { 300, 350, 400, 450, 500, 550, 600 };
    public int[] increaseArr = new int[] { 10, 12, 17, 24, 33, 47, 65 };
    public int[] waitTime = new int[] { 5, 10, 15, 20, 25, 30, 35 };
    public int increaseLevel = 0;
    public int redIncreaseLevel = 0;
    bool isIncomeStop;
    bool isRedIncomeStop;

    public int level;
    public float[] levelResource = new float[] { 1f, 1.5f, 2f };

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        timeValue = 1f;
        resource = startResource;
        level = ChooseType.Instance.level;
    }

    void Start()
    {
        timeValue = 1f;
        upgradeLevel = 1;
        redUpgradeLevel = 1;
        initialUpgrade = 200;
        StartCoroutine(Income());
        StartCoroutine(redIncome());
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName=="MainGameplay") type = ChooseType.Instance.type;
        else if(currentSceneName=="SceneType1") 
        isLive = true;
        //Debug.Log(type);
        upgradeText.text = upgradeCostArr[0].ToString();
        CalculateHeartIncrease();
    }

    public void GoldUpgrade()
    {
        if(isIncomeStop) return;

        if (resource >= upgradeCostArr[increaseLevel])
        {
            if (increaseLevel>= 7) return;
            
            resource -= upgradeCostArr[increaseLevel];
            resourceIncrease = increaseArr[increaseLevel];
            if (increaseLevel < 7) increaseLevel++;
            upgradeText.text = upgradeCostArr[increaseLevel].ToString();
            StartCoroutine(incomeStop());
        }
    }


    public void RedGoldUpgrade()
    {
        if (isRedIncomeStop) return;

        if (redResource >= upgradeCostArr[redIncreaseLevel])
        {
            if (redIncreaseLevel >= 7) return;

            redResource -= upgradeCostArr[redIncreaseLevel];
            redResourceIncrease = increaseArr[redIncreaseLevel];
            if (redIncreaseLevel < 7) redIncreaseLevel++;
            upgradeText.text = upgradeCostArr[redIncreaseLevel].ToString();
            StartCoroutine(redIncomeStop());
        }
    }

    IEnumerator incomeStop()
    {
        isIncomeStop = true;
        int temp = resourceIncrease;
        resourceIncrease = 0;
        yield return new WaitForSeconds(waitTime[increaseLevel]);
        resourceIncrease = temp;
        isIncomeStop = false;
    }

    IEnumerator redIncomeStop()
    {
        isRedIncomeStop = true;
        int temp = redResourceIncrease;
        redResourceIncrease = 0;
        yield return new WaitForSeconds(waitTime[redIncreaseLevel]);
        redResourceIncrease = temp;
        isIncomeStop = false;
    }


    public void GameOver()
    {
        if (IsGameOver) return; // Prevent multiple calls
        IsGameOver = true;
        SaveCurrentTime(); // 현재 시간 저장
        StartCoroutine(GameOverRoutine());
        UpdateExperienceAndLevel(25); // From GameController
        UpdateGold(1);
        UpdateHeart(1);
        // 데이터 업데이트
        BackendGameData.Instance.GameDataUpdate(() =>
        {
            // 데이터 업데이트 완료 후 랭킹 업데이트
            BackendGameData.Instance.UpdateUserRanking();
        });
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        if (IsGameWon) return; // Prevent multiple calls
        IsGameWon = true;
        SaveCurrentTime(); // 현재 시간 저장
        StartCoroutine(GameVictoryRoutine());
        UpdateExperienceAndLevel(50); // From GameController
        UpdateGold(2);
        UpdateHeart(1);
        // 데이터 업데이트
        BackendGameData.Instance.GameDataUpdate(() =>
        {
            // 데이터 업데이트 완료 후 랭킹 업데이트
            BackendGameData.Instance.UpdateUserRanking();
        });
    }



    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }



    // 게임 종료 또는 승리 시 현재 시간을 저장
    private void SaveCurrentTime()
    {
        PlayerPrefs.SetString("LastSessionTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }


    private void UpdateExperienceAndLevel(int experienceGain)
    {
        BackendGameData.Instance.UserGameData.experience += experienceGain;
        if (BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }
    }

    private void CalculateHeartIncrease()
    {
        if (PlayerPrefs.HasKey("LastSessionTime"))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("LastSessionTime"));
            DateTime lastSessionTime = DateTime.FromBinary(temp);
            TimeSpan timeDifference = DateTime.Now - lastSessionTime;

            int heartIncrease = (int)(timeDifference.TotalMinutes / 5); // 5분당 하트 1개 증가
            IncreaseHeart(heartIncrease);
        }
    }

    private void IncreaseHeart(int heartIncrease)
    {
        var userGameData = BackendGameData.Instance.UserGameData;
        userGameData.heart = Mathf.Min(userGameData.heart + heartIncrease, 30); // 최대 30개까지만 증가
    }


    private void UpdateGold(int goldGain)
    {
        BackendGameData.Instance.UserGameData.gold += goldGain;
    }

    private void UpdateHeart(int heartGain)
    {
        BackendGameData.Instance.UserGameData.heart -= heartGain;
    }


    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }


    public void ReturnLobby()
    {
        SceneManager.LoadScene("GameStart");
    }

    public void TimeOnOff()
    {
        if (timeValue == 1f)
        {
            timeValue = 2f;
            timeText.text = "현재: 2배속";
        }
        else
        {
            timeValue = 1f;
            timeText.text = "현재: 1배속";
        }
    }

    void Update()
    {
        if (!isLive) return;
        Time.timeScale = timeValue;
        gameTime += Time.deltaTime;
        //???? ?????? ?? ?????????? ???? ????
        //blueCount, redCount ?? ????????
        blueCount = Spawner.instance.mobCount;
        redCount = SpawnerRed.instance.mobCount;
    }

    IEnumerator Income()
    {
        /*while (true)
        {
            //?????? ???? -> ???????? ???????????? resourceIncrese ??????????
            if (isLive)
            {
                 
                int nextResource = resource + Mathf.FloorToInt(resourceIncrease);
                while (resource != nextResource)
                {
                    resource++; //?????? ???????? ???? ??????????
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForSeconds((60f-resourceIncrease) / 60f);
        }*/
        while (true)
        {
            yield return new WaitForSeconds(1);
            resource += resourceIncrease;
        }
    }
    IEnumerator redIncome()
    {
        /*while (true)
        {
            //?????? ???? -> ???????? ???????????? resourceIncrese ??????????
            if (isLive)
            {

                int nextResource = redResource + Mathf.FloorToInt(redResourceIncrease);
                while (redResource != nextResource)
                {
                    redResource++; //?????? ???????? ???? ??????????
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForSeconds((60f - redResourceIncrease) / 60f);
        }*/
        while (true)
        {
            yield return new WaitForSeconds(1);
            redResource += (int)(redResourceIncrease * levelResource[level]);
        }
    }
}
