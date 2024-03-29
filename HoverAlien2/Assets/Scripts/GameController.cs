﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.Advertisements;
using Unity.RemoteConfig;



public class GameController : MonoBehaviour
{
    public static int score;
    public static string menu;
    public static string lastMenu;
    public SkinsRoot allSkins;
    public Dictionary<string, string> options;
    public int highscore;
    public int money;
    public GameObject joystickObject;

    public GameObject titleText;
    public GameObject scoreText;
    public GameObject modeText;
    public GameObject playButtonText;
    public GameObject moneyText;

    public GameObject player;
    public GameObject background1;
    public GameObject background2;
    public GameObject background3;
    public Sprite playerSprite;
    public Sprite backgroundSprite;
    public Sprite laserSprite;

    public GameObject controlButtonText;
    public GameObject modeButtonText;
    public GameObject tiltingOptionsButton;
    public GameObject joystickOptionsButton;

    public GameObject framerateButtonText;
    public int framerate;

    public GameObject errorMessage;
    
    public static LevelsRoot allLevels;
    public int currentLevel;
    public static int activeLevelID = -27;
    public static Level activeLevel;
    public GameObject levelText;

    public GameObject progressText;

    public SceneFade sceneFade;

    Vector3 screenBounds;

    public struct userAttributes {};
    public struct appAttributes {};

    public GameObject bonusTab;

    static bool lastLevelCompleted = false;
    public GameObject conMessage;

    public static float verticalSize = 1.5f;
    CanvasScaler canvasScaler;
    
    void Awake()
    {   
        if (SceneManager.GetActiveScene().name == "Start")
        {
            ConfigManager.FetchCompleted += LoadLevels;
            ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
        }

        else
        {
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Play" && activeLevel.id != 30)
        {
            #if UNITY_EDITOR
            int waveCount = 2;
            #else
            int waveCount = activeLevel.waveCount;
            #endif

            float x = waveCount * (activeLevel.obstacleRespawnTime * activeLevel.difficulty * verticalSize + activeLevel.waveWait);
            float s = activeLevel.speed * activeLevel.speed * 2 * 0.032f / 4;
            float e = 2 * screenBounds.x + x * s;
            
            int progress = (int)(this.transform.position.x / e * 100);

            if (progress > 100)
            {
                progress = 100;
            }

            progressText.GetComponent<TMPro.TextMeshProUGUI>().text = progress.ToString() + "%";
        }
    }

    void OnDestroy()
    {
        ConfigManager.FetchCompleted -= LoadLevels;
    }

    void OnEnable ()
    {
        Time.timeScale = 1;
        LoadCurrentLevel();

        LoadUserOptions();

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            highscore = PlayerPrefs.GetInt("highscore", 0);
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + (activeLevelID + 1);

            if (menu == "GameOver")
            {
                playButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Restart";
                titleText.GetComponent<TMPro.TextMeshProUGUI>().text = "Game Over";

                if (activeLevel.id == 30)
                {
                    scoreText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 87.6f);
                    scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + highscore + "\n" + "Your Score: " + score;

                    modeText.SetActive(true);
                }
            }

            else if (menu == "Start")
            {
                playButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Play";
                titleText.GetComponent<TMPro.TextMeshProUGUI>().text = "Hover Alien";

                if (activeLevel.id == 30)
                {
                    scoreText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 87.6f);
                    scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + highscore;

                    modeText.SetActive(true);
                }
            }

