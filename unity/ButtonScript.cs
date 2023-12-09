using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject gameStartPannel;
    public Button[] buttons;
    public Button[] typeButton;
    public Button[] levelButton;

    public Color original = new Color(52f / 255f, 52f / 255f, 52f / 255f);
    public Color ClickColor = new Color(1f, 1f, 1f);
    public int level;

    public void Start()
    {
        Debug.Log(BackendGameData.Instance.UserGameData.t2Unlocked);
        foreach(Button button in typeButton)
        {
            Image image = button.GetComponent<Image>();
            if(image != null)
            {
                image.color = original;
            }
        }
        if (!BackendGameData.Instance.UserGameData.t2Unlocked)
        {
            typeButton[1].interactable = false; //클릭 이벤트 제거
        }
        if (!BackendGameData.Instance.UserGameData.t3Unlocked)
        {
            typeButton[2].interactable = false;
        }
    }
    public void typeClick()
    {
        string EventButtonName = EventSystem.current.currentSelectedGameObject.name;
        foreach (Button button in typeButton)
        {
            Image image = button.GetComponent<Image>();
            if (button.name == EventButtonName)
            {
                
                if( image != null)
                {
                    image.color = ClickColor;
                }
            }
            else
            {
                image.color = original;
            }
        }
        ChooseType.Instance.type = EventButtonName;
        Debug.Log(EventButtonName);
    }

    public void easyGameStart()
    {
        ChooseType.Instance.level = 0;
        SceneManager.LoadScene("MainGamePlay");
    }

    public void normalGameStart()
    {
        ChooseType.Instance.level = 1;
        SceneManager.LoadScene("MainGamePlay");
    }

    public void hardGameStart()
    {
        ChooseType.Instance.level = 2;
        SceneManager.LoadScene("MainGamePlay");
    }

    public void closeGamePannel()
    {
        gameStartPannel.SetActive(false);
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }
}
