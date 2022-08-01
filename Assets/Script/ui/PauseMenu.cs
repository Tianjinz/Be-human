using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       if(GameManager.instance.inGame && Input.GetKeyDown(KeyCode.Escape))
       {
           if(GameManager.instance.GamePaused)
           {
               GameManager.instance.Resume();
               pauseMenuUI.SetActive(false);
               GameManager.instance.lockCursor();
           }
           else
           {
               GameManager.instance.unlockCursor();
               GameManager.instance.Pause();
               pauseMenuUI.SetActive(true);
           }
       }
    }
    

    public void Resume()
    {
       GameManager.instance.Resume(); 
       pauseMenuUI.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.instance.Resume();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(1);
    }


}
