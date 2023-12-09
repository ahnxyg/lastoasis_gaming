using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using Unity.MLAgents.Policies;
using System;
using Random = UnityEngine.Random;


public class RedAgent : Agent
{
    [HideInInspector]
    public bool Terminal = false;

    private static bool initialized = false;

    public static RedAgent instance;

    public bool image_state;

    public Building MySilo;
    public Building MyNexus;

    private float previousSIloHP;
    private float previousNexusHP;
    
    private float previousEnemySiloHP;
    private float previousEnemyNexusHP;

    public Building EnemySilo;
    public Building EnemyNexus;

    [SerializeField]
    private Transform targetTransform;

    //[HideInInspector]
    public Camera observationCamera; // 카메라 오브젝트를 연결하기 위한 public 변수

    [HideInInspector]
    private RenderTexture observationTexture; // 렌더 텍스쳐를 저장할 변수

    private int type;
    private string[] Types = { "Type1", "Type2", "Type3" };

    private float isUltimateUsed = 0;
    public bool brainShare = false;

    private float mySiloMaxHealth;
    private float myNexusMaxHealth;

    private float enemySiloMaxHealth;
    private float enemyNexusMaxHealth;

    private int previousEnemyMobsCount = 0;
    private int previousMyMobsCount = 0;

    private AgentManager agentManager;

    /*protected override void Awake()
    {
        return;
    }*/

    private void Start()
    {
        RedAgent.instance = this;
    }

    public override void Initialize()
    {
        if (RedAgent.initialized)
        {
            return;
        }
        RedAgent.initialized = true;
        this.mySiloMaxHealth = this.MySilo.maxHealth;
        this.myNexusMaxHealth = this.MyNexus.maxHealth;

        this.enemySiloMaxHealth = this.EnemySilo.maxHealth;
        this.enemyNexusMaxHealth = this.EnemyNexus.maxHealth;

        this.previousNexusHP = this.myNexusMaxHealth;
        this.previousSIloHP = this.mySiloMaxHealth;

        this.previousEnemyNexusHP = this.enemyNexusMaxHealth;
        this.previousEnemySiloHP = this.enemySiloMaxHealth;

        if (this.image_state)
        {
            this.EnableCamera();
        }
        this.agentManager = GetComponent<AgentManager>();
    }

    /*public override void OnActionReceived(ActionBuffers actions)
    {
        int terminal = this.agentManager.DetermineTerminal();

        var agentAction = actions.DiscreteActions[0];
        if (this.brainShare && agentAction == 13)
        {
            agentAction = 0;
        }

        float reward = this.ActionReward(agentAction);

        float onGameReward = this.AgainstReward(agentAction);
        reward = reward + onGameReward * 3;

        string team = "Red";
        this.previousMyMobsCount = GameManager.instance.redCount;
        this.previousEnemyMobsCount = GameManager.instance.blueCount;

        this.previousEnemyNexusHP = this.EnemyNexus.currentHealth;
        this.previousEnemySiloHP = this.EnemySilo.currentHealth;

        this.previousNexusHP = this.MyNexus.currentHealth;
        this.previousSIloHP = this.MySilo.currentHealth;

        if (terminal > 0)
        {
            //float onGameReward = this.AgainstReward(agentAction);
            //reward = reward + onGameReward;

            //this.reward = this.reward + reward;
            //this.reward = this.reward / this.StepCount;
            reward = reward + this.FinalReward();
            if (terminal == 2)
            {
                reward = reward - 100;
            }
            this.Terminal = true;
            SetReward(reward / FPS.frames);
        }
        else
        {
            SetReward(reward);
        }
        Debug.Log($"{team} Team, {this.Types[this.type]} | Reward: {reward} | Cumulative Reward: {this.GetCumulativeReward()} | StepCount: {this.StepCount} | frames: {FPS.frames} | FPS: {FPS.fps}");
    }*/
    private void Update()
    {
    }

    private float FinalReward()
    {
        float finHP = (this.enemyNexusMaxHealth - this.EnemyNexus.currentHealth + this.enemySiloMaxHealth - this.EnemySilo.currentHealth)
            - (this.myNexusMaxHealth - this.MyNexus.currentHealth + this.mySiloMaxHealth - this.MySilo.currentHealth);
        return finHP;
    }

