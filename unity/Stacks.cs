using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stacks : MonoBehaviour
{
    public Sprite[] t1Sprites;
    public Sprite[] t2Sprites;
    public Sprite[] t3Sprites;
    public SpriteRenderer[] sprites;
    public TextMeshProUGUI[] textMeshPro;

    void Start()
    {
        if(ChooseType.Instance.type == "Type1")
        {
            for(int i = 0; i<sprites.Length; i++)
            {
                sprites[i].sprite = t1Sprites[i];
            }
        }else if(ChooseType.Instance.type == "Type2")
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].sprite = t2Sprites[i];
            }
        }
        else
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].sprite = t3Sprites[i];
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Dictionary<string, int> temp = PoolManager.instance.stack;

        for(int i = 0; i < sprites.Length; i++)
        {
            string currentSprite = sprites[i].sprite.name;
            
            if (temp.ContainsKey(currentSprite))
            {
                Debug.Log(currentSprite + " : " + temp[currentSprite]);
                textMeshPro[i].text = temp[currentSprite].ToString();
                Debug.Log(temp[currentSprite].ToString());  
            }
        }
    }
}
