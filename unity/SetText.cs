using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    public List<MobController> T1 = new List<MobController>();
    public List<MobController> T2 = new List<MobController>();
    public List<MobController> T3 = new List<MobController>();
    public TextMeshProUGUI[] tmp2;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< tmp2.Length; i++)
        {
            if(ChooseType.Instance.type == "Type1")
            {
                tmp2[i].text = "Cost:" + T1[i].cost.ToString();
            }else if(ChooseType.Instance.type == "Type2")
            {
                tmp2[i].text = "Cost:" + T2[i].cost.ToString();
            }
            else
            {
                tmp2[i].text = "Cost:" + T3[i].cost.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
