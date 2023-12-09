using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DmgBar : MonoBehaviour
{
    public Slider[] hps;
    public TextMeshProUGUI[] texts;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        int[] values = new int[9];
        int sum = 0;
        for(int i = 0; i<9; i++)
        {
            values[i] = int.Parse(texts[i].text);
            sum += values[i];
            
        }
        if (sum != 0)
        {
            for (int i = 0; i < hps.Length; i++)
            {
                if (values[i] > 0)
                {
                    
                    hps[i].value = (float)values[i] / sum;
                    Debug.Log(hps[i].value);

                }
                else
                {
                    hps[i].value = 0;
                }

            }
        }

    }
}
