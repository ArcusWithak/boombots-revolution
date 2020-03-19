using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainEnemy : MonoBehaviour
{
    private Gamemanager gamemanagerScript;
    private player_controller playerScript;
    public SpriteRenderer spriteRenderer;

    private GameObject Wall;

    [Space(10)]
    [Range(0,20)]
    public float healthTotal;
    public float health;

    [Space(11)]
    [Range(0, 25)]
    public float attack_Dmg;

    [Space(12)]
    [Range(0, 10)]
    public float attack_Speed;

    [Space(13)]
    [Range(0, 100)]
    public float attack_Range;

    [Space(14)]
    [Range(0, 10)]
    public float movementSpeed;
    public bool moving;

    [Space(15)]
    public GameObject healthBar;
    private float healthBarMaxSize;

    [Space(16)]
    public float attack_Delay;

    [Space(17)]
    public int wealth;

    // Start is called before the first frame update
    void Awake()
    {
        if(SceneManager.GetActiveScene().name != "Death")
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller>();
            gamemanagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager>();
        }
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        Wall = GameObject.FindGameObjectWithTag("Wall");
        //asining healthbar
        healthBar = transform.GetChild(0).gameObject;
        healthBarMaxSize = healthBar.transform.localScale.x;
        //asinging health
        health = healthTotal;
        //assining attack_delay
        attack_Delay = attack_Speed;
    }

    //movement
    public virtual void Movement()
    {
        transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
    }

    //taking enemy damage
    public virtual void TakeDamage()
    {
        //flashhit
        spriteRenderer.color = Color.red;
        StartCoroutine(HitEnemyFlash());
        //reduce health
        if(playerScript != null)
        {
            health -= playerScript.currentGunDamage;
        }
        else
        {
            health -= 5;
        }

        //adjust healthbar
        if(healthBar != null)
        {
            healthBar.transform.localScale = new Vector3(healthBarMaxSize / healthTotal * health, healthBar.transform.localScale.y, 1);
        }
    }

    //enemy death
    public void Death()
    {
        //check if target is dead
        if (health <= 0)
        {
            //add gained currency
            gamemanagerScript.currency += wealth;
            //remove target
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Bullet") || Other.CompareTag("SniperBullet"))
        {
            TakeDamage();
            if (!Other.CompareTag("SniperBullet"))
            {
                Destroy(Other.gameObject);
            }
        }
    }

    //enemy Attacks
    public virtual float Attack()
    {
        //adjusting wall center for perspective
        float enemyPosition = transform.position.x;
        if(transform.position.y == 6.5f)
        {
            enemyPosition -= 1.5f;
        }
        else if (transform.position.y == 1.5f)
        {
            enemyPosition += 1.5f;
        }
        //declaring distance to wall
        float distanceToWall;
        if (Wall != null)
        {
            //assining distance to wall
            distanceToWall = enemyPosition - Wall.transform.position.x;
        }
        else
        {
            //assining distance to wall if no wall is avalible
            distanceToWall = 10000000;
        }
        //check if enemy is within attack range
        if (distanceToWall < attack_Range)
        {
            //checking if delay is 0
            if (attack_Delay <= 0)
            {
                //reseting delay time
                attack_Delay = attack_Speed;
                //damaging wall
                gamemanagerScript.wallHealth -= attack_Dmg;
                Wall.GetComponent<SpriteRenderer>().color = Color.red;
                StartCoroutine(HitWallFlash());
            }
            else
            {
                //removing time for next attack
                attack_Delay -= Time.deltaTime;
            }
        }
        else
        {
            //reseting time to attack
            if (attack_Delay != attack_Speed)
            {
                attack_Delay = attack_Speed;
            }
        }
        //feeding distance to enemy
        return distanceToWall;
    }

    //generate currency
    public int GenerateCurrecy(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    //flash on enemy hit
    public IEnumerator HitEnemyFlash()
    {
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    //flash on wall hit
    public IEnumerator HitWallFlash()
    {
        yield return new WaitForSeconds(.1f);
        Wall.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
