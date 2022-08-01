using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController playerController;

    public static PlayerMovement instance;


    // move speed
    public float speed = 20f;

    private float gravity = -0.1f;

    Vector3 velocity;

    public Vector3 playerOGPos;


    private void Start()
    {
        PlayerMovement.instance = this;

        playerOGPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // moving direction based on the camera direction
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        playerController.Move(direction * speed * Time.deltaTime);
        // transform.position = transform.position + new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);

        // apply gravity
        // distance = 1/2 * g * t * t
        velocity.y += gravity * Time.deltaTime;
        // drop smoothly
        playerController.Move(velocity * Time.deltaTime);
    }



    public void Death()
    {
        print("mvment death");
        //Get the position of the active checkpoint and spawn player there
        Vector3 spawnPos = Checkpoint.GetActiveCheckPointPosition();
        print(spawnPos);
        playerController.enabled = false;

        // controller.Move(spawnPos - transform.position);
        transform.position = spawnPos;
        playerController.enabled = true;

    }
   
}
