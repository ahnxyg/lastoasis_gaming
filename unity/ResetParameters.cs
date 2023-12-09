using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetParameters : MonoBehaviour
{
    
    public void resetParams()
    {
        GameManager.instance.gameTime = 0;
        GameManager.instance.resource = GameManager.instance.startResource;
        GameManager.instance.resourceIncrease = 5;
        GameManager.instance.redResource = GameManager.instance.startResource; 
    }
}
