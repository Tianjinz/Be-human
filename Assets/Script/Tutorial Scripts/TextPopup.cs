using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextPopup : MonoBehaviour
{

    public GameObject popup;

    private bool isPop = false;

    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPop) {
            isPop = true;
            popup.SetActive(true);
            GameManager.instance.unlockCursor();
            GameManager.instance.Pause();
        }
            
            
        
    }

    

    /*void OnTriggerExit(Collider other)
    {
            popup.SetActive(false);
        
    }*/

    
}
