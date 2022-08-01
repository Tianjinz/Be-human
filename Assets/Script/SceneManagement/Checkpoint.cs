using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    // List with all checkpoint objects in the scene
    public static GameObject[] CheckPointsList;

    public bool activated = false;

    void Start()
    {
        // We search all the checkpoints in the current scene
        CheckPointsList = GameObject.FindGameObjectsWithTag("Checkpoint");

    }
    
    void Awake()
    {
        activated = false;
    }

    // Activate the checkpoint
    private void ActivateCheckPoint()
    {
        // We deactive all checkpoints in the scene
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<Checkpoint>().activated = false;
            //cp.GetComponent<Checkpoint>().SetBool("Active", false);
        }

        // We activate the current checkpoint
        activated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player passes through the checkpoint, we activate it
        if (other.tag == "Player")
        {
            ActivateCheckPoint();
        }
    }

        // Get position of the last activated checkpoint
    public static Vector3 GetActiveCheckPointPosition()
    {
        // If player die without activate any checkpoint, we will return the original position of player
        Vector3 result = PlayerMovement.instance.playerOGPos;

        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                // We search the activated checkpoint to get its position
                if (cp.GetComponent<Checkpoint>().activated)
                {
                    result = cp.transform.position;
                    break;
                }
            }
        }

        return result;
    }
}

