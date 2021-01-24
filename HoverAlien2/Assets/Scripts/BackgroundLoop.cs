using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour{
    public GameObject[] levels;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;
    public float scrollSpeed;
    public Sprite backgroundSprite;

    void Start(){
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        scrollSpeed = GameController.activeLevel.speed;

        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach(GameObject obj in levels){
            loadChildObjects(obj);
        }
    }
    void loadChildObjects(GameObject obj){
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);
        GameObject clone = Instantiate(obj) as GameObject;
        for(int i = 0; i <= childsNeeded; i++){
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            //int max = obj.GetComponent<SpriteRenderer>().bounds.size.y * 10
            //Debug.Log((float)obj.GetComponent<SpriteRenderer>().bounds.size.y);
            if (obj.tag == "Background")
            {
                c.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
                c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            }

            //else if (obj.tag == "Coin")
            //{
            //    c.GetComponent<CircleCollider2D>().enabled = true;
            //    int randomNumberMax = (int)(100 * ((float)screenBounds.y / (float)obj.GetComponent<SpriteRenderer>().bounds.size.y));
            //    c.transform.position = new Vector3(objectWidth * i, Random.Range(-1 * randomNumberMax, randomNumberMax) * (float)obj.GetComponent<SpriteRenderer>().bounds.size.y, obj.transform.position.z);
            //}

            //else 
            //{
                //c.GetComponent<BoxCollider2D>().enabled = true;
            //    int randomNumberMax = (int)(100 * ((float)screenBounds.y / (float)obj.GetComponent<SpriteRenderer>().bounds.size.y));
            //    c.transform.position = new Vector3(objectWidth * i, Random.Range(-1 * randomNumberMax, randomNumberMax) * (float)obj.GetComponent<SpriteRenderer>().bounds.size.y, obj.transform.position.z);
            //}
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }
    void repositionChildObjects(GameObject obj){
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if(children.Length > 1){
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if(transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth){
                firstChild.transform.SetAsLastSibling();
                if (obj.tag == "Background"){
                    firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
                }

                else{
                    firstChild.SetActive(true);
                    int randomNumberMax = (int)(((float)screenBounds.y / (float)firstChild.GetComponent<SpriteRenderer>().bounds.size.y));
                    firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 4, Random.Range(-1 * randomNumberMax, randomNumberMax) * (float)firstChild.GetComponent<SpriteRenderer>().bounds.size.y, lastChild.transform.position.z);
                }
                //float randomNumberMax = ((float)screenBounds.y / (float)firstChild.GetComponent<SpriteRenderer>().bounds.size.y);
                //firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, Random.Range(-1 * randomNumberMax, randomNumberMax) * (float)firstChild.GetComponent<SpriteRenderer>().bounds.size.y, lastChild.transform.position.z);
            }else if(transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjectWidth){
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
    void FixedUpdate() {

        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = transform.position + new Vector3(2 * scrollSpeed * scrollSpeed * Time.fixedDeltaTime, 0, 0);
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;
       // Debug.Log(desiredPosition.x);
       // Debug.Log(smoothPosition.x);

    }
    void LateUpdate(){
        foreach(GameObject obj in levels){
            repositionChildObjects(obj);
        }
    }
}
