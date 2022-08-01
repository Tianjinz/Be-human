using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.toNextLevel.AddListener(nextLevel);
        GameManager.instance.gameLost.AddListener(reloadLvl);
    }
    private void nextLevel(string sceneName){
        print(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private void reloadLvl(){
        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
