using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour {

    private float animationTime = 13.0f;

    public void Update() {

        animationTime -= Time.deltaTime;
        // change to start menu scene when animation ends
        if (animationTime <= 0) {
            SceneManager.LoadScene(1);
        }

    }
    
}
