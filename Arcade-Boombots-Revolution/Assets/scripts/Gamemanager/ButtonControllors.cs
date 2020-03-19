using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllors : Gamemanager
{
    private Gamemanager gameMangerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameMangerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager>();
    }

    //starts next wave
    public override void Startnextwave()
    {
        gameMangerScript.Startnextwave();
    }

    //shop interactions
    public void OpenShop()
    {
        loadScene("Shop");
    }
}
