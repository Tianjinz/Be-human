using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChoose : MonoBehaviour
{
    public void Tutorial() {
        SceneManager.LoadScene(2);
    }
    
    public void LevelOne() {
        SceneManager.LoadScene(3);
    }
}
