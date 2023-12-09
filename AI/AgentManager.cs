using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgentManager : MonoBehaviour
{
    public float thresholdFPS; // FPS 임계값 설정

    private void Update()
    {
    }

    public int SubmitAction(int team, string type, int action)
    {
        return BlueRedAcademy.btn[team].summonFunctions[action](type);
    }


    /*public int DetermineTerminal()
    {
        int terminal = 0;
        bool Terminal = Building.Terminal;
        if (Terminal)
        {
            terminal = 1;
        }
        if (Input.GetKey(KeyCode.R))
        {
            terminal = 2;
        }
        if (FPS.fps > 0)
        {
            if (FPS.emaFPS < this.thresholdFPS && FPS.start)
            {
                Debug.Log("Low FPS is continuing. End Episode.");
                terminal = 1;
                FPS.start = false;
                FPS.emaFPS = 0;
            }
        }

        return terminal;
    }*/
}
