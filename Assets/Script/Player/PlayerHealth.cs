using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth instance;
    public float maxHealth;

    [SerializeField]
    private float currentHealth;

    void Start()
    {
        PlayerHealth.instance = this;
        // set the starting health
        // changeHealth(maxHealth);
    }

    public void changeHealth(float change){
        currentHealth += change;
        if(currentHealth <= 0){
            playerDeath();
        }
        else if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }

        InGameUIManager.instance.setHealth(currentHealth);

    }

    private void playerDeath(){
        PlayerMovement.instance.Death();
        currentHealth = maxHealth;
        InGameUIManager.instance.playerDeath();
        GameManager.instance.setInGameState(false);        
    }

    public float getCurrentHealth(){
        return currentHealth;
    }

}
