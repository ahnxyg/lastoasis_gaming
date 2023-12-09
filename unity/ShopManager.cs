using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public Button[] buttons;

    public void Start()
    {
        if(BackendGameData.Instance.UserGameData.t2Unlocked)
        {
            buttons[0].interactable = false;
            buttons[1].interactable = false;
        }
        if(BackendGameData.Instance.UserGameData.t3Unlocked)
        {
            buttons[2].interactable = false;
            buttons[3].interactable = false;
        }
    }


    public void buyType2()
    {
        BackendGameData.Instance.UserGameData.t2Unlocked = true;
        BackendGameData.Instance.GameDataUpdate();
        buttons[0].interactable = false;
        buttons[1].interactable = false;
    }

    public void  buyType3()
    {
        BackendGameData.Instance.UserGameData.t3Unlocked = true;
        BackendGameData.Instance.GameDataUpdate();
        buttons[2].interactable = false;
        buttons[3].interactable = false;
    }
}
