using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{

    public static InGameUIManager instance;

    public GameObject deathScreen;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // emptyItemUI();
        deathScreen.SetActive(false);
    }

    // display item on ui
    public void displayItem(Item item){
        ItemUIScript.instance.displayItem(item);
    }

    public void emptyItemUI(){
        ItemUIScript.instance.emptyItemUI();
    }

    public void playerDeath(){
        GameManager.instance.Pause();
        deathScreen.SetActive(true);
    }

    public void setHealth(float health){
        HealthBarFiller.instance.setHealth(health);
    }

}
