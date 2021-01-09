using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStarCoin : MonoBehaviour
{
    public Vector2 screenBounds;
    public GameObject gameController;
    public GameObject floatingScore;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
    }

    void Update()
    {
        //Debug.Log(screenBounds.x);
        if(transform.position.x < Camera.main.transform.position.x - screenBounds.x * 2){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy(this);
        if (other.tag == "Player")
        {
            Instantiate(floatingScore, new Vector3(transform.position.x, transform.position.y + 0.25F, transform.position.z), Quaternion.identity);
            gameController.GetComponent<GameController>().UpdateScore(100);
            Destroy(this.gameObject);
        }
    }   
}
