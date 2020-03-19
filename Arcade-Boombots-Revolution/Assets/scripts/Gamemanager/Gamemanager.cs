using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : shortcuts
{
    //declaration
    public int wave;
    private bool waveUpdate = true;
    private bool waveSpawned;
    public bool rest;

    private int spawnPointNumb;
    private float spawnStartingHight;
    private float spawningDistance;
    private float spawnPointX;

    [HideInInspector]
    public Vector3[] spawningPoints;

    [Space(12)]
    public GameObject[] enemyTypes;

    [Space(13)]
    public GameObject waveMeter;

    [Space(15)]
    //declaring health and health bar
    public float wallHealth;
    public float wallHeathMax;
    public Image wallHeathBar;
    public Text wallHeathText;

    [Space(16)]
    public GameObject menuItems;

    [Space(15)]
    //declaring currency
    public float currency;

    //declaring price increases amount
    public int wallHealthIncreaseAmount;

    //declaring max guntype value
    public int maxGunUnlocked = 0;

    ////delcaring current Scene
    [Space(18)]
    public bool updateScene;
    public bool death;
    public bool victory;

    //declaring music soucreses
    [Space(19)]
    public AudioSource battleMusic;
    public AudioSource shopMusic;
    public AudioSource idleMusic;
    public AudioSource victoryMusic;
    public AudioSource defeatMusic;

    //declaring bullets variables
    [Space(20)]
    public GameObject bullet;
    private GameObject armJoint;
    private GameObject barrel;

    //declaring victory objects
    [Space(21)]
    private Text victoryText;
    public GameObject backUp;

    void Awake()
    {
        //asigning spawn variables
        spawnPointNumb = 3;
        spawnStartingHight = 1.5f;
        spawningDistance = 2.5f;
        spawnPointX = 12;

        //resizing spawning point array
        spawningPoints = new Vector3[spawnPointNumb];

        //asigning spawning points
        for (int I = 0; I < spawnPointNumb; I++)
        {
            spawningPoints[I] = new Vector3(10, spawnStartingHight + (I * spawningDistance));
        }

        //asinging wall health
        wallHealth = 100;
        wallHeathMax = wallHealth;

        //preassining load path
        loadScenesetpath("Assets/Scenes/");

        //starting sleep fuctions
        SLeeprestart(10);
    }

    // Start is called before the first frame update
    private void Start()
    {
        //make sure this doesn't get destroyed
        DontDestroyOnLoad(this.gameObject);

        //reseting
        wave = 0;
        maxGunUnlocked = 0;

        //assinging barrel
        armJoint = GameObject.FindGameObjectWithTag("Barrel");
        barrel = armJoint.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //play shop music
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            if (!shopMusic.isPlaying)
            {
                idleMusic.Pause();
                shopMusic.Play();
            }
            else
            {
                idleMusic.Pause();
                shopMusic.UnPause();
            }
        }

        //escape
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        //checking witch scene is active and running code based on it
        if (SceneManager.GetActiveScene().name == "Level")
        {
            //update missing variables
            if (updateScene)
            {
                waveMeter = GameObject.FindGameObjectWithTag("WaveCounter");
                waveMeter.transform.rotation = new Quaternion(0, 0, 0, 0);
                waveMeter.transform.Rotate(0, 0, -9 * wave);

                wallHeathBar = GameObject.FindGameObjectWithTag("HealthBar").transform.GetChild(0).GetComponent<Image>();
                wallHeathText = GameObject.FindGameObjectWithTag("HealthBar").GetComponentInChildren<Text>();

                //assining text
                victoryText = GameObject.FindGameObjectWithTag("victorytext").GetComponent<Text>();
                victoryText.gameObject.SetActive(false);

                menuItems = GameObject.FindGameObjectWithTag("MenuItems");

                GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller>().ChangeArmor();

                shopMusic.Pause();
                idleMusic.UnPause();

                updateScene = false;
            }

            //update heath
            resourcebarEmpty(wallHealth, 100, wallHeathBar);
            updatetext(wallHeathText, wallHeathMax.ToString() + "/" + wallHealth.ToString());


            //activate\deactivate menu Items
            {
                if (rest && !menuItems.activeSelf)
                {
                    menuItems.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else if (!rest && menuItems.activeSelf)
                {
                    menuItems.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            //update wave if all enemy's are killed
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && !rest && waveUpdate && !victory)
            {
                if (wave == 10)
                {
                    battleMusic.Stop();
                    idleMusic.Pause();
                    victory = true;
                    waveSpawned = false;
                    StartCoroutine(SpawnArmy());
                    victoryMusic.time = 2f;
                    victoryMusic.Play();
                }
                else
                {
                    battleMusic.Stop();
                    idleMusic.UnPause();
                    waveUpdate = false;
                    waveSpawned = false;
                    wave++;
                    rest = true;
                }
            }

            //wavespawner
            if (!rest && !waveSpawned && !victory)
            {
                switch (wave)
                {
                    case 1:
                        //spawning wave 1
                        Spawnwave(1, 2, 2.5f, 3, 3);
                        Spawngroup(2, 1, 2.5f, 3, -1);
                        break;
                    case 2:
                        //spawning wave 2
                        Spawnwave(2, 4, 2.5f, 3, -1);
                        break;
                    case 3:
                        //spawning wave 3
                        Spawnwave(3, 6, 2.5f, 3, -1);
                        break;
                    case 4:
                        //spawning wave 4
                        Spawnwave(4, 1, 2.5f, 3, -1);
                        Spawngroup(3, 3, 2.5f, 3, -1);
                        break;
                    case 5:
                        //spawning wave 5
                        Spawnwave(4, 2, 2.5f, 3, -1);
                        break;
                    case 6:
                        //spawning wave 6
                        Spawnwave(3, 5, 2.5f, 3, -1);
                        break;
                    case 7:
                        //spawning wave 7
                        Spawnwave(1, 5, 2.5f, 3, -1);
                        Spawngroup(2, 3, 2.5f, 3, -1);
                        break;
                    case 8:
                        //spawning wave 8 
                        Spawnwave(4, 2, 2.5f, 3, -1);
                        Spawngroup(2, 3, 2.5f, 3, -1);
                        break;
                    case 9:
                        //spawning wave 9
                        Spawnwave(3, 2, 2.5f, 3, -1);
                        Spawngroup(2, 2, 2.5f, 3, -1);
                        break;
                    case 10:
                        //spawning wave 10
                        Spawnwave(4, 5, 2.5f, 3, -1);
                        break;
                }
                idleMusic.Pause();
                battleMusic.Play();

            }
        }
        else if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            if (Sleep(Random.Range(2, 4), true, 1))
            {
                Spawngroup(1, 1, 0, 1, 2);
            }
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy")[0].GetComponent<Renderer>().isVisible)
                {
                    if (Sleep(Random.Range(.5f, 1.5f), true, 2))
                    {
                        Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
                    }
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Death")
        {
            //activate wait
            Sleep(10, false, 2);
            //spawning final wave of doom
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Spawngroup(4, 1, 2.5f, 3, i + 1);
                    Spawngroup(1, 10, 2.5f, 3, i + 1);
                    Spawngroup(2, 5, 2.5f, 3, i + 1);
                }
            }
            //wait for timer
            if (ReturnSleep(2))
            {
                //load new level
                loadScene("StartScreen");
                //reset timer
                SLeeprestart(10);
                //unlocking mouse
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //removing altered maganger
                Destroy(gameObject);
            }
        }


        //kill the player if health below zero
        if (wallHealth <= 0)
        {
            //telling code player is dead
            death = true;
            //reseting health
            wallHealth = 100;
            //playing defeat music
            battleMusic.Stop();
            defeatMusic.time = 1;
            defeatMusic.Play();
            //loading death screen
            loadScene("Death");
        }

        //start victory
        if (victory)
        {
            //reseting health
            wallHealth = 100;
            //activate wait
            Sleep(15, false, 3);
            //wait for timer
            if (ReturnSleep(3))
            {
                //load new level
                loadScene("StartScreen");
                //reset timer
                SLeeprestart(10);
                //unlocking mouse
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //removing altered maganger
                Destroy(gameObject);
            }
        }
    }

    //spawn wave
    public void Spawnwave(int enemyIndexType, int spawnNumber_type1, float Type1_distanceMin, float Type1_distanceMax, int spawnpoint)
    {
        //updating wavecounter
        waveMeter.transform.Rotate(0, 0, -9);

        //restarting spawning points
        for (int I = 0; I < spawnPointNumb; I++)
        {
            spawningPoints[I] = new Vector3(10, spawnStartingHight + (I * spawningDistance));
        }

        waveSpawned = true;
        //spawning first group
        Spawngroup(enemyIndexType, spawnNumber_type1, Type1_distanceMin, Type1_distanceMax, spawnpoint);
        waveUpdate = true;
    }

    //spawn enemy's
    public void Spawngroup(int enemyIndexType, int spawnNumber_type1, float Type1_distanceMin, float Type1_distanceMax, int spawnpoint)
    {
        //ajusting enemy type for easy input
        enemyIndexType--;

        //spawn type 1 enemy's
        for (int J = 0; J < spawnNumber_type1; J++)
        {
            //ajusting spawning poition
            for (int I = 0; I < spawnPointNumb; I++)
            {
                spawningPoints[I] = new Vector3(spawnPointX + (J * Random.Range(Type1_distanceMin + 0.5f, Type1_distanceMax)), spawnStartingHight + (I * spawningDistance));
            }
            if (spawnpoint > 3 || spawnpoint < 0)
            {
                //spawning type 1
                Instantiate(enemyTypes[enemyIndexType], spawningPoints[Random.Range(0, 3)], Quaternion.identity);
            }
            else
            {
                //spawning type 1
                Instantiate(enemyTypes[enemyIndexType], spawningPoints[spawnpoint - 1], Quaternion.identity);
            }
        }
    }

    ////menu Interactions

    //StartScreen
    public void StartGame()
    {
        loadScene("Level");
        updateScene = true;
    }

    //starts next wave
    public virtual void Startnextwave()
    {
        rest = false;
    }

    //spawn victory condition
    public IEnumerator SpawnArmy()
    {
        victoryText.text = "the Army has arived,\nthis battle is won";
        victoryText.gameObject.SetActive(true);
        Instantiate(backUp, new Vector3(-12, 1.5f, 0), Quaternion.identity);
        Instantiate(backUp, new Vector3(-12, 6.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        victoryText.gameObject.SetActive(false);
        Instantiate(backUp, new Vector3(-12, 1.5f, 0), Quaternion.identity);
        Instantiate(backUp, new Vector3(-12, 6.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        victoryText.text = "let's hope the other barrier's\nlast as long";
        victoryText.gameObject.SetActive(true);
        for (int i = 0; i < 9; i++)
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(backUp, new Vector3(-12, 1.5f, 0), Quaternion.identity);
            Instantiate(backUp, new Vector3(-12, 6.5f, 0), Quaternion.identity);
        }
    }
}

