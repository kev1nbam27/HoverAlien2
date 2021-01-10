using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    static bool end;

    void Start()
    {
        if (end)
        {
            transition.SetTrigger("End");
        }
        end = false;
    }

    public IEnumerator LoadScene(string scene)
    {
        //transition.SetTrigger("End");
        transition.SetTrigger("Start");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(transitionTime);
        end = true;
        SceneManager.LoadScene(scene);
    }
}
