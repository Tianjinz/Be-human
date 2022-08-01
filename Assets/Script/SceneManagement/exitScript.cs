using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script for the exit 

public class exitScript : MonoBehaviour
{
    
    public string loadLevel;
    
    public static GameObject[] KeyList;

    public static int ogKeyAmount;
    public static int keyAmount;

    public GameObject endNotif;

    void Start()
    {
        // tag the exit to make it easier for collision detection
        this.gameObject.tag = "exit";
        endNotif.SetActive(false);

        // Get all the items tagged "key" in the scene
        KeyList = GameObject.FindGameObjectsWithTag("Key");

        exitScript.keyAmount = KeyList.Length;
        exitScript.ogKeyAmount = KeyList.Length;
        // print(ogKeyAmount);
        KeyUIScript.instance.changeQuantity(ogKeyAmount - keyAmount);
    }

    void Update()
    {
        int keyAmountBefore = keyAmount;
        KeyList = GameObject.FindGameObjectsWithTag("Key");
        exitScript.keyAmount = KeyList.Length;
        if(keyAmount != keyAmountBefore){
            print(keyAmount);
            print(keyAmountBefore);
            KeyUIScript.instance.changeQuantity(ogKeyAmount - keyAmount);
        }
        // Debug.Log(keyAmount);          
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && keyAmount == 0)
        {
            GameManager.instance.unlockCursor();
            SceneManager.LoadScene(loadLevel);
        }
        else
        {
            endNotif.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
       
        endNotif.SetActive(false);
        
    }
}
