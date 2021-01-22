using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootLaser : MonoBehaviour
{
    public GameObject laser;
    public float laserSpeed;
    public int maxLasers;
    public float fireRate;
    private float nextFire;
    public Sprite laserSprite;
    public RectTransform touchArea;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Laser").Length < maxLasers && Time.time > nextFire)
        {   
            if (!this.GetComponent<PlayerMover>().joystick)
            {
                if (Input.GetButton("Fire1"))
                {
                    nextFire = Time.time + fireRate;
                        GameObject laserClone = Instantiate(laser, transform.position + new Vector3(0.75F, 0, 0), Quaternion.Euler(0, 0, -90));
                        laserClone.transform.SetParent(transform.parent);
                        laserClone.SetActive(true);
                        laserClone.GetComponent<SpriteRenderer>().sprite = laserSprite;
                        return;
                }
            }

            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if(RectTransformUtility.RectangleContainsScreenPoint(touchArea,touch.position))
                    {
                        nextFire = Time.time + fireRate;
                        GameObject laserClone = Instantiate(laser, transform.position + new Vector3(0.75F, 0, 0), Quaternion.Euler(0, 0, -90));
                        laserClone.transform.SetParent(transform.parent);
                        laserClone.SetActive(true);
                        laserClone.GetComponent<SpriteRenderer>().sprite = laserSprite;
                    }
                }

            }

            else if (Input.GetKey("left ctrl") || Input.GetKey("right ctrl") || Input.GetButton("Fire1_joystick"))
            {
                nextFire = Time.time + fireRate;
                GameObject laserClone = Instantiate(laser, transform.position + new Vector3(0.75F, 0, 0), Quaternion.Euler(0, 0, -90));
                laserClone.transform.SetParent(transform.parent);
                laserClone.SetActive(true);
                laserClone.GetComponent<SpriteRenderer>().sprite = laserSprite;
            }

            
        }

        
    }
}
