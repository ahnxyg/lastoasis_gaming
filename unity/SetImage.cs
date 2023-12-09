using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImage : MonoBehaviour
{
    public Sprite[] t1Sprites;
    public Sprite[] t2Sprites;
    public Sprite[] t3Sprites;

    public Image[] BlueButtons2;
    //public Image[] RedButtons2;

    void Start()
    {

        if (ChooseType.Instance.type == "Type1")
        {
            for (int i = 0; i < BlueButtons2.Length; i++)
            {
                BlueButtons2[i].GetComponent<Image>().sprite = t1Sprites[i];
                try
                {
                    GameManager.instance.totalDmg.Add(t1Sprites[i].name + "(Clone)", 0);
                }
                catch { }
            }
        }
        else if (ChooseType.Instance.type == "Type2")
        {
            for (int i = 0; i < BlueButtons2.Length; i++)
            {
                BlueButtons2[i].GetComponent<Image>().sprite = t2Sprites[i];
                try
                {
                    GameManager.instance.totalDmg.Add(t2Sprites[i].name + "(Clone)", 0);
                }
                catch { }
            }
        }
        else
        {
            for (int i = 0; i < BlueButtons2.Length; i++)
            {
                BlueButtons2[i].GetComponent<Image>().sprite = t3Sprites[i];
                try
                {
                    GameManager.instance.totalDmg.Add(t3Sprites[i].name + "(Clone)", 0);
                }
                catch { }
            }
        }


        /*if (ChooseType.Instance.enemyType == "Type1")
        {
            for (int i = 0; i < RedButtons2.Length; i++)
            {
                RedButtons2[i].GetComponent<Image>().sprite = t1Sprites[i];
            }
        }
        else if (ChooseType.Instance.enemyType == "Type2")
        {
            for (int i = 0; i < RedButtons2.Length; i++)
            {
                RedButtons2[i].GetComponent<Image>().sprite = t2Sprites[i];
            }
        }
        else
        {
            for (int i = 0; i < RedButtons2.Length; i++)
            {
                RedButtons2[i].GetComponent<Image>().sprite = t3Sprites[i];
            }
        }*/
    }
}
