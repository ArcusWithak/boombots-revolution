using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : MainEnemy
{
    //declaration
    [Space(14)]
    private int wealthMin, WealthMax;
    private bool attacking;
    private float attacking_Delay;
    [Space(15)]
    public GameObject muzzelFlash;

    private Coroutine attackAnimation;
    private bool playAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //asinging wealth
        wealthMin = 10;
        WealthMax = 15;
        //generating random wealth
        wealth = GenerateCurrecy(wealthMin, WealthMax);
    }

    // Update is called once per frame
    void Update()
    {
        //checking if dead
        base.Death();
        //movement
        if (moving)
        {
            //control animation
            if (playAnimation)
            {
                playAnimation = false;
                StopAllCoroutines();
            }
            //moving
            base.Movement();
        }
        else if (attacking_Delay <= 0)
        {
            moving = true;
            attacking = false;
        }
        else if (attacking)
        {
            attacking_Delay -= Time.deltaTime;
        }
        if (!attacking)
        {
            //run attack
            Attack();
        }
        else
        {
            //change animation to attack
            if (!playAnimation)
            {
                playAnimation = true;
                attackAnimation = StartCoroutine(AttackAnimation());
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D Other)
    {
        base.OnTriggerEnter2D(Other);
    }

    public override float Attack()
    {
        float distanceToWall = 0;
        if (attack_Delay <= 0)
        {
            attacking = true;
            moving = false;
            attacking_Delay = 2;
        }
        distanceToWall = base.Attack();
        if(distanceToWall < attack_Range - (attack_Range / 1.5f))
        {
            moving = false;
        }
        return distanceToWall;
    }

    public IEnumerator AttackAnimation()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            muzzelFlash.SetActive(true);
            yield return new WaitForSeconds(0.025f);
            muzzelFlash.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
