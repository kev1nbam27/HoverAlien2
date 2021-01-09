using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float laserSpeed;
    
    void Start()
    {
        laserSpeed = laserSpeed * 1.5F;
    }
    
    void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = transform.position + new Vector3(laserSpeed * Time.fixedDeltaTime * laserSpeed * 2, 0, 0);
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;
    }

    void OnBecameInvisible()
    {
        if (gameObject.name != "Laser")
        {
            Destroy(gameObject);
        }
    }
}
