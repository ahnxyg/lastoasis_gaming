using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using Unity.MLAgents.Policies;
using System;


public class TypeAgent : Agent
{
    public enum Team
    {
        Blue = 0,
        Red = 1
    }
    public static int count = 0;
    public bool image_state;

    public Building MySilo;
    public Building MyNexus;

    public Building EnemySilo;
    public Building EnemyNexus;

    [HideInInspector]
    public Team team;

    [HideInInspector]
    public BehaviorParameters b_Params;

    [SerializeField]
    private Transform targetTransform;

    [HideInInspector]
    public Camera observationCamera; // 카메라 오브젝트를 연결하기 위한 public 변수

    [HideInInspector]
    private RenderTexture observationTexture; // 렌더 텍스쳐를 저장할 변수

    private bool isLearning = true;
    private float isUltimateUsed = 0;
    private float reward = 0;

    private float mySiloMaxHealth;
    private float myNexusMaxHealth;

    private float enemySiloMaxHealth;
    private float enemyNexusMaxHealth;

    public override void Initialize()
    {
        this.eval();
        this.mySiloMaxHealth = this.MySilo.maxHealth;
        this.myNexusMaxHealth = this.MyNexus.maxHealth;

        this.enemySiloMaxHealth = this.EnemySilo.maxHealth;
        this.enemyNexusMaxHealth = this.EnemyNexus.maxHealth;

        b_Params = GetComponent<BehaviorParameters>();
    }

    /*public override void OnActionReceived(ActionBuffers actions)
    {
        bool terminal = Building.Terminal;

        if (!terminal)
        {
            if (this.MaxStep == 0)
            {
                this.MaxStep = int.MaxValue;
            }

            if (this.MaxStep <= this.StepCount)
            {
                terminal = true;
            }
            else
            {
                terminal = false;
            }
        }

        if (this.isLearning)
        {
            var agentAction = actions.DiscreteActions[0];

            float reward = -1;

            int oneStepActionReward = -1 * (LearningManager.actions[this.b_Params.TeamId][agentAction](this.b_Params.BehaviorName));
            //if (oneStepActionReward != 2)
            //{
            //    reward = reward + oneStepActionReward;
            //}

            reward = reward + this.ActionMaskingReward(agentAction);

            string team;
            if (this.b_Params.TeamId == (int) (Team.Red))
            {
                team = "Red";
            }
            else
            {
                team = "Blue";
            }

            if (terminal)
            {
                float onGameReward = this.AgainstReward(agentAction);
                reward = reward + onGameReward;

                this.reward = this.reward + reward;
                this.reward = this.reward / this.StepCount;
                SetReward(this.reward);
                TypeAgent.count++;
                this.DisableCamera();
                this.eval();
                this.reward = 0;

            }
            else
            {
                //SetReward(reward);
            }
            Debug.Log($"{team} Team, {this.b_Params.BehaviorName} | Reward: {reward} | Cumulative Reward: {this.GetCumulativeReward()} | StepCount/MaxStep: {this.StepCount}");
        }
        else
        {
            if (terminal)
            {
                this.DisableCamera();
                this.eval();
            }
        }
    }*/

    float ActionMaskingReward(int agentAction)
    {
        float reward = 0;

        if (agentAction == 12)
        {
            if (this.isUltimateUsed == 0)
            {
                //int enemyCount;
                //if (this.b_Params.TeamId == (int)Team.Blue)
                //{
                //    enemyCount = GameManager.instance.redCount;
                //}
                //else
                //{
                //    enemyCount = GameManager.instance.blueCount;
                //}
                //if (enemyCount == 0)
                //{
                //    reward = reward - 100;
                //}
                this.isUltimateUsed = 1;
            }
        }

        if (agentAction == 13)
        {
            if (this.b_Params.TeamId == (int) Team.Red)
            {
                reward = reward - 1;
            }
        }

        if (this.isUltimateUsed == 1)
        {
            reward = reward - 1;
        }

        return reward;
    }

    private float AgainstReward(int agentAction)
    {

        float defensePenalty = -1 * (((this.myNexusMaxHealth - this.MyNexus.currentHealth) / this.myNexusMaxHealth) + ((this.mySiloMaxHealth - this.MySilo.currentHealth) / this.mySiloMaxHealth)) / 2;
        float attackReward = ((this.enemyNexusMaxHealth - this.EnemyNexus.currentHealth) / this.enemyNexusMaxHealth + (this.enemySiloMaxHealth - this.EnemySilo.currentHealth) / this.enemySiloMaxHealth) / 2;

        return ((defensePenalty + attackReward) / 2) / this.StepCount;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        if (this.isLearning || !this.isLearning)
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
                if (Input.GetKey(KeyCode.X))
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

                float myTeam = this.b_Params.TeamId;

                float myNexusHealth = this.MyNexus.currentHealth;
                float mySiloHealth = this.MySilo.currentHealth;

                float enemyNexusHealth = this.EnemyNexus.currentHealth;
                float enemySiloHealth = this.EnemySilo.currentHealth;

                float myResource;
                int myCount;
                int enemyCount;
                if (this.b_Params.TeamId == (int) Team.Blue)
                {
                    myResource = GameManager.instance.resource;

                    myCount = GameManager.instance.blueCount;
                    enemyCount = GameManager.instance.redCount;
                }
                else
                {
                    myResource = GameManager.instance.redResource;

                    myCount = GameManager.instance.redCount;
                    enemyCount = GameManager.instance.blueCount;
                }

                sensor.AddObservation(time);

                sensor.AddObservation(myTeam);
                sensor.AddObservation(this.isUltimateUsed);

                sensor.AddObservation(myNexusHealth);
                sensor.AddObservation(enemyNexusHealth);

                sensor.AddObservation(enemySiloHealth);
                sensor.AddObservation(enemySiloHealth);

                sensor.AddObservation(myResource);

                sensor.AddObservation(myCount);
                sensor.AddObservation(enemyCount);
            }
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
        //}
    }

    public override void OnEpisodeBegin()
    {
    }

    public void EnableCamera()
    {
        this.observationCamera = GetComponent<Camera>();
        // 렌더 텍스쳐 초기화
        this.observationTexture = new RenderTexture(this.observationCamera.pixelWidth, this.observationCamera.pixelHeight, 32); // 적절한 해상도와 깊이를 선택

        this.observationTexture.Create();

        this.observationCamera.targetTexture = observationTexture;
    }

    public void DisableCamera()
    {
        this.observationCamera = null;
        this.observationTexture = null;
    }

    public void eval()
    {
        this.isLearning = false;
    }

    public void train()
    {
        this.isLearning = true;
    }
}
