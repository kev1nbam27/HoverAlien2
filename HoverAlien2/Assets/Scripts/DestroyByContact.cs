using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public Vector2 screenBounds;
    public GameObject gameController;
    public GameObject starCoinPrefab;
    public GameObject explosion;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
    }

    void Update()
    {
        if(transform.position.x < Camera.main.transform.position.x - screenBounds.x * 2){
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy(this);
        if (other.tag == "Laser")
        {
            GameObject e = Instantiate(explosion) as GameObject;
            ParticleSystem.ShapeModule es = e.GetComponent<ParticleSystem>().shape;
            es.texture = Resources.Load<Texture2D>(GameController.activeLevel.obstclSprite);
            e.transform.position = transform.position;
            GameObject s = Instantiate(starCoinPrefab) as GameObject;
            s.transform.position = new Vector3(transform.position.x, transform.position.y, -3F);
            s.GetComponent<CollectStarCoin>().screenBounds = screenBounds;
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        else if (other.tag == "Player")
        {
            gameController.GetComponent<GameController>().GameOver();
        }
    }
}
