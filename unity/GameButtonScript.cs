using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtonScript : MonoBehaviour
{
    public GameObject[] pannels;
    public GameObject[] buttons;
    void Start()
    {
        for (int i = 0; i < pannels.Length; i++)
        {
            pannels[i].gameObject.SetActive(false);
        }
    }

    public void gameStartButton()
    {
        foreach (GameObject button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        pannels[0].gameObject.SetActive(true);
    }

    public void shopOpen()
    {
        foreach (GameObject button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        pannels[1].gameObject.SetActive(true);
    }

    public void menuOpen()
    {
        foreach (GameObject button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        pannels[2].gameObject.SetActive(true);
    }

    public void rankOpen()
    {
        foreach(GameObject button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        pannels[3].gameObject.SetActive(true);
    }
}