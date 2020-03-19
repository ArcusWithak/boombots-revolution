using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : MainEnemy
{
    //declaration
    [Space(14)]
    private int wealthMin, WealthMax;
    private bool dash;
    public float dashTimerMax, dashTimerMin;
    private float dashcooldown;

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
        //attack
        Attack();
        //movement
        if (moving)
        {
            //move
            base.Movement();

            //type 3 special abillty
            if(transform.position.x > -2.578175f)
            {
                if (dash)
                {
                    float newY = Random.Range(0, 3) * 2.5f + 1.5f;
                    transform.position = new Vector3(transform.position.x, newY);
                    dashcooldown = Random.Range(dashTimerMin, dashTimerMax);
                    dash = false;
                }
                else if (dashcooldown > 0)
                {
                    dashcooldown -= Time.deltaTime;
                }
                else if (dashcooldown <= 0)
                {
                    dash = true;
                }
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D Other)
    {
        base.OnTriggerEnter2D(Other);
    }

    public override float Attack()
    {
        //calculate distance to wall
        float distanceToWall;
        distanceToWall = base.Attack();
        //check if within attack Range
        if (distanceToWall < attack_Range)
        {
            //stopping movement
            moving = false;

            //change animation to move
        }
        return 0;
    }
}
