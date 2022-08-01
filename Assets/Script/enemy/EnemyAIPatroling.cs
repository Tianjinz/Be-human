using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIPatroling : MonoBehaviour
{

    Animator anim; // Character Animator

    public float speed = 5 ;
    public float waitTime =  .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;

    public float damagePerSec;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    float viewAngle;
    float playerVisibleTimer;

    // enemy patroling acceptable distance to destination
    public float offset;

    public Transform pathHolder;
    Transform player;
    Color originalSpotlightColour;

    private bool patroling = true;
    public Vector3[] waypoints;
    


    // get the points on the path
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;

        waypoints = new Vector3[pathHolder.childCount];
        for(int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3 (waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        startPatroling();

        anim = GetComponent<Animator>(); //Get Character Animator

    }

    void Update() {

        if (CanSeePlayer()) {
            playerVisibleTimer += Time.deltaTime;
        } else {
            playerVisibleTimer -= Time.deltaTime;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer) {
            // if enemy sees player, damage the player
            float damage = damagePerSec * Time.deltaTime;
            PlayerHealth.instance.changeHealth(-damage);
        }

    }

    // check if the Ai can see the player
    bool CanSeePlayer() {
        if(Vector3.Distance(transform.position, player.position) < viewDistance) {
            // the angle between the Ai forward direction and the direction to the player
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenEnemyAndPlayer < viewAngle / 2f) {
                // if the line of sight to the player is blocked by obstacle
                if(!Physics.Linecast (transform.position, player.position, viewMask)) {
                    return true;
                }
            }
        }
        return false;
    }

    // make the enemy walk on setting path
    IEnumerator FollowPath(Vector3[] waypoints) {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (patroling)
        {   
            // move the Ai towards to the target point
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            // if the transform dot position is equal to target position
            if (Vector3.Distance(transform.position, targetWaypoint) < offset) {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds (waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
             
        }
    }


    // set the Ai rotate to face the next waypoint before moving off
    IEnumerator TurnToFace(Vector3 lookTarget) {
        // the Ai have on the y-axis to be facing the look target 
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
        // while the delta angle is greater than 0.05 keep rotating towards the target angle
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
            // make the Ai rotate towards this target angle at a time
            float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed *Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;

        }
    }

    // draw the pathline and patrol points
    void OnDrawGizmos(){
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        // set the Ai go back to the start point
        Gizmos.DrawLine(previousPosition, startPosition);
        // draw the view distance of Ai by red line
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    public void stopPatroling(){
        // print("stopped");
        patroling = false;
    }

    public void startPatroling(){
        patroling = true;
        StartCoroutine(FollowPath(waypoints));

    }

}