            else if (menu == "LevelCompleted")
            {
                playButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Continue";
                titleText.GetComponent<TMPro.TextMeshProUGUI>().text = "Level Completed";
                
                if (lastLevelCompleted == true)
                {
                    conMessage.SetActive(true);
                    lastLevelCompleted = false;
                }

                if (activeLevel.id == 30)
                {
                    scoreText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 87.6f);
                    scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + highscore;

                    modeText.SetActive(true);
                }
            }
            
            background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
        }

        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            money = PlayerPrefs.GetInt("money", 0);
            moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = money.ToString();
            LoadSkins();

            background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
        }

        else if (SceneManager.GetActiveScene().name == "MySkins")
        {
            money = PlayerPrefs.GetInt("money", 0);
            moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = money.ToString();
            LoadSkins();

            background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
        }

        else if (SceneManager.GetActiveScene().name == "Levels")
        {
            background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);

            if (currentLevel == 30)
                bonusTab.SetActive(true);
        }

        else if (SceneManager.GetActiveScene().name == "Options" || SceneManager.GetActiveScene().name == "HowToPlay")
        {
            LoadSkins();
            //LoadUserOptions();
            //LoadAdsOptions();

            background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
            background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(allLevels.levels[currentLevel].bgSprite);
        }

        else if (SceneManager.GetActiveScene().name == "Play")
        {
            highscore = PlayerPrefs.GetInt("highscore", 0);
            money = PlayerPrefs.GetInt("money", 0);

            score = 0;

            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score.ToString();
            levelText.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + (activeLevelID + 1).ToString();
            if (activeLevel.id == 30)
            {
                levelText.GetComponent<TMPro.TextMeshProUGUI>().text = "Endless Mode";
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                progressText.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
            }

            LoadSkins();
            //LoadUserOptions();
            //LoadAdsOptions();

            foreach (Skin s in allSkins.skins)
            {
                if (s.selected == true)
                {
                    playerSprite = Resources.Load<Sprite>(s.image);
                    player.GetComponent<SpriteRenderer>().sprite = playerSprite;
                }
            }

            Camera.main.GetComponent<BackgroundLoop>().backgroundSprite = Resources.Load<Sprite>(allLevels.levels[activeLevelID].bgSprite);

            foreach (Laser l in allSkins.lasers)
            {
                if (l.selected == true)
                {
                    player.GetComponent<ShootLaser>().laserSprite = Resources.Load<Sprite>(l.image);
                }
            }
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Start")
        {
            canvasScaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(800f, verticalSize * canvasScaler.referenceResolution.y);
        }

        if (SceneManager.GetActiveScene().name == "Play")
        {
            Camera.main.orthographicSize *= verticalSize;
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            player.GetComponent<PlayerMover>().screenBounds = screenBounds;
            Camera.main.GetComponent<BackgroundLoop>().screenBounds = screenBounds;
            this.GetComponent<SpawnObjects>().screenBounds = screenBounds;
        }
    }

    void LoadLevels(ConfigResponse response)
    {
        allLevels = JsonConvert.DeserializeObject<LevelsRoot>(ConfigManager.appConfig.GetJson("Levels"));
        activeLevel = allLevels.levels[activeLevelID];


        if (SceneManager.GetActiveScene().name == "Start")
        {
            menu = "Start";
            lastMenu = "Start";
            Input.GetJoystickNames();

            if (PlayerPrefs.GetString("HowToPlay", "Felix") != "Felix")
            {
                SceneManager.LoadScene("Menu");
            }
            
            else
            {
                PlayerPrefs.SetString("HowToPlay", "false");
                SceneManager.LoadScene("HowToPlay");
            }
        }
    }

    public void LoadLevels1()
    {
        if (PlayerPrefs.GetString("levels", "Felix") != "Felix")
        {
            allLevels = JsonConvert.DeserializeObject<LevelsRoot>(PlayerPrefs.GetString("levels"));
            Debug.Log(PlayerPrefs.GetString("levels"));
        }

        else
        {
            allLevels = new LevelsRoot();
            allLevels.levels = new List<Level>() {

                //PAGE_0 PAGE_0 PAGE_0 PAGE_0 PAGE_0
                //PAGE_0 PAGE_0 PAGE_0 PAGE_0 PAGE_0
                //PAGE_0 PAGE_0 PAGE_0 PAGE_0 PAGE_0
                //PAGE_0 PAGE_0 PAGE_0 PAGE_0 PAGE_0
                //PAGE_0 PAGE_0 PAGE_0 PAGE_0 PAGE_0
                new Level{
                        id= 0,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 5
                    },

                new Level{
                        id= 1,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 6
                    },

                new Level{
                        id= 2,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 7
                    },

                new Level{
                        id= 3,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 8
                    },

                new Level{
                        id= 4,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 9
                    },

                new Level{
                        id= 5,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 10
                    },
                
                new Level{
                        id= 6,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 11
                    },

                new Level{
                        id= 7,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 12
                    },

                new Level{
                        id= 8,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 13
                    },

                new Level{
                        id= 9,
                        page= "Page_0",
                        bgSprite= "BG_Colored_grass",
                        obstclSprite= "Ground_GrassHalf",
                        difficulty= 14
                    },


                

                //PAGE_1 PAGE_1 PAGE_1 PAGE_1 PAGE_1
                //PAGE_1 PAGE_1 PAGE_1 PAGE_1 PAGE_1
                //PAGE_1 PAGE_1 PAGE_1 PAGE_1 PAGE_1
                //PAGE_1 PAGE_1 PAGE_1 PAGE_1 PAGE_1
                //PAGE_1 PAGE_1 PAGE_1 PAGE_1 PAGE_1
                new Level{
                        id= 10,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 15
                    },

                new Level{
                        id= 11,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 16
                    },

                new Level{
                        id= 12,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 17
                    },

                new Level{
                        id= 13,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 18
                    },

                new Level{
                        id= 14,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 19
                    },

                new Level{
                        id= 15,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 20
                    },
                
                new Level{
                        id= 16,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 21
                    },

                new Level{
                        id= 17,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 22
                    },

                new Level{
                        id= 18,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 23
                    },

                new Level{
                        id= 19,
                        page= "Page_1",
                        bgSprite= "BG_Colored_desert",
                        obstclSprite= "Ground_SandHalf",
                        difficulty= 24
                    },

            


                //PAGE_2 PAGE_2 PAGE_2 PAGE_2 PAGE_2
                //PAGE_2 PAGE_2 PAGE_2 PAGE_2 PAGE_2
                //PAGE_2 PAGE_2 PAGE_2 PAGE_2 PAGE_2
                //PAGE_2 PAGE_2 PAGE_2 PAGE_2 PAGE_2
                //PAGE_2 PAGE_2 PAGE_2 PAGE_2 PAGE_2
                new Level{
                        id= 20,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 25
                    },

                new Level{
                        id= 21,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 26
                    },

                new Level{
                        id= 22,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 27
                    },

                new Level{
                        id= 23,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 28
                    },

                new Level{
                        id= 24,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 29
                    },

                new Level{
                        id= 25,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 30
                    },
                
                new Level{
                        id= 26,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 31
                    },

                new Level{
                        id= 27,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 32
                    },

                new Level{
                        id= 28,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 33
                    },

                new Level{
                        id= 29,
                        page= "Page_2",
                        bgSprite= "BG_Colored_shroom",
                        obstclSprite= "Ground_DirtHalf",
                        difficulty= 34
                    },
            };

            SaveLevels1();
        }
        activeLevel = allLevels.levels[activeLevelID];
    }

    public void SaveLevels1()
    {
        string json = JsonConvert.SerializeObject(allLevels);
        PlayerPrefs.SetString("levels", json);
        PlayerPrefs.Save();
    }

    public void LoadCurrentLevel()
    {
        if (PlayerPrefs.GetInt("currentLevel", -27) != -27)
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }

        else
        {
            currentLevel = 0;
            SaveCurrentLevel();
        }

        if (activeLevelID == -27)
        {
            activeLevelID = currentLevel;
        }
        
        if (allLevels != null)
        {
            activeLevel = allLevels.levels[activeLevelID];
        }
    }

    public void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        activeLevelID = currentLevel;
    }

    public void GameOver()
    {
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
        
        PlayerPrefs.SetInt("money", money + score);
        PlayerPrefs.Save();
        menu = "GameOver";

        StartCoroutine(sceneFade.LoadScene("Menu"));
    }

    public void LevelCompleted()
    {
        if (activeLevelID == currentLevel)
        {
            if (activeLevelID == 29)
                lastLevelCompleted = true;
            
            currentLevel = currentLevel + 1;
            SaveCurrentLevel();
            menu = "LevelCompleted";
        }

        else
        {
            menu = "Start";
        }

        
        
        StartCoroutine(sceneFade.LoadScene("Menu"));
    }

    public void ControlButtonClicked(Button btn)
    {
        if (options["joystick"] == "true")
        {
            options["joystick"] = "false";
        }

        else
        {
            options["joystick"] = "true";
        }

        SaveOptions();
        LoadUserOptions();
    }

    public void ModeButtonClicked(Button btn)
    {
        if (options["tablet-mode"] == "true")
        {
            options["tablet-mode"] = "false";
        }

        else
        {
            options["tablet-mode"] = "true";
        }

        SaveOptions();
        LoadUserOptions();
    }

     public void FramerateButtonClicked(Button btn)
    {
        if (framerate == 60)
        {
            framerate = 30;
            options["framerate"] = "30";
        }

        else
        {
            framerate = 60;
            options["framerate"] = "60";
        }

        SaveOptions();
    }

    void JoystickSetUp()
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            if (options["joystick"] == "true")
            {
                joystickObject.SetActive(true);
                player.GetComponent<PlayerMover>().joystick = true;
            }
        }

        if (SceneManager.GetActiveScene().name == "Options")
        {
            if (options["joystick"] == "true")
            {
                controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: On";
                controlButtonText.GetComponentInParent<Image>().color = new Color(160f / 255f, 219f / 255f, 69f / 255f);
            }

            else
            {
                controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: Off";
                controlButtonText.GetComponentInParent<Image>().color = new Color(190f / 255f, 251f / 255f, 255f / 255f);
            }
        }
    }

    void ModeSetUp()
    {
        if (options["tablet-mode"] == "true")
        {
            verticalSize = 1.5f;
        }

        else
        {
            verticalSize = 1f;
        }

        if (SceneManager.GetActiveScene().name == "Options")
        {
            if (options["tablet-mode"] == "true")
            {
                modeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "         Tablet-Mode: On";
                modeButtonText.GetComponentInParent<Image>().color = new Color(160f / 255f, 219f / 255f, 69f / 255f);
            }

            else
            {
                modeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "         Tablet-Mode: Off";
                modeButtonText.GetComponentInParent<Image>().color = new Color(190f / 255f, 251f / 255f, 255f / 255f);
            }
        }
    }

    bool isTablet()
    {
        float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
    
            return diagonalInches;
        }

        bool deviceIsIpad = false;
        bool isAndroidTablet = false;

