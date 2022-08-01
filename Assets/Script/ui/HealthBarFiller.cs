using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFiller : MonoBehaviour
{
    public Slider slider;

    public static HealthBarFiller instance;

    void Start()
    {
        print("hb init");
        HealthBarFiller.instance = this;
    }

    public void setHealth(float health){
        slider.value = health;
    }
}
