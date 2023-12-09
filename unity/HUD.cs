using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public enum InfoType { Time, Resource, ResourceRed, BlueCount, RedCount }
    public InfoType type;

    Text myText;

    void Awake()
    {
        instance = this;
        myText = GetComponent<Text>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Time:
                float time = GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(time / 60);
                int sec = Mathf.FloorToInt(time % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Resource:
                myText.text = string.Format("{0:D0}", GameManager.instance.resource);
                break;
            case InfoType.ResourceRed:
                myText.text = string.Format("{0:D0}", GameManager.instance.redResource); 
                break;
            case InfoType.BlueCount:
                myText.text = string.Format("{0:D0}", GameManager.instance.blueCount);
                break;
            case InfoType.RedCount:
                myText.text = string.Format("{0:D0}", GameManager.instance.redCount);
                break;
        }
    }
}