#if UNITY_IOS
        deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
#elif UNITY_ANDROID

        float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
        isAndroidTablet = (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);
#endif

        if (deviceIsIpad || isAndroidTablet)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void FramerateSetUp()
    {
        framerate = int.Parse(options["framerate"]);

        if (SceneManager.GetActiveScene().name == "Options")
        {
            framerateButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Framerate: " +framerate.ToString()+ " FPS";
        }

        Application.targetFrameRate = framerate;
    }

    void LoadUserOptions()
    {
        //old code
        /*if (PlayerPrefs.GetString("options", "Felix") != "Felix")
        {
            optionsObject = JsonConvert.DeserializeObject<Options>(PlayerPrefs.GetString("options"));
            if (SceneManager.GetActiveScene().name == "Play")
            {
                if (optionsObject.joystick == true)
                {
                    joystickObject.SetActive(true);
                    player.GetComponent<PlayerMover>().joystick = optionsObject.joystick;
                }
            }

            if (SceneManager.GetActiveScene().name == "Options")
            {
                if (optionsObject.joystick == true)
                {
                    controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: On";
                    controlButtonText.GetComponentInParent<Image>().color = new Color(160f / 255f, 219f / 255f, 69f / 255f);
                }

                else
                {
                    controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: Off";
                    controlButtonText.GetComponentInParent<Image>().color = new Color(190f / 255f, 251f / 255f, 255f / 255f);
                }
            }
        }

        else
        {
            optionsObject.joystick = false;
            SaveOptions();
        }*/

        //new code
        if (PlayerPrefs.GetString("UserOptions", "Felix") != "Felix")
        {

            bool save = false; //define if it should call SaveOptions()
            string json = PlayerPrefs.GetString("UserOptions");
            options = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (options.ContainsKey("joystick"))
            {
                JoystickSetUp();
            }

            else
            {
                options.Add("joystick", "false");
                save = true;
            }

            if (options.ContainsKey("framerate"))
            {
                FramerateSetUp();
            }

            else
            {
                options.Add("framerate", "30");
                save = true;
            }

            if (options.ContainsKey("tablet-mode"))
            {
                ModeSetUp();
            }

            else
            {
                string deviceIsTablet = isTablet().ToString().ToLowerInvariant();
                options.Add("tablet-mode", deviceIsTablet);
                save = true;
            }

            if (save)
            {
                SaveOptions();
            }
        }

        else
        {
            options = new Dictionary<string, string>();
            options.Add("joystick", "false");
            options.Add("framerate", "30");
            string deviceIsTablet = isTablet().ToString().ToLowerInvariant();
            options.Add("tablet-mode", deviceIsTablet);
            SaveOptions();
        }

    }

    public void SaveOptions()
    {
        string json = JsonConvert.SerializeObject(options);
        PlayerPrefs.SetString("UserOptions", json);
        PlayerPrefs.Save();
        LoadUserOptions();
        
        //probably delete this
        /*if (SceneManager.GetActiveScene().name == "Play")
        {
            if (optionsObject.joystick == true)
            {
                joystickObject.SetActive(true);
                player.GetComponent<PlayerMover>().joystick = optionsObject.joystick;
            }
        }*/
    }
