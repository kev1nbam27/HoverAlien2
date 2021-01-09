using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public Vector2 screenBounds;
    public GameObject gameController;
    public GameObject floatingScore;
    public int value;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        int r = Random.Range(0, 100);
        if (r < 50)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item_CoinBronze");
            value = 10;
        }

        else if (50 < r && r < 83)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item_CoinSilver");
            value = 20;
        }

        else if (83 < r && r < 100)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item_CoinGold");
            value = 50;
        }

        else
        {
            if (GameController.score < 5000)
            {
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item_GemRed");
                value = 750;
            }

            else if (GameController.score < 10000)
            {
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item_GemGreenDark");
                value = 1500;
            }

            else
            {
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Item_GemVioletDark");
                value = 3000;
            }
        }
        //Debug.Log(Resources.Load<Sprite>("ItemGem_Red"));



        
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
            GameObject t = Instantiate(floatingScore, transform.position, Quaternion.identity);
            t.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "+" + value;
            gameController.GetComponent<GameController>().UpdateScore(value);
            Destroy(this.gameObject);
        }
    }   
}
