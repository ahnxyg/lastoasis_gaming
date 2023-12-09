using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider hp;
    public MobController parentMobController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        hp.value = parentMobController.health / parentMobController.maxHealth;
        

    }
}
