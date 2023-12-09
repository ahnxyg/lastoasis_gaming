using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class SetButton : MonoBehaviour
{
    public Sprite[] t1Sprites;
    public Sprite[] t2Sprites;
    public Sprite[] t3Sprites;

    [Header ("# Buttons")]
    public Button[] BlueButtons2;
    public Button[] RedButtons2;

    [Header("# pattern")]
    public GameObject patternManager;

    void Start()
    {
        if (BlueButtons2 != null)
        {
            if (ChooseType.Instance.type == "Type1")
            {
                for (int i = 0; i < BlueButtons2.Length; i++)
                {
                    BlueButtons2[i].GetComponent<Image>().sprite = t1Sprites[i];
                }
            }
            else if (ChooseType.Instance.type == "Type2")
            {
                for (int i = 0; i < BlueButtons2.Length; i++)
                {
                    BlueButtons2[i].GetComponent<Image>().sprite = t2Sprites[i];
                }
            }
            else
            {
                for (int i = 0; i < BlueButtons2.Length; i++)
                {
                    BlueButtons2[i].GetComponent<Image>().sprite = t3Sprites[i];
                }
            }
        }

        if (RedButtons2 != null)
        {
            if (ChooseType.Instance.enemyType == "Type1")
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
            }
        }
        if (patternManager != null)
        {
            patternManager.SetActive(true);
        }
    }
}
