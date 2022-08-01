using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{

    public float rotationAnglePerSec;

    public float floatSpeedPerSec;

    public float maxDisplacement;

    private bool isMovingUp = true;

    private float ogYValue;

    void Start()
    {
        ogYValue = transform.position.y;
    }

    // items rotate and move up and down
    void Update()
    {
        // item rotates
        Vector3 rotation = new Vector3(0, 1, 0);
        rotation = rotation * rotationAnglePerSec * Time.deltaTime;
        transform.Rotate(rotation, Space.World);

        // item moves up and down
        Vector3 movement = new Vector3(0f, floatSpeedPerSec * Time.deltaTime, 0f);

        if(isMovingUp){
            transform.position += movement;
            if(transform.position.y > (ogYValue + maxDisplacement)){
                isMovingUp = false;
            }
        }else{
            transform.position -= movement;
            if(transform.position.y < (ogYValue - maxDisplacement)){
                isMovingUp = true;
            }
        }

    }
}
