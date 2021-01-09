using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkinLoader : MonoBehaviour
{
    public GameObject skinTemplate;
    public GameController gameController;
    public GameObject popUp;
    public int sks;
    public int bgs;
    public int lrs;
    //public class SkinsRoot{};
    // Start is called before the first frame update
    void OnEnable()
    {
        sks = 0;
        bgs = 0;
        lrs = 0;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        GameController.SkinsRoot allSkins = gameController.allSkins;
        if (this.name == "PageSkins_MySkins")
        {
            if (transform.childCount > 0)
            {
                return;
            }

            else
            {
                int skis = 0;
                foreach (GameController.Skin s in allSkins.skins)
                {
                    if (s.buyed == true)
                    {
                        CreateSkinObject(s, transform);
                        skis = skis + 1;
                    }
                }
                //this.GetComponent<RectTransform>().sizeDelta = new Vector2((192.5F * 1F * skis) -192.5F * 2 * 1.8F - 175.5F / 2, 260);
            }
        }

        else if (this.name == "PageBackgrounds_MySkins")
        {
            if (transform.childCount > 0)
            {
                return;
            }

            else
            {
                int bgrs = 0;
                foreach (GameController.Background b in allSkins.backgrounds)
                {
                    if (b.buyed == true)
                    {
                        CreateBackgroundObject(b, transform);
                        bgrs = bgrs + 1;
                    }
                }
                //this.GetComponent<RectTransform>().sizeDelta = new Vector2((192.5F * 1F * bgrs) -192.5F * 2 * 1.8F - 175.5F / 2, 260);
            }
        }

        else if (this.name == "PageLasers_MySkins")
        {
            if (transform.childCount > 0)
            {
                return;
            }

            else
            {
                int lasrs = 0;
                foreach (GameController.Laser l in allSkins.lasers)
                {
                    if (l.buyed == true)
                    {
                        CreateLaserObject(l, transform);
                        lasrs = lasrs + 1;
                    }
                }
                //this.GetComponent<RectTransform>().sizeDelta = new Vector2((192.5F * 1F * lasrs) -192.5F * 2 * 1.8F - 175.5F / 2, 260);
            }
        }

        else if (this.name == "PageSkins_Shop")
        {
            if (transform.childCount > 0)
            {
                return;
            }

            else
            {
                int skis = 0;
                foreach (GameController.Skin s in allSkins.skins)
                {
                    if (s.buyed == false)
                    {
                        CreateSkinObject(s, transform);
                        skis = skis + 1;
                    }
                }
                this.GetComponent<RectTransform>().sizeDelta = new Vector2((192.5F * 1F * skis) -192.5F * 2 * 1.8F - 175.5F / 2, 260);
            }
        }

        else if (this.name == "PageBackgrounds_Shop")
        {
            if (transform.childCount > 0)
            {
                return;
            }

            else
            {
                int bgrs = 0;
                foreach (GameController.Background b in allSkins.backgrounds)
                {
                    if (b.buyed == false)
                    {
                        CreateBackgroundObject(b, transform);
                        bgrs = bgrs + 1;
                    }
                }
                this.GetComponent<RectTransform>().sizeDelta = new Vector2((192.5F * 1F * bgrs) -192.5F * 2 * 1.8F - 175.5F / 2, 260);
            }
        }

        else if (this.name == "PageLasers_Shop")
        {
            if (transform.childCount > 0)
            {
                return;
            }

            else
            {
                int lasrs = 0;
                foreach (GameController.Laser l in allSkins.lasers)
                {
                    if (l.buyed == false)
                    {
                        CreateLaserObject(l, transform);
                        lasrs = lasrs + 1;
                    }
                }
                this.GetComponent<RectTransform>().sizeDelta = new Vector2((192.5F * 1F * lasrs) -192.5F * 2 * 1.8F - 175.5F / 2, 260);
            }
        }
    }

    private void CreateSkinObject(GameController.Skin skin, Transform container)
    {
        sks = sks + 1;
        Transform skinObject = Instantiate(skinTemplate.transform, container);
        
        Vector3 pos = transform.position;
        skinObject.transform.position = pos + new Vector3(192.5F * 1.8F * (float)sks - 175.5F * 1.8F, -17.5F * 1.8F, 0);
        skinObject.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = skin.name;
        skinObject.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(skin.image);
        TMPro.TextMeshProUGUI buttonText = skinObject.GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        skinObject.GetChild(2).name = skin.name;
        Button button = skinObject.GetChild(2).GetComponent<Button>();
        button.name = skin.name;

        if (skin.buyed == true)
        {
            if (skin.selected == true)
            {
                buttonText.text = "Selected";
                button.transform.GetComponent<Image>().color = new Color(160F / 255, 219F / 255, 69F / 255);
            }

            else
            {
                button.onClick.AddListener(() => SelectSkin(button));
                buttonText.text = "Select";
                button.transform.GetComponent<Image>().color = new Color(190F / 255, 251F / 255, 255F / 255);
            }
        }

        else
        {
            button.name = "Buy " + skin.name + " for " + skin.price + " Coins?";
            button.onClick.AddListener(() => popUp.SetActive(true));
            button.onClick.AddListener(() => popUp.transform.GetChild(2).name = skin.name);
            button.onClick.AddListener(() => popUp.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = button.name);
            buttonText.text = skin.price + " Coins";
        }
        
    }

    private void CreateBackgroundObject(GameController.Background bg, Transform container)
    {
        bgs = bgs + 1;
        Transform bgObject = Instantiate(skinTemplate.transform, container);
        
        Vector3 pos = transform.position;
        bgObject.transform.position = pos + new Vector3(192.5F * 1.8F * (float)bgs - 175.5F * 1.8F, -17.5F * 1.8F, 0);
        bgObject.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = bg.name;
        bgObject.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(bg.image);
        TMPro.TextMeshProUGUI buttonText = bgObject.GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        bgObject.GetChild(2).name = bg.name;
        Button button = bgObject.GetChild(2).GetComponent<Button>();
        button.name = bg.name;

        if (bg.buyed == true)
        {
            if (bg.selected == true)
            {
                buttonText.text = "Selected";
                button.transform.GetComponent<Image>().color = new Color(160F / 255, 219F / 255, 69F / 255);
            }

            else
            {
                button.onClick.AddListener(() => SelectSkin(button));
                buttonText.text = "Select";
                button.transform.GetComponent<Image>().color = new Color(190F / 255, 251F / 255, 255F / 255);
            }
        }

        else
        {
            button.name = "Buy " + bg.name + " for " + bg.price + " Coins?";
            button.onClick.AddListener(() => popUp.SetActive(true));
            button.onClick.AddListener(() => popUp.transform.GetChild(2).name = bg.name);
            button.onClick.AddListener(() => popUp.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = button.name);
            buttonText.text = bg.price + " Coins";
        }
        
    }

    private void CreateLaserObject(GameController.Laser lsr, Transform container)
    {
        lrs = lrs + 1;
        Transform lsrObject = Instantiate(skinTemplate.transform, container);
        
        Vector3 pos = transform.position;
        lsrObject.transform.position = pos + new Vector3(192.5F * 1.8F * (float)lrs - 175.5F * 1.8F, -17.5F * 1.8F, 0);
        lsrObject.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = lsr.name;
        lsrObject.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(lsr.image);
        TMPro.TextMeshProUGUI buttonText = lsrObject.GetChild(2).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        lsrObject.GetChild(2).name = lsr.name;
        Button button = lsrObject.GetChild(2).GetComponent<Button>();
        button.name = lsr.name;

        if (lsr.buyed == true)
        {
            if (lsr.selected == true)
            {
                buttonText.text = "Selected";
                button.transform.GetComponent<Image>().color = new Color(160F / 255, 219F / 255, 69F / 255);
            }

            else
            {
                button.onClick.AddListener(() => SelectSkin(button));
                buttonText.text = "Select";
                button.transform.GetComponent<Image>().color = new Color(190F / 255, 251F / 255, 255F / 255);
            }
        }

        else
        {
            button.name = "Buy " + lsr.name + " for " + lsr.price + " Coins?";
            button.onClick.AddListener(() => popUp.SetActive(true));
            button.onClick.AddListener(() => popUp.transform.GetChild(2).name = lsr.name);
            button.onClick.AddListener(() => popUp.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = button.name);
            buttonText.text = lsr.price + " Coins";
        }
        
    }

    public void SelectSkin(Button button)
    {
        GameController.SkinsRoot allSkins = gameController.allSkins;
        foreach (GameController.Skin s in allSkins.skins)
        {
            if (s.name == button.name)
            {
                foreach (GameController.Skin sk in allSkins.skins)
                {
                    sk.selected = false;
                }
                s.selected = true;
                gameController.allSkins = allSkins;
                gameController.SaveSkins();
                SceneManager.LoadScene("MySkins");
            }
        }

        foreach (GameController.Background b in allSkins.backgrounds)
        {
            if (b.name == button.name)
            {
                foreach (GameController.Background ba in allSkins.backgrounds)
                {
                    ba.selected = false;
                }
                b.selected = true;
                gameController.allSkins = allSkins;
                gameController.SaveSkins();
                SceneManager.LoadScene("MySkins");
            }
        }

        foreach (GameController.Laser l in allSkins.lasers)
        {
            if (l.name == button.name)
            {
                foreach (GameController.Laser la in allSkins.lasers)
                {
                    la.selected = false;
                }
                l.selected = true;
                gameController.allSkins = allSkins;
                gameController.SaveSkins();
                SceneManager.LoadScene("MySkins");
            }
        }
    }
}
