using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float sensitivity = 250f;

    public Transform player;

    float xRotation = 0f;

    void Start() {

    }

    void Update() {
        // get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // vertical movement
        xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -15f, 15f);

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // horizontal movement
        player.Rotate(Vector3.up * mouseX);
        
    }
    
}
