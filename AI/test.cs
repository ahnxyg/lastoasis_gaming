using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class test : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Initialize()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (Input.GetKey(KeyCode.A))
        {
            EndEpisode();
            Regame.instance.ResetAll();
        }
    }
}
