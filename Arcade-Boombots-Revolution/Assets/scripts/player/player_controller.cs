using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_controller : shortcuts
{
    [Space(10)]
    private Gamemanager gameManagerScript;

    public GameObject armJoint;
    [Space(10)]
    public Transform gunBarrel;
    public GameObject[] bullet;
    public GameObject[] gunPrefebs;

    private float fireDelay;
    private float ammocount;
    private float reloadDelay;
    private bool outOfAmmo;

    private float gunAjustment;

    private GameObject spawnedBullet;


    [Space(14)]
    public float currentFireRate;
    public float currentTotalAmmoCount;
    public float currentReloadTime;
    public float currentGunDamage;
    public float currentGunAccuracy;

    public enum GunTypes
    {
        pistol = 0,
        ar = 1,
        shotgun = 2,
        sniper = 3,
        lmg = 4
    }

    [Space(16)]
    public GunTypes equipedGun, NewGun;
    public int ShotgunBulletAmount;
    //gunstats
    public struct GunStats
    {
        public float Dmg;
        public float FireRate;
        public float ReloadTime;
        public float TotalAmmoCount;
        public float Accuracy;
        public float currentAmmo;
    }

    [Space(17)]
    //declaring gunstats
    public GunStats Pistol, Ar, Shotgun, Sniper, Lmg;

    [Space(18)]
    public Text ammoText;
    public Image ammoUiImage;

    private GameObject currentGun;
    private Vector3 currentGunPos;
    private Quaternion currentGunRotation;
    private int currentGunIndex;
    private float switchCooldown;

    [Space(19)]
    public GameObject[] bodyParts;

    [Space(20)]
    public Sprite[] possibleParts;

    // Start is called before the first frame update
    void Start()
    {
        //asinging gamemanager
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager>();

        ////asinging gun stats
        {
            //pistol
            Pistol.Dmg = 5;
            Pistol.FireRate = 1;
            Pistol.ReloadTime = 2;
            Pistol.TotalAmmoCount = 6;
            Pistol.Accuracy = 0;
            Pistol.currentAmmo = Pistol.TotalAmmoCount;

            //ar
            Ar.Dmg = 5;
            Ar.FireRate = 0.5f;
            Ar.ReloadTime = 4;
            Ar.TotalAmmoCount = 16;
            Ar.Accuracy = 1;
            Ar.currentAmmo = Ar.TotalAmmoCount;

            //shotgun
            Shotgun.Dmg = 2.5f;
            Shotgun.FireRate = 2;
            Shotgun.ReloadTime = 4;
            Shotgun.TotalAmmoCount = 4;
            Shotgun.Accuracy = 25;
            Shotgun.currentAmmo = Shotgun.TotalAmmoCount;

            //sniper
            Sniper.Dmg = 10;
            Sniper.FireRate = 2;
            Sniper.ReloadTime = 5;
            Sniper.TotalAmmoCount = 6;
            Sniper.Accuracy = 0;
            Sniper.currentAmmo = Sniper.TotalAmmoCount;

            //lmg
            Lmg.Dmg = 1f;
            Lmg.FireRate = 0.1f;
            Lmg.ReloadTime = 5;
            Lmg.TotalAmmoCount = 100;
            Lmg.Accuracy = 20;
            Lmg.currentAmmo = Lmg.TotalAmmoCount;

            //asinging player stats
            currentFireRate = Pistol.FireRate;
            currentTotalAmmoCount = Pistol.TotalAmmoCount;
            currentReloadTime = Pistol.ReloadTime;
            currentGunDamage = Pistol.Dmg;
            currentGunAccuracy = Pistol.Accuracy;

            //assigning gun positions and rotation
            currentGun = transform.GetChild(0).GetChild(0).gameObject;
            currentGunPos = currentGun.transform.position;
            currentGunRotation = currentGun.transform.rotation;
            gunBarrel = currentGun.transform.GetChild(0).GetChild(0);

            //assiging gun index
            currentGunIndex = (int)equipedGun;
        }

        //resetting gun position
        armJoint.transform.eulerAngles = new Vector3(0, 0, 0);

        //assining gunAmmo
        ammocount = currentTotalAmmoCount;

        //update ui text
        updatetext(ammoText, ammocount.ToString());
        ammoUiImage.fillAmount = 1;

        //assigning bodyparts sprites
        bodyParts[0].GetComponent<SpriteRenderer>().sprite = possibleParts[0];
        bodyParts[1].GetComponent<SpriteRenderer>().sprite = possibleParts[1];
        bodyParts[2].GetComponent<SpriteRenderer>().sprite = possibleParts[2];
    }

    // Update is called once per frame
    void Update()
    {
        //no actions during menuscreen
        if (!gameManagerScript.rest && !gameManagerScript.death)
        {
            //godmode code
            if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.E))
            {
                gameManagerScript.currency = 1000000000000;
                gameManagerScript.wallHealth = 1000000000000;
                if (Input.GetKey(KeyCode.Space))
                {
                    GameObject[] enemycount = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < enemycount.Length; i++)
                    {
                        Destroy(enemycount[i]);
                    }
                }
                if (Input.GetKey(KeyCode.R))
                {
                    gameManagerScript.wallHealth = -10;
                }
            }

            //counting down reload/firerate
            if (fireDelay > 0)
            {
                fireDelay -= Time.deltaTime;
            }

            if (reloadDelay > 0)
            {
                reloadDelay -= Time.deltaTime;
                resourcebarFill(reloadDelay, currentReloadTime, ammoUiImage);
            }

            if (switchCooldown > 0)
            {
                switchCooldown -= Time.deltaTime;
            }

            //fire gun if ammo is avalible
            if (Input.GetKey(KeyCode.R) && fireDelay <= 0 && ammocount > 0 || Input.GetKey(KeyCode.Alpha4) && fireDelay <= 0 && ammocount > 0)
            {
                if (equipedGun == GunTypes.shotgun)
                {
                    //spawning multiple bullets for the shotgun
                    for (int I = 0; I < ShotgunBulletAmount; I++)
                    {
                        spawnedBullet = Instantiate(bullet[(int)equipedGun], gunBarrel.position, armJoint.transform.rotation);
                        //changing bullet direction based on accuracy
                        spawnedBullet.transform.Rotate(0, 0, Random.Range(-currentGunAccuracy, currentGunAccuracy));
                    }
                }
                else
                {
                    //spawning bullet
                    spawnedBullet = Instantiate(bullet[(int)equipedGun], gunBarrel.position, armJoint.transform.rotation);
                    //changing bullet direction based on accuracy
                    spawnedBullet.transform.Rotate(0, 0, Random.Range(-currentGunAccuracy, currentGunAccuracy));
                }
                //adding delay before you can fire again
                fireDelay = currentFireRate;

                //removing ammo
                ammocount--;

                //update ui text
                updatetext(ammoText, ammocount.ToString());
                resourcebarEmpty(ammocount, currentTotalAmmoCount, ammoUiImage);
            }

            ////reloading
            {

                // *Lore*
                //// all weapons are loaded with blue cum ////
                /// * this is a joke * ///
                
                //active reload
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Alpha7))
                {
                    ammocount = 0;
                    //update ui text
                    updatetext(ammoText, "");
                    ammoUiImage.fillAmount = 0;
                }

                //checking if gun has been reloaded
                if ((outOfAmmo) && reloadDelay <= 0)
                {
                    ammocount = currentTotalAmmoCount;
                    outOfAmmo = false;
                    //update ui text
                    updatetext(ammoText, ammocount.ToString());
                    ammoUiImage.fillAmount = 1;
                }

                //checking their is ammo left
                else if (ammocount <= 0 && reloadDelay <= 0 && (!outOfAmmo))
                {
                    reloadDelay = currentReloadTime;
                    outOfAmmo = true;
                    //update ui text
                    updatetext(ammoText, "");
                    ammoUiImage.fillAmount = 0;
                }
            }

            ////gunrotation
            {
                //changing gun rotation
                gunAjustment += Input.GetAxis("Vertical");

                //ajusting gun rotation/keeping it within boundry's

                if ((int)equipedGun != 0)
                {
                    armJoint.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Clamp(gunAjustment, 0, 100));
                }
                else
                {
                    armJoint.transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Clamp(gunAjustment, 0, 180));
                }
            }

            ////weapons switching
            {
                if (Input.GetKey(KeyCode.LeftShift) && switchCooldown <= 0 || Input.GetKey(KeyCode.Alpha8) && switchCooldown <= 0)
                {
                    reloadDelay = 0;
                    outOfAmmo = false;
                    currentGunIndex++;
                    if (currentGunIndex > gameManagerScript.maxGunUnlocked)
                    {
                        currentGunIndex = 0;
                    }
                    ChangeCurrentGunType();
                    switchCooldown = 0.25f;
                }
                else if (Input.GetKey(KeyCode.LeftControl) && switchCooldown <= 0 || Input.GetKey(KeyCode.Alpha5) && switchCooldown <= 0)
                {
                    reloadDelay = 0;
                    outOfAmmo = false;
                    currentGunIndex--;
                    if (currentGunIndex < 0)
                    {
                        currentGunIndex = gameManagerScript.maxGunUnlocked;
                    }
                    ChangeCurrentGunType();
                    switchCooldown = 0.25f;
                }

                if (NewGun != equipedGun)
                {
                    switch ((int)NewGun)
                    {
                        case 0:
                            ChangeGun(Pistol);
                            break;
                        case 1:
                            ChangeGun(Ar);
                            break;
                        case 2:
                            ChangeGun(Shotgun);
                            break;
                        case 3:
                            ChangeGun(Sniper);
                            break;
                        case 4:
                            ChangeGun(Lmg);
                            break;
                    }
                }
            }
        }
    }
    //assigning new gun
    public void ChangeCurrentGunType()
    {
        switch ((int)equipedGun)
        {
            case 0:
                Pistol.currentAmmo = ammocount;
                break;
            case 1:
                Ar.currentAmmo = ammocount;
                break;
            case 2:
                Shotgun.currentAmmo = ammocount;
                break;
            case 3:
                Sniper.currentAmmo = ammocount;
                break;
            case 4:
                Lmg.currentAmmo = ammocount;
                break;
        }
        switch (currentGunIndex)
        {
            case 0:
                NewGun = GunTypes.pistol;
                break;
            case 1:
                NewGun = GunTypes.ar;
                break;
            case 2:
                NewGun = GunTypes.shotgun;
                break;
            case 3:
                NewGun = GunTypes.sniper;
                break;
            case 4:
                NewGun = GunTypes.lmg;
                break;
        }
    }

    //changing gunstats
    public void ChangeGun(GunStats Gun)
    {
        //ajust states
        equipedGun = NewGun;

        currentGunDamage = Gun.Dmg;
        currentFireRate = Gun.FireRate;
        currentReloadTime = Gun.ReloadTime;
        currentTotalAmmoCount = Gun.TotalAmmoCount;
        currentGunAccuracy = Gun.Accuracy;
        fireDelay = 0;
        reloadDelay = 0;
        ammocount = Gun.currentAmmo;

        //update ui text
        updatetext(ammoText, ammocount.ToString());
        ammoUiImage.fillAmount = 1;

        //spawning gun
        Destroy(currentGun);
        armJoint.transform.localEulerAngles = new Vector3(0, 0, 90);
        currentGun = Instantiate(gunPrefebs[currentGunIndex], currentGunPos, currentGunRotation, armJoint.transform);
        currentGunPos = currentGun.transform.position;
        currentGunRotation = currentGun.transform.rotation;
        gunBarrel = currentGun.transform.GetChild(0).GetChild(0);
    }

    //changes armor if health Threashold 
    public void ChangeArmor()
    {
        if (150 <= gameManagerScript.wallHeathMax)
        {
            bodyParts[0].GetComponent<SpriteRenderer>().sprite = possibleParts[3];
            bodyParts[1].GetComponent<SpriteRenderer>().sprite = possibleParts[4];
            bodyParts[2].GetComponent<SpriteRenderer>().sprite = possibleParts[5];
        }
        if (200 <= gameManagerScript.wallHeathMax)
        {
            bodyParts[0].GetComponent<SpriteRenderer>().sprite = possibleParts[6];
            bodyParts[1].GetComponent<SpriteRenderer>().sprite = possibleParts[7];
            bodyParts[2].GetComponent<SpriteRenderer>().sprite = possibleParts[8];
        }
    }

}
