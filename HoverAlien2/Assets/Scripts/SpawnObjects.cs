using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnObjects : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public float obstacleRespawnTime = 0.5f;
    public float coinRespawnTime = 0.5f;
    public int obstacleCount;
    public float obstacleWaveWait;
    public int obstacleWaveCount = 10;
    int obstacleWaves;
    private Vector2 screenBounds;

    // Use this for initialization
    void Start() {
        obstacleWaveWait = obstacleWaveWait / 2;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(obstacleWave());
        StartCoroutine(coinWave());
    }

    void Update()
    {
        obstacleCount = GameController.activeLevel.difficulty;
    }

    private void spawnObstacle(){
        GameObject o = Instantiate(obstaclePrefab) as GameObject;
        o.transform.position = new Vector2(screenBounds.x * 2 + transform.position.x, Random.Range(-screenBounds.y + o.GetComponent<SpriteRenderer>().bounds.size.y / 2, screenBounds.y - o.GetComponent<SpriteRenderer>().bounds.size.y / 2));
        o.GetComponent<DestroyByContact>().screenBounds = screenBounds;
    }
    IEnumerator obstacleWave(){
        int j = 0;
        while(j<=obstacleWaveCount){
            for (int i = 0; i < obstacleCount; i++)
            {
                yield return new WaitForSeconds(obstacleRespawnTime);
                spawnObstacle();
            }
            yield return new WaitForSeconds (obstacleWaveWait);
            j = j + 1;
        }
        if (GameController.activeLevelID == transform.GetComponent<GameController>().currentLevel)
        {
            transform.GetComponent<GameController>().currentLevel = transform.GetComponent<GameController>().currentLevel + 1;
            transform.GetComponent<GameController>().SaveCurrentLevel();
        }
        GameController.menu = "Start";
        SceneManager.LoadScene("Menu");
    }

    private void spawnCoin(){
        GameObject c = Instantiate(coinPrefab) as GameObject;
        c.transform.position = new Vector2(screenBounds.x * 2 + transform.position.x, Random.Range(-screenBounds.y, screenBounds.y));
        c.GetComponent<CollectCoin>().screenBounds = screenBounds;
    }
    IEnumerator coinWave(){
        while(true){
            yield return new WaitForSeconds(coinRespawnTime);
            spawnCoin();
        }
    }
}
