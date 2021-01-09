using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float speed = 500f;
    public static bool supportsAccelerometer;
    public Camera MainCamera;
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    public bool joystick;
    public Joystick joystickObject;

    void Start () {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    void Update()
    {
#if UNITY_EDITOR
    //get the Input from Horizontal axis
    float horizontalInput = Input.GetAxis("Horizontal");
    //get the Input from Vertical axis
    float verticalInput = Input.GetAxis("Vertical");

    //update the position
    transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
#endif
        if (joystick == true)
        {
            Vector3 dir = Vector3.zero;

            dir.x = joystickObject.Horizontal;
            dir.y = joystickObject.Vertical;

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;

            // Move object
            transform.Translate(dir * 17.5f);
        }

        else
        { 
            Vector3 dir = Vector3.zero;

            dir.x = Input.acceleration.x;
            dir.y = -Input.acceleration.z -0.75f;

            // clamp acceleration vector to unit sphere
            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            // Make it move 10 meters per second instead of 10 meters per frame...
            dir *= Time.deltaTime;

            // Move object
            transform.Translate(dir * 50f);
        }
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        Vector3 cam = MainCamera.transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth + cam.x, screenBounds.x - objectWidth + cam.x);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }
    
}
