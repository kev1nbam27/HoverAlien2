using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public GameObject gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            gameController.GetComponent<GameController>().LevelCompleted();
        }
    }
}