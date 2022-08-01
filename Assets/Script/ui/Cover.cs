using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cover : MonoBehaviour
{
    public void ToStartMenu() {
        SceneManager.LoadScene(1);
    }
}