    private float ActionReward(int agentAction)
    {
        float reward = 0;

        int oneStepActionReward = -1 * this.agentManager.SubmitAction(1, this.Types[this.type], agentAction);
        if (oneStepActionReward != 2)
        {
            reward = reward + oneStepActionReward;
        }

        if (agentAction == 12)
        {
            if (this.isUltimateUsed == 0)
            {
                this.isUltimateUsed = 1;
            }
        }
        else
        {
            //if (this.isUltimateUsed == 1)
            //{
            //    reward = reward - 1;
            //}
            //else
            //{
            //    reward = reward + 1;
            //}
        }
        return reward;
    }

    private float AgainstReward(int agentAction)
    {

        float defensePenalty = -1 * ((this.previousNexusHP - this.MyNexus.currentHealth) + (this.previousSIloHP - this.MySilo.currentHealth)) / 2;
        float attackReward = (this.previousEnemyNexusHP - this.EnemyNexus.currentHealth + this.previousEnemySiloHP - this.EnemySilo.currentHealth) / 2;

        int killedPenalty;
        int killingReward;

        if (this.previousMyMobsCount > GameManager.instance.redCount)
        {
            killedPenalty = -1;
        }
        else
        {
            killedPenalty = 0;
        }

        if (this.previousEnemyMobsCount > GameManager.instance.blueCount)
        {
            killingReward = 1;
        }
        else if (this.previousEnemyMobsCount < GameManager.instance.blueCount)
        {
            killingReward = -1;
        }
        else
        {
            killingReward = 0;
        }

        int myCount;
        if (GameManager.instance.redCount == 0)
        {
            myCount = 1;
        }
        else
        {
            myCount = GameManager.instance.redCount;
        }

        int enemyCount;
        if (GameManager.instance.blueCount == 0)
        {
            enemyCount = 1;
        }
        else
        {
            enemyCount = GameManager.instance.blueCount;
        }

        return (defensePenalty + attackReward) + (killedPenalty * (killedPenalty / enemyCount) + killingReward * (killingReward / myCount)) / 2;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (this.image_state)
        {
            // 렌더 텍스쳐에서 관찰 데이터로 변환
            Texture2D observation = ObservationToTexture(observationTexture);

            observation = this.ScaleTexture(observation, 224, 224);

            var instanceId = this.name;
            // 이미지를 저장할 경로 설정
            string imagePath = $"D:/source/RLs/results/observations/{instanceId}/";

            // 프레임마다 이미지 저장
            if (Input.GetKey(KeyCode.S))
            {
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }
                CaptureObservations(observation, imagePath + $"observation_{this.StepCount}.png");
            }


            // 관찰 데이터를 VectorSensor에 추가
            sensor.AddObservation(observation);
        }
        else
        {
            float time = GameManager.instance.gameTime;

            float myTeam = 1;

            float myNexusHealth = this.MyNexus.currentHealth;
            float mySiloHealth = this.MySilo.currentHealth;

            float enemyNexusHealth = this.EnemyNexus.currentHealth;
            float enemySiloHealth = this.EnemySilo.currentHealth;

            float myResource;
            int myCount;
            int enemyCount;

            myResource = GameManager.instance.redResource;

            myCount = GameManager.instance.redCount;
            enemyCount = GameManager.instance.blueCount;

            sensor.AddObservation(time);

            sensor.AddObservation(myTeam);
            sensor.AddObservation(this.type);

            float blueCanUse;
            if (NukePower.instance[0].isUsable)
            {
                blueCanUse = 1;
            }
            else
            {
                blueCanUse = 0;
            }
            float blueCanUseMC;

            if (mindControl.Instance.isUsable)
            {
                blueCanUseMC = 1;
            }
            else
            {
                blueCanUseMC = 0;
            }
            sensor.AddObservation(blueCanUseMC);

            sensor.AddObservation(blueCanUse);
            sensor.AddObservation(this.isUltimateUsed);

            sensor.AddObservation(myNexusHealth);
            sensor.AddObservation(enemyNexusHealth);

            sensor.AddObservation(mySiloHealth);
            sensor.AddObservation(enemySiloHealth);

            sensor.AddObservation(myResource);

            sensor.AddObservation(myCount);
            sensor.AddObservation(enemyCount);
        }
    }

    // RenderTexture를 Texture2D로 변환하는 함수
    private Texture2D ObservationToTexture(RenderTexture rt)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentActiveRT;
        return tex;
    }

    private void CaptureObservations(Texture2D image, string path)
    {
        // 이미지를 파일로 저장
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log($"Saved image to {path}");
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //if (this.isLearning)
        //{
        //    if (this.b_Params.TeamId == (int) Team.Blue)
        //    {
        //        var heuristicAction = actionsOut.DiscreteActions;

        //        if (Input.GetKeyUp(KeyCode.Alpha1))
        //        {
        //            heuristicAction[0] = 1;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha2))
        //        {
        //            heuristicAction[0] = 2;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha3))
        //        {
        //            heuristicAction[0] = 3;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha4))
        //        {
        //            heuristicAction[0] = 4;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha5))
        //        {
        //            heuristicAction[0] = 5;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha6))
        //        {
        //            heuristicAction[0] = 6;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha7))
        //        {
        //            heuristicAction[0] = 7;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha8))
        //        {
        //            heuristicAction[0] = 8;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha9))
        //        {
        //            heuristicAction[0] = 9;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha0))
        //        {
        //            heuristicAction[0] = 10;
        //        }

        //        else if (Input.GetKey(KeyCode.Minus))
        //        {
        //            heuristicAction[0] = 11;
        //        }
        //        else if (Input.GetKey(KeyCode.Plus))
        //        {
        //            heuristicAction[0] = 12;
        //        }
        //        else if (Input.GetKey(KeyCode.Backspace))
        //        {
        //            heuristicAction[0] = 13;
        //        }
        //        else
        //        {
        //            heuristicAction[0] = 0;
        //        }
        //    }
        //    // Red 팀일 때 추가 구현 (예정)
        ////}
        //if (this.MyTeam == 0)
        //{

        //    var heuristicAction = actionsOut.DiscreteActions;

        //    if (Input.GetKeyUp(KeyCode.Alpha1))
        //    {
        //        heuristicAction[0] = 1;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha2))
        //    {
        //        heuristicAction[0] = 2;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha3))
        //    {
        //        heuristicAction[0] = 3;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha4))
        //    {
        //        heuristicAction[0] = 4;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha5))
        //    {
        //        heuristicAction[0] = 5;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha6))
        //    {
        //        heuristicAction[0] = 6;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha7))
        //    {
        //        heuristicAction[0] = 7;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha8))
        //    {
        //        heuristicAction[0] = 8;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha9))
        //    {
        //        heuristicAction[0] = 9;
        //    }

        //    else if (Input.GetKey(KeyCode.Alpha0))
        //    {
        //        heuristicAction[0] = 10;
        //    }

        //    else if (Input.GetKey(KeyCode.Minus))
        //    {
        //        heuristicAction[0] = 11;
        //    }
        //    else if (Input.GetKey(KeyCode.Plus))
        //    {
        //        heuristicAction[0] = 12;
        //    }
        //    else if (Input.GetKey(KeyCode.Backspace))
        //    {
        //        heuristicAction[0] = 13;
        //    }
        //    else
        //    {
        //        heuristicAction[0] = 0;
        //    }

        //}
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("RedAgent Episode Begin");
        this.type = Random.Range(0, 3);
        this.previousEnemyMobsCount = 0;
        this.previousMyMobsCount = 0;
        this.Terminal = false;
        this.previousNexusHP = this.myNexusMaxHealth;
        this.previousSIloHP = this.mySiloMaxHealth;

        this.previousEnemyNexusHP = this.enemyNexusMaxHealth;
        this.previousEnemySiloHP = this.enemySiloMaxHealth;
    }

    public void EnableCamera()
    {
        //this.observationCamera = GetComponent<Camera>();

        // 렌더 텍스쳐 초기화
        this.observationTexture = new RenderTexture(this.observationCamera.pixelWidth, this.observationCamera.pixelHeight, 32); // 적절한 해상도와 깊이를 선택

        this.observationTexture.Create();

        //this.observationCamera.targetTexture = observationTexture;
    }

    public void DisableCamera()
    {
        this.observationCamera = null;
        this.observationTexture = null;
    }
}
