using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollison : MonoBehaviour
{
    public float minDist = 0.1f;
    public float maxDist = 4.0f;
    public float speed = 10.0f;

    private float distance;
    Vector3 direction;

    void Awake() {
        direction = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Avoid collision
    void Update() {
        if (!Input.GetKey(KeyCode.W)) {
            Vector3 desiredCamPos = transform.TransformPoint(direction * maxDist);
            RaycastHit hit;

            if (Physics.Linecast(transform.parent.position, desiredCamPos, out hit)) {
                distance = Mathf.Clamp(hit.distance * 0.7f, minDist, maxDist);
            }
            else {
                distance = maxDist;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, direction * distance, Time.deltaTime * speed);
                
        }
        
    }
}
