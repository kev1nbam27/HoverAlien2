using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnObjects : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    public GameObject finish;

    public float obstacleRespawnTime = 0.5f;
    public float coinRespawnTime = 0.5f;
    public int obstacleCount;
    public float obstacleWaveWait;
    public int obstacleWaveCount = 10;
    int obstacleWaves;
    public Vector2 screenBounds;
    
    float scale;

    // Use this for initialization
    void Start() {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        scale = GameController.verticalSize;

        obstacleCount = (int)Mathf.Round(scale * GameController.activeLevel.difficulty);
        obstacleWaveCount = GameController.activeLevel.waveCount;
        obstacleWaveWait = GameController.activeLevel.waveWait;
        obstacleRespawnTime = GameController.activeLevel.obstacleRespawnTime;
        coinRespawnTime = GameController.activeLevel.coinRespawnTime;

        #if UNITY_EDITOR
        obstacleWaveCount = 2;
        #endif

        float x = obstacleWaveCount * (obstacleRespawnTime * obstacleCount + obstacleWaveWait);
        float s = GameController.activeLevel.speed * GameController.activeLevel.speed * 2 * 0.032f / 4;
        float e = 2 * screenBounds.x + x * s;

        //GameObject f = Instantiate(finish) as GameObject;
        //f.transform.position = new Vector2(x, 0);

        StartCoroutine(obstacleWave());
        StartCoroutine(coinWave());
    }

    void Update()
    {
        if (GameController.activeLevel.id == 30)
            obstacleCount = (int)Mathf.Round(scale * (5f + (float)GameController.score / 1000));
    }

    private void spawnObstacle(){
        GameObject o = Instantiate(obstaclePrefab) as GameObject;
        o.transform.position = new Vector2(screenBounds.x * 2 + transform.position.x, Random.Range(-screenBounds.y + o.GetComponent<SpriteRenderer>().bounds.size.y / 2, screenBounds.y - o.GetComponent<SpriteRenderer>().bounds.size.y / 2));
        o.GetComponent<DestroyByContact>().screenBounds = screenBounds;
        o.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameController.activeLevel.obstclSprite);
    }

    IEnumerator obstacleWave(){
        if (GameController.activeLevel.id != 30)
        {
            int j = 0;
            while(j < obstacleWaveCount){
                for (int i = 0; i < obstacleCount; i++)
                {
                    yield return new WaitForSeconds(obstacleRespawnTime);
                    spawnObstacle();
                }
                yield return new WaitForSeconds (obstacleWaveWait);
                j = j + 1;
            }
            GameObject f = Instantiate(finish) as GameObject;
            f.transform.position = new Vector2(screenBounds.x * 2 + transform.position.x, 0);
        }

        else
        {
            while(true){
                for (int i = 0; i < obstacleCount; i++)
                {
                    yield return new WaitForSeconds(obstacleRespawnTime);
                    spawnObstacle();
                }
                yield return new WaitForSeconds (obstacleWaveWait);
            }
        }

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