/*
    void LoadFramerate()
    {
        if (PlayerPrefs.GetInt("framerate", -27) != -27)
        {
            framerate = PlayerPrefs.GetInt("framerate");

            if (SceneManager.GetActiveScene().name == "Options")
            {
                framerateButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Framerate: " +framerate.ToString()+ " FPS";
            }

            Application.targetFrameRate = framerate;
        }

        else
        {
            framerate = 30;
            Application.targetFrameRate = framerate;
            SaveFramerate();
        }
    }

    public void SaveFramerate()
    {
        PlayerPrefs.SetInt("framerate", framerate);
        PlayerPrefs.Save();
        LoadFramerate();
    }
*/
    void SaveMoney()
    {
        PlayerPrefs.SetInt("money", money);
    }

    void LoadSkins()
    {
        if (PlayerPrefs.GetString("skins", "Felix") != "Felix")
        {
            allSkins = JsonConvert.DeserializeObject<SkinsRoot>(PlayerPrefs.GetString("skins"));
			if (allSkins.lasers.Count == 3)
			{
				allSkins.lasers.AddRange(new Laser[]{
					new Laser{
                        	name = "Orange Laser",
	                        image = "LaserOrange9",
    	                    price = 45000,
        	                buyed = false,
            	            selected = false
                	},
	                new Laser{
    	                    name = "Violet Laser",
        	                image = "LaserViolet9",
            	            price = 20000,
                	        buyed = false,
                    	    selected = false
                    },
					new Laser{
                        	name = "Yellow Laser",
	                        image = "LaserYellow9",
    	                    price = 70000,
        	                buyed = false,
            	            selected = false
                	},
	                new Laser{
    	                    name = "Dark Blue Laser",
        	                image = "LaserBlueDark9",
            	            price = 30000,
                	        buyed = false,
                    	    selected = false
                    },
					new Laser{
    	                    name = "Dark Green Laser",
        	                image = "LaserGreenDark9",
            	            price = 35000,
                	        buyed = false,
                    	    selected = false
                    },
					new Laser{
    	                    name = "Dark Orange Laser",
        	                image = "LaserOrangeDark9",
            	            price = 50000,
                	        buyed = false,
                    	    selected = false
                    },
					new Laser{
    	                    name = "Dark Red Laser",
        	                image = "LaserRedDark9",
            	            price = 60000,
                	        buyed = false,
                    	    selected = false
                    },
					new Laser{
    	                    name = "Dark Violet Laser",
        	                image = "LaserVioletDark9",
            	            price = 100000,
                	        buyed = false,
                    	    selected = false
                    },
					new Laser{
    	                    name = "Dark Yellow Laser",
        	                image = "LaserYellowDark9",
            	            price = 50000,
                	        buyed = false,
                    	    selected = false
                    },
				});
			}

            if (allSkins.skins.Count == 5)
			{
				allSkins.skins.AddRange(new Skin[]{
                    new Skin{
                            name= "Beige Dark Alien",
                            image= "AlienBeigeDark_stand",
                            price= 10000,
                            buyed= false,
                            selected= false
                        },
                    new Skin{
                            name= "Green Dark Alien",
                            image= "AlienGreenDark_stand",
                            price= 15000,
                            buyed= false,
                            selected= false
                        },
                    new Skin{
                            name= "Pink Dark Alien",
                            image= "AlienPinkDark_stand",
                            price= 80000,
                            buyed= false,
                            selected= false
                        },
                   new Skin{
                            name= "Red Dark Alien",
                            image= "AlienRedDark_stand",
                            price= 200000,
                            buyed= false,
                            selected= false
                        },
                    new Skin{
                            name= "Red Alien",
                            image= "AlienRed_stand",
                            price= 70000,
                            buyed= false,
                            selected= false
                        },
                    new Skin{
                            name= "Violet Dark Alien",
                            image= "AlienVioletDark_stand",
                            price= 150000,
                            buyed= false,
                            selected= false
                        },
                    new Skin{
                            name= "Violet Alien",
                            image= "AlienViolet_stand",
                            price= 15000,
                            buyed= false,
                            selected= false
                        },
                });
            }

            if (allSkins.skins.Count == 12)
			{
				allSkins.skins.AddRange(new Skin[]{
					new Skin{
                        	name = "Masked Alien",
	                        image = "AlienWithMask",
    	                    price = 20000,
        	                buyed = false,
            	            selected = false
                	},
                    new Skin{
                        	name = "Xmas Alien",
	                        image = "SantaAlien",
    	                    price = 30000,
        	                buyed = false,
            	            selected = false
                	},
                });
            }
        
            if (allSkins.backgrounds.Count == 7)
			{
                allSkins.backgrounds.AddRange(new Background[]{
                    new Background{
                            name = "Dark Blue Grass",
                            image = "BG_BlueDark_grass",
                            price = 100000,
                            buyed = false,
                            selected = false
                        },
                    new Background{
                            name = "Dark Colored Grass",
                            image = "BG_ColoredDark_grass",
                            price = 40000,
                            buyed = false,
                            selected = false
                        },
                    new Background{
                            name = "Dark Colored Shroom",
                            image = "BG_ColoredDark_shroom",
                            price = 50000,
                            buyed = false,
                            selected = false
                        },
                    new Background{
                            name = "Dark Colored Desert",
                            image = "BG_ColoredDark_desert",
                            price = 20000,
                            buyed = false,
                            selected = false
                        },
                    new Background{
                            name = "Dark Red Desert",
                            image = "BG_RedDark_desert",
                            price = 60000,
                            buyed = false,
                            selected = false
                        },
                    new Background{
                            name = "Dark Violet Desert",
                            image = "BG_VioletDark_desert",
                            price = 200000,
                            buyed = false,
                            selected = false
                        },
                });
            }

            if (allSkins.backgrounds.Count == 13)
			{
                allSkins.backgrounds.AddRange(new Background[]{
                    new Background{
                            name = "Halloween",
                            image = "HalloweenBackgroundWide",
                            price = 10000,
                            buyed = false,
                            selected = false
                        },
                });
            }

            if (allSkins.backgrounds.Count == 14)
			{
                allSkins.backgrounds.AddRange(new Background[]{
                    new Background{
                            name = "Snow Land",
                            image = "SnowBackground",
                            price = 40000,
                            buyed = false,
                            selected = false
                        },
                });
            }

            if (allSkins.backgrounds.Count == 15)
			{
                allSkins.backgrounds.AddRange(new Background[]{
                    new Background{
                            name = "Jungle",
                            image = "JungleBackground",
                            price = 50000,
                            buyed = false,
                            selected = false
                        },

                    new Background{
                            name = "Egypt",
                            image = "EgyptBackground",
                            price = 500000,
                            buyed = false,
                            selected = false
                        },

                    new Background{
                            name = "Switzerland",
                            image = "SwitzerlandBackground",
                            price = 1000000,
                            buyed = false,
                            selected = false
                        },
                });
            }

        }

        else
        {
            allSkins = new SkinsRoot();
            allSkins.skins = new List<Skin>() {
                new Skin{
                        name= "Beige Alien",
                        image= "AlienBeige_stand",
                        price= 0,
                        buyed= true,
                        selected= true
                    },
                new Skin{
                        name= "Blue Alien",
                        image= "AlienBlue_stand",
                        price= 30000,
                        buyed= false,
                        selected= false
                    },
                new Skin{
                        name= "Green Alien",
                        image= "AlienGreen_stand",
                        price= 50000,
                        buyed= false,
                        selected= false
                    }, 
                new Skin{
                        name= "Pink Alien",
                        image= "AlienPink_stand",
                        price= 70000,
                        buyed= false,
                        selected= false
                    },
                new Skin{
                        name= "Yellow Alien",
                        image= "AlienYellow_stand",
                        price= 100000,
                        buyed= false,
                        selected= false
                    },
            };

            allSkins.backgrounds = new List<Background>() {
                new Background{
                        name = "Blue Land",
                        image = "BG_Blue_land",
                        price = 0, buyed = true,
                        selected = true
                    },
                new Background{
                        name = "Blue Grass",
                        image = "BG_Blue_grass",
                        price = 20000,
                        buyed = false,
                        selected = false
                    }, 
                new Background{
                        name = "Blue Shroom",
                        image = "BG_Blue_shroom",
                        price = 20000,
                        buyed = false,
                        selected = false
                    },
                new Background{
                        name = "Colored Land",
                        image = "BG_Colored_land",
                        price = 35000,
                        buyed = false,
                        selected = false
                    },
                new Background{
                        name = "Colored Grass",
                        image = "BG_Colored_grass",
                        price = 50000,
                        buyed = false,
                        selected = false
                    },
                new Background{
                        name = "Colored Desert",
                        image = "BG_Colored_desert",
                        price = 80000,
                        buyed = false,
                        selected = false
                    },
                new Background{
                        name = "Colored Shroom",
                        image = "BG_Colored_shroom",
                        price = 80000,
                        buyed = false,
                        selected = false
                    },
            };

            allSkins.lasers = new List<Laser>(){
                new Laser{
                        name = "Blue Laser",
                        image = "LaserBlue9",
                        price = 0,
                        buyed = true,
                        selected = true
                    },
                new Laser{
                        name = "Red Laser",
                        image = "LaserRed9",
                        price = 40000,
                        buyed = false,
                        selected = false
                    },
                new Laser{
                        name = "Green Laser",
                        image = "LaserGreen9",
                        price = 60000,
                        buyed = false,
                        selected = false
                    },
            };
            SaveSkins();
        }
    }

    public void SaveSkins()
    {
        string json = JsonConvert.SerializeObject(allSkins);
        PlayerPrefs.SetString("skins", json);
        PlayerPrefs.Save();
        LoadSkins();
        if (SceneManager.GetActiveScene().name == "Play")
        {
            foreach (Skin s in allSkins.skins)
            {
                if (s.selected == true)
                {
                    playerSprite = Resources.Load<Sprite>(s.image);
                    player.GetComponent<SpriteRenderer>().sprite = playerSprite;
                }
            }
        }
    }

    public void BuySkin(Button button)
    {
        foreach (Skin s in allSkins.skins)
        {
            if (s.name == button.name)
            {
                if (s.price > money)
                {
                    errorMessage.SetActive(true);
                    return;
                }

                foreach (Skin sk in allSkins.skins)
                {
                    sk.selected = false;
                }
                money = money - s.price;
                s.buyed = true;
                s.selected = true;
                SaveMoney();
                SaveSkins();
            }
        }

        foreach (Background b in allSkins.backgrounds)
        {
            if (b.name == button.name)
            {
                if (b.price > money)
                {
                    errorMessage.SetActive(true);
                    return;
                }

                foreach (Background ba in allSkins.backgrounds)
                {
                    ba.selected = false;
                }
                money = money - b.price;
                b.buyed = true;
                b.selected = true;
                SaveMoney();
                SaveSkins();
            }
        }

        foreach (Laser l in allSkins.lasers)
        {
            if (l.name == button.name)
            {
                if (l.price > money)
                {
                    errorMessage.SetActive(true);
                    return;
                }

                foreach (Laser la in allSkins.lasers)
                {
                    la.selected = false;
                }
                money = money - l.price;
                l.buyed = true;
                l.selected = true;
                SaveMoney();
                SaveSkins();
            }
        }

        LoadShop();
    }

    public void Restart()
    {
        StartCoroutine(sceneFade.LoadScene("Play"));    
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void LoadMySkins()
    {
        SceneManager.LoadScene("MySkins");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadHowToPlay()
    {
        lastMenu = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("HowToPlay");
    }
    
    public void LoadLastMenu()
    {
        SceneManager.LoadScene(lastMenu);
    }

    public void LoadLevelMenu()
    {
        SceneManager.LoadScene("Levels");
    }

    public void UpdateScore(int s)
    {
        score = score + s;
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score.ToString();

        if (activeLevel.id == 30)
        {
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            progressText.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        }
    }
    [System.Serializable]
    public class Skin    {
        public string name; 
        public string image; 
        public int price; 
        public bool buyed; 
        public bool selected; 
    }

    [System.Serializable]
    public class Background    {
        public string name; 
        public string image; 
        public int price; 
        public bool buyed; 
        public bool selected; 
    }

    [System.Serializable]
    public class Laser    {
        public string name; 
        public string image; 
        public int price; 
        public bool buyed; 
        public bool selected; 
    }

    [System.Serializable]
    public class SkinsRoot    {
        public List<Skin> skins = new List<Skin>(); 
        public List<Background> backgrounds = new List<Background>(); 
        public List<Laser> lasers = new List<Laser>(); 
    }


    [System.Serializable]
    public class Level    {
        public int id;
        public string page;
        public string bgSprite;
        public string obstclSprite;
        public int difficulty;
        public float speed;
        public int waveCount;
        public float waveWait;
        public float obstacleRespawnTime;
        public float coinRespawnTime;
    }

    [System.Serializable]
    public class LevelsRoot    {
        public List<Level> levels = new List<Level>();
    }
}
