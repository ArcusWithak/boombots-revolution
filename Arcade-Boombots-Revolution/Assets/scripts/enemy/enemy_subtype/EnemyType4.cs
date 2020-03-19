using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType4 : MainEnemy
{
    //declaration
    [Space(14)]
    private int wealthMin, WealthMax;
    private bool shieldup;

    private GameObject upperArm;
    private GameObject underArm;

    private float animationTime;

    // Start is called before the first frame update
    void Start()
    {
        //asinging wealth
        wealthMin = 25;
        WealthMax = 40;
        //generating random wealth
        wealth = GenerateCurrecy(wealthMin, WealthMax);

        //assign shield
        underArm = transform.GetChild(1).gameObject;
        upperArm = transform.GetChild(2).gameObject;
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
            //change animation to move
            base.Movement();
        }
    }

    public override void OnTriggerEnter2D(Collider2D Other)
    {
        if (shieldup)
        {
            base.OnTriggerEnter2D(Other);
        }
        else if (Other.CompareTag("SniperBullet"))
        {
            base.OnTriggerEnter2D(Other);
        }
        else if (Other.CompareTag("Bullet"))
        {
            Destroy(Other.gameObject);
        }
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

            //shield up
            if (attack_Delay < (attack_Speed - attack_Speed / 5f))
            {
                if (!shieldup)
                {
                    animationTime = 0;
                }
                //change animation to attack
                upperArm.transform.rotation = Quaternion.Slerp(upperArm.transform.rotation, Quaternion.Euler(0, 0, -61.451f), animationTime * 0.5f);
                underArm.transform.rotation = Quaternion.Slerp(underArm.transform.rotation, Quaternion.Euler(0, 0, 5.087f), animationTime * 0.5f);
                shieldup = true;
            }
            else
            {
                if (shieldup)
                {
                    animationTime = 0;
                }
                //change animation to attack
                upperArm.transform.rotation = Quaternion.Slerp(upperArm.transform.rotation, Quaternion.Euler(0, 0, 6.053f), animationTime * 25);
                underArm.transform.rotation = Quaternion.Slerp(underArm.transform.rotation, Quaternion.Euler(0, 0, -3.189f), animationTime * 25);
                shieldup = false;
            }
            animationTime += Time.deltaTime;
        }
        return 0;
    }
}
