using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using System.Threading;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class TeamAgent
{
    public TypeAgent[] agents = new TypeAgent[3];
    public void Start()
    {
        if (Regame.instance.resetCount > 1) return;
    }
}
public class LearningManager : MonoBehaviour
{
    public static InsufficientBalance[][] actions;
    public TeamAgent[] agents = new TeamAgent[2];

    [HideInInspector]
    public int[] agentType = new int[2];

    public float timeScale;
    private float defaultTimeScale;
    private int round = 0;

    private string[] types = {"Type1", "Type2", "Type3"};

    /*private void Awake()
    {
        GameManager.reloaded = true;
        if (timeScale <= 0.2f || timeScale >= 16)
        {
            timeScale = 1f;
        }
        this.defaultTimeScale = timeScale;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Learning Start");

        LearningManager.actions = new InsufficientBalance[2][];
        LearningManager.actions[1] = new InsufficientBalance[13];

        BlueActionSpace b_Btn = GetComponent<BlueActionSpace>();
        RedActionSpace r_Btn = GetComponent<RedActionSpace>();

        LearningManager.actions[0] = b_Btn.summonFunctions;
        LearningManager.actions[1] = r_Btn.summonFunctions;
        this.round++;
        this.OnBeginEpisode();
    }


    // Update is called once per frame
    /*void Update()
    {
        if (GameManager.reloaded)
        {
            Time.timeScale = this.timeScale;
            GameManager.reloaded = false;
        }

        this.AdjustTimeScale();

        if (Input.GetKeyDown(KeyCode.L)) // 한 에피소드가 진행되지 않는 것 같으면 강제로 에피소드 종료
        {
            this.EndEpisode();
            this.OnBeginEpisode();
            Regame.instance.ResetAll();
        }

        if (TypeAgent.count >= 2) // endepisode
        {
            this.EndEpisode();
            this.round++;
            this.OnBeginEpisode();
            Regame.instance.ResetAll();
        }
        //Debug.Log($"TimeScale: {Time.timeScale}, {this.timeScale}");
    }*/

    void OnBeginEpisode()
    {
        int blueTeamType = Random.Range(0, 3);
        int redTeamType = Random.Range(0, 3);

        Debug.Log($"Round{this.round} {this.types[blueTeamType]} vs. {this.types[redTeamType]}");

        TeamAgent blueTeamAgent = agents[0];
        TeamAgent redTeamAgent = agents[1];

        this.agentType[0] = blueTeamType;
        this.agentType[1] = redTeamType;

        blueTeamAgent.agents[blueTeamType].train();
        redTeamAgent.agents[redTeamType].train();

        blueTeamAgent.agents[blueTeamType].EnableCamera();
        redTeamAgent.agents[redTeamType].EnableCamera();
        TypeAgent.count = 0;
    }

    void EndEpisode()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                agents[i].agents[j].EndEpisode();
            }
        }
    }

    void AdjustTimeScale()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (this.timeScale + 0.2f <= 16)
            {
                this.timeScale = this.timeScale + 0.2f;
            }
            Time.timeScale = this.timeScale;
            Debug.Log($"Time scale was set to {this.timeScale}");

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (this.timeScale - 0.2f >= 0f)
            {
                this.timeScale = this.timeScale - 0.2f;
            }
            Time.timeScale = this.timeScale;
            Debug.Log($"Time scale was changed to {this.timeScale}");
        }
        else if (Input.GetKey(KeyCode.Home))
        {
            this.timeScale = this.defaultTimeScale;
            Time.timeScale = this.timeScale;
            Debug.Log($"Time scale was changed to {this.timeScale}");
        }
    }
}
