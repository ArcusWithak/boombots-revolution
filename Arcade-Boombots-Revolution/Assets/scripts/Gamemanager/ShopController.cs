using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : Gamemanager
{
    private Gamemanager gameManagerScript;

    ////declaring price

    //wall Price
    public int wallPrice;
    public int wallHealthIncrease;
    public int wallPriceIncrease;

    [Space(10)]
    //gun Prices
    public int gunPrice;

    //declaring text
    //[Space(11)]
    [TextArea]
    public string[] greetingMessage;
    //[Space(12)]
    [TextArea]
    public string[] purchaseMessage;
    //[Space(13)]
    [TextArea]
    public string[] rejectionMessage;
    //declaring Ui items
    [Space(14)]
    public Text speachText;
    private float speachReset;
    [Space(15)]
    public Text currencyText;
    public Text wallPriceText;
    [Space(16)]
    public Sprite[] gunSprites;
    public Image gunRack;
    public Text gunPriceText;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager>();
        AjustSpeachBubble(greetingMessage);
        currencyText.text = gameManagerScript.currency.ToString();
        wallPrice += (gameManagerScript.wallHealthIncreaseAmount * wallPriceIncrease);
        wallPriceText.text = "price:" + wallPrice;
        gunPrice += (gameManagerScript.maxGunUnlocked + 1) * 25;
        gunPriceText.text = "price:" + gunPrice;
        gunRack.sprite = gunSprites[gameManagerScript.maxGunUnlocked];
        if (gameManagerScript.maxGunUnlocked >= gunSprites.Length - 1)
        {
            gunRack.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speachReset <= 0)
        {
            speachText.text = "Can i do anything else \nfor you?";
        }
        else if (speachReset < 6)
        {
            speachReset -= Time.deltaTime;
        }
    }

    //return to level
    public void CloseShop()
    {
        loadScene("Level");
        gameManagerScript.updateScene = true;
    }

    //speach bubble text
    public void AjustSpeachBubble(string[] message)
    {
        int speachIndex;
        speachIndex = Random.Range(0, message.Length);
        speachText.text = message[speachIndex];
        if (message != greetingMessage)
        {
            speachReset = 3;
        }
        else
        {
            speachReset = 6;
        }
    }

    //upgrade wall health

    public void UpgradeWallHealth()
    {
        //checking if player has enough currency
        if (gameManagerScript.currency >= wallPrice)
        {
            //ajusting text 
            AjustSpeachBubble(purchaseMessage);
            //remove cost
            gameManagerScript.currency -= wallPrice;
            //increase wall health
            gameManagerScript.wallHealth += wallHealthIncrease;
            gameManagerScript.wallHeathMax += wallHealthIncrease;
            //increasing price
            wallPrice += wallPriceIncrease;
            //mantaining price increases
            gameManagerScript.wallHealthIncreaseAmount++;
            //update currency text
            currencyText.text = gameManagerScript.currency.ToString();
            wallPriceText.text = "price:" + wallPrice;
        }
        else
        {
            AjustSpeachBubble(rejectionMessage);
        }
    }

    //unlock guns
    public void UnlockGun()
    {
        if (gameManagerScript.currency >= gunPrice && gameManagerScript.maxGunUnlocked < gunSprites.Length - 1)
        {
            //ajusting text 
            AjustSpeachBubble(purchaseMessage);

            //remove cost
            gameManagerScript.currency -= gunPrice;

            //unlock new gun
            gameManagerScript.maxGunUnlocked++;

            //change price
            if (gameManagerScript.maxGunUnlocked >= gunSprites.Length - 1)
            {
                gunRack.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gunRack.transform.localScale = new Vector3(0.25f, 0.2f, gunRack.transform.localScale.z);
            }
            else
            {
                //change price
                gunPrice += 25;
                gunPriceText.text = "price:" + gunPrice;
            }

            //change gun image
            gunRack.sprite = gunSprites[gameManagerScript.maxGunUnlocked];

            //update currency text
            currencyText.text = gameManagerScript.currency.ToString();
        }
        else
        {
            AjustSpeachBubble(rejectionMessage);
        }
    }

}
