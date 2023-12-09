using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightHp : MonoBehaviour
{
    public Slider hp;
    public KnightController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hp.value = controller.health / controller.maxHealth;
    }
}
