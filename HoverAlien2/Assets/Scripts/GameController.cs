using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.Advertisements;



public class GameController : MonoBehaviour
{
    public static int score;
    public static string menu;
    public static string lastMenu;
    public Root allSkins;
    public Options optionsObject;
    public bool adsEnabled;
    public int highscore;
    public int money;
    public GameObject joystickObject;
    public GameObject titleText;
    public GameObject scoreText;
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
    public GameObject adsButtonText;
    public GameObject tiltingOptionsButton;
    public GameObject joystickOptionsButton;

    public bool retry = true;
    public static bool retried;
    public GameObject retryPopUp;
    string placement = "rewardedVideo";

    public GameObject errorMessage;
    // Start is called before the first frame update
    
    
    void OnEnable ()
    {
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().name == "Start")
        {
            menu = "Start";
            lastMenu = "Start";
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

        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (menu == "GameOver")
            {
                //menu = "";
                highscore = PlayerPrefs.GetInt("highscore", 0);
                playButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Restart";
                titleText.GetComponent<TMPro.TextMeshProUGUI>().text = "Game Over";
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + highscore + "\n" + "Your Score: " + score;
            }

            else if (menu == "Start")
            {
                highscore = PlayerPrefs.GetInt("highscore", 0);
                playButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Play";
                titleText.GetComponent<TMPro.TextMeshProUGUI>().text = "Hover Alien";
                scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + highscore;
            }

            LoadSkins();
            foreach (Background b in allSkins.backgrounds)
            {
                if (b.selected == true)
                {
                    background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                }
            }
        }

        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            money = PlayerPrefs.GetInt("money", 0);
            moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = money.ToString();
            LoadSkins();
            foreach (Background b in allSkins.backgrounds)
            {
                if (b.selected == true)
                {
                    background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                }
            }
        }

        else if (SceneManager.GetActiveScene().name == "MySkins")
        {
            money = PlayerPrefs.GetInt("money", 0);
            moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = money.ToString();
            LoadSkins();
            foreach (Background b in allSkins.backgrounds)
            {
                if (b.selected == true)
                {
                    background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                }
            }
        }

        else if (SceneManager.GetActiveScene().name == "Options" || SceneManager.GetActiveScene().name == "HowToPlay")
        {
            LoadSkins();
            LoadUserOptions();
            LoadAdsOptions();
            foreach (Background b in allSkins.backgrounds)
            {
                if (b.selected == true)
                {
                    background1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                    background3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(b.image);
                }
            }
        }

        else if (SceneManager.GetActiveScene().name == "Play")
        {
            highscore = PlayerPrefs.GetInt("highscore", 0);
            money = PlayerPrefs.GetInt("money", 0);

            if (retried == false)
            {
                score = 0;
            }

            scoreText.GetComponent<TMPro.TextMeshPro>().text = score.ToString();

            LoadSkins();
            LoadUserOptions();
            LoadAdsOptions();
            Debug.Log(allSkins.skins[0].image);
            foreach (Skin s in allSkins.skins)
            {
                if (s.selected == true)
                {
                    playerSprite = Resources.Load<Sprite>(s.image);
                    player.GetComponent<SpriteRenderer>().sprite = playerSprite;
                }
            }

            foreach (Background b in allSkins.backgrounds)
            {
                if (b.selected == true)
                {
                    Camera.main.GetComponent<BackgroundLoop>().backgroundSprite = Resources.Load<Sprite>(b.image);
                }
            }

            foreach (Laser l in allSkins.lasers)
            {
                if (l.selected == true)
                {
                    Debug.Log("laser");
                    player.GetComponent<ShootLaser>().laserSprite = Resources.Load<Sprite>(l.image);
                }
            }
        }
    }
 
    public void GameOverRetry()
    {
        if (retry && Advertisement.IsReady(placement) && !retried && score > 1990 && adsEnabled != false)
        {
            Time.timeScale = 0;
            joystickObject.SetActive(false);
            retryPopUp.SetActive(true);
        }
        
        else
        {
            GameOver();
        }
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
        retried = false;
        menu = "GameOver";
        SceneManager.LoadScene("Menu");
    }

    public void AdCompleted()
    {
        retried = true;
        SceneManager.LoadScene("Play");
    }

    public void ControlButtonClicked(Button btn)
    {
        optionsObject.joystick = !optionsObject.joystick;
        SaveOptions();
        LoadUserOptions();
        if (optionsObject.joystick == true)
        {
            controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: On";
            controlButtonText.GetComponentInParent<Image>().color = new Color(160f / 255f, 219f / 255f, 69f / 255f);
            //tiltingOptionsButton.SetActive(false);
            //joystickOptionsButton.SetActive(true);
        }

        else
        {
            controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: Off";
            controlButtonText.GetComponentInParent<Image>().color = new Color(190f / 255f, 251f / 255f, 255f / 255f);
            //tiltingOptionsButton.SetActive(true);
            //joystickOptionsButton.SetActive(false);
        }

    }

    public void AdsButtonClicked(Button btn)
    {
        adsEnabled = !adsEnabled;
        SaveAdsOptions();
        LoadAdsOptions();
    }

    void LoadUserOptions()
    {
        if (PlayerPrefs.GetString("options", "Felix") != "Felix")
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
                    //tiltingOptionsButton.SetActive(false);
                    //joystickOptionsButton.SetActive(true);
                }

                else
                {
                    controlButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "    Onscreen Joystick: Off";
                    controlButtonText.GetComponentInParent<Image>().color = new Color(190f / 255f, 251f / 255f, 255f / 255f);
                    //tiltingOptionsButton.SetActive(true);
                    //joystickOptionsButton.SetActive(false);
                }
            }
        }

        else
        {
            optionsObject.joystick = false;
            SaveOptions();
        }
    }

    public void SaveOptions()
    {
        string json = JsonConvert.SerializeObject(optionsObject);
        PlayerPrefs.SetString("options", json);
        PlayerPrefs.Save();
        LoadUserOptions();
        if (SceneManager.GetActiveScene().name == "Play")
        {
            if (optionsObject.joystick == true)
            {
                joystickObject.SetActive(true);
                player.GetComponent<PlayerMover>().joystick = optionsObject.joystick;
            }
        }
    }

    void LoadAdsOptions()
    {
        if (PlayerPrefs.GetString("adsOptions", "Felix") != "Felix")
        {
            adsEnabled = JsonConvert.DeserializeObject<bool>(PlayerPrefs.GetString("adsOptions"));

            if (SceneManager.GetActiveScene().name == "Options")
            {
                if (adsEnabled)
                {
                    adsButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "            Ads enabled";
                    adsButtonText.GetComponentInParent<Image>().color = new Color(160f / 255f, 219f / 255f, 69f / 255f);
                    //tiltingOptionsButton.SetActive(false);
                    //joystickOptionsButton.SetActive(true);
                }

                else
                {
                    adsButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "            Ads disabled";
                    adsButtonText.GetComponentInParent<Image>().color = new Color(190f / 255f, 251f / 255f, 255f / 255f);
                    //tiltingOptionsButton.SetActive(true);
                    //joystickOptionsButton.SetActive(false);
                }
            }
        }

        else
        {
            adsEnabled = true;
            SaveAdsOptions();
        }
    }

    public void SaveAdsOptions()
    {
        string json = JsonConvert.SerializeObject(adsEnabled);
        PlayerPrefs.SetString("adsOptions", json);
        PlayerPrefs.Save();
        LoadUserOptions();
    }

    void SaveMoney()
    {
        PlayerPrefs.SetInt("money", money);
    }

    void LoadSkins()
    {
        Debug.Log(PlayerPrefs.GetString("skins", ""));
        //Debug.Log(JsonUtility.ToJson(allSkins));
        if (PlayerPrefs.GetString("skins", "Felix") != "Felix")
        {
            Debug.Log(PlayerPrefs.GetString("skins"));
            allSkins = JsonConvert.DeserializeObject<Root>(PlayerPrefs.GetString("skins"));
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
            allSkins = new Root();
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
            //Dictionary<string, List<Dictionary<string, string>>>
            //string j = "{\"skins\= [{\name\= \"Alien Beige\", \image\= \"AlienBeige_stand\", \price\= 0, \buyed\= true, \selected\= true}, {\name\= \"Alien Blue\", \image\= \"AlienBlue_stand\", \price\= 30000, \buyed\= false, \selected\= false}, {\name\= \"Alien Green\", \image\= \"AlienGreen_stand\", \price\= 50000, \buyed\= false, \selected\= false}, {\name\= \"Alien Pink\", \image\= \"AlienPink_stand\", \price\= 70000, \buyed\= false, \selected\= false}, {\name\= \"Alien Yellow\", \image\= \"AlienYellow_stand\", \price\= 100000, \buyed\= false, \selected\= false}], \"backgrounds\= [{\name\= \"Blue Land\", \image\= \"BG_Blue_land\", \price\= 0, \buyed\= true, \selected\= true}, {\name\= \"Blue Grass\", \image\= \"BG_Blue_grass\", \price\= 20000, \buyed\= false, \selected\= false}, {\name\= \"Blue Shroom\", \image\= \"BG_Blue_shroom\", \price\= 20000, \buyed\= false, \selected\= false}, {\name\= \"Colored Land\", \image\= \"BG_Colored_land\", \price\= 35000, \buyed\= false, \selected\= false}, {\name\= \"Colored Grass\", \image\= \"BG_Colored_grass\", \price\= 50000, \buyed\= false, \selected\= false}, {\name\= \"Colored Desert\", \image\= \"BG_Colored_desert\", \price\= 80000, \buyed\= false, \selected\= false}, {\name\= \"Colored Shroom\", \image\= \"BG_Colored_shroom\", \price\= 80000, \buyed\= false, \selected\= false}], \"lasers\= [{\name\= \"Blue Laser\", \image\= \"sp=LaserBlue9\", \price\= 0, \buyed\= true, \selected\= true}, {\name\= \"Red Laser\", \image\= \"sp=LaserRed9\", \price\= 40000, \buyed\= false, \selected\= false}, {\name\= \"Green Laser\", \image\= \"sp=LaserGreen9\", \price\= 60000, \buyed\= false, \selected\= false}]}";
            //Root x = JsonUtility.FromJson<Root>(jsonFile.text);
            //Debug.Log(JsonUtility.FromJson<Root>(jsonFile.text));
            //foreach (string key in x.Keys)
            //{
            //    Debug.Log(key);
            //}
            //Debug.Log("");
            Debug.Log(JsonConvert.SerializeObject(allSkins));
            Debug.Log(allSkins.backgrounds.Count);
            SaveSkins();
            Debug.Log(JsonConvert.SerializeObject(allSkins));
        }
        Debug.Log(JsonConvert.SerializeObject(allSkins));

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
        SceneManager.LoadScene("Play");     
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

    public void UpdateScore(int s)
    {
        score = score + s;
        scoreText.GetComponent<TMPro.TextMeshPro>().text = score.ToString();
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
    public class Root    {
        public List<Skin> skins = new List<Skin>(); 
        public List<Background> backgrounds = new List<Background>(); 
        public List<Laser> lasers = new List<Laser>(); 
    }

    [System.Serializable]
    public class Options    {
        public bool joystick; 
    }
}
