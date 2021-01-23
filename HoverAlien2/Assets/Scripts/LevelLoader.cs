using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject levelPrefab;
    public GameController gameController;

    public SceneFade sceneFade;

    //public class SkinsRoot{};
    // Start is called before the first frame update
    void OnEnable()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        GameController.LevelsRoot allLevels = GameController.allLevels;

        sceneFade = GameObject.FindWithTag("SceneFade").GetComponent<SceneFade>();

        if (transform.childCount > 0)
        {
            return;
        }

        else
        {
            foreach (GameController.Level l in allLevels.levels)
            {
                if (l.page == this.name)
                {
                    CreateLevelObject(l, transform);
                }
            }
        }
    }

    private void CreateLevelObject(GameController.Level level, Transform container)
    {
        Transform levelObject = Instantiate(levelPrefab.transform, container);
        
        Button button = levelObject.GetChild(0).GetComponent<Button>();
        button.name = level.id.ToString();
        button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = (level.id + 1).ToString();

        if (level.id < gameController.currentLevel)
        {
            button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0F / 255, 106F / 255, 121F / 255);
            button.transform.GetComponent<Image>().color = new Color(190F / 255, 251F / 255, 255F / 255);
        }

        else if (level.id == gameController.currentLevel)
        {
            button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0F / 255, 106F / 255, 121F / 255);
            button.transform.GetComponent<Image>().color = new Color(160F / 255, 219F / 255, 69F / 255);
        }

        else if (level.id > gameController.currentLevel)
        {
            button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = new Color(190F / 255, 251F / 255, 255F / 255, 128F / 255);
            button.transform.GetComponent<Image>().color = new Color(0F / 255, 106F / 255, 121F / 255);
            button.interactable = false;
        }

        button.onClick.AddListener(() => SelectLevel(button));
    }

    public void SelectLevel(Button button)
    {
        GameController.LevelsRoot allLevels = GameController.allLevels;
        
        GameController.activeLevelID = int.Parse(button.name);
        StartCoroutine(sceneFade.LoadScene("Play"));
    }
}
