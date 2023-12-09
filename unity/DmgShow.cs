using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DmgShow : MonoBehaviour
{
    public static DmgShow instance;
    public TextMeshProUGUI[] dmgs;
    public Image[] imgs;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        foreach(TextMeshProUGUI dmg in dmgs)
        {
            dmg.text = 0.ToString();
        }
    }

    // Update is called once per frame
    public void UpdateText()
    {
        for(int i = 0; i < imgs.Length; i++)
        {
            try
            {
                string temp = imgs[i].GetComponent<Image>().sprite.name.ToString();
 
                int d = GameManager.instance.totalDmg[temp + "(Clone)"];
                
                dmgs[i].text = d.ToString();
            }
            catch
            {

                continue;
            }
        }
    }
}
