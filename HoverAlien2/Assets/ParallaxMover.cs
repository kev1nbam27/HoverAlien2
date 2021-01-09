using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
    public float scrollSpeed;
    
    void FixedUpdate() {

        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = transform.position + new Vector3(2 * scrollSpeed * scrollSpeed * Time.fixedDeltaTime, 0, 0);
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;

    }
}
