using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : MainEnemy
{
    //declaration
    [Space(14)]
    private int wealthMin, WealthMax;
    [Space(15)]
    public GameObject wielJoint;
    public GameObject arm1Joint;
    public GameObject arm2Joint;
    public GameObject drillJoint;

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
        //moving
        if (moving)
        {
            //change animation to move
            wielJoint.transform.Rotate(0, 0, 5);
            //movement
            base.Movement();
        }
        //attacking
        Attack();
    }

    public override void OnTriggerEnter2D(Collider2D Other)
    {
        base.OnTriggerEnter2D(Other);
        if (Other.CompareTag("Enemy") && Other.transform.position.x < transform.position.x)
        {
            moving = false;
        }
    }

    public void OnTriggerExit2D(Collider2D Other)
    {
        if (!Other.CompareTag("Bullet") && !Other.CompareTag("SniperBullet"))
        {
            moving = true;
        }
    }

    public override float Attack()
    {
        //calculate distance to wall
        float distanceToWall;
        distanceToWall = base.Attack();
        //check if within attack Range
        if(distanceToWall < attack_Range)
        {
            //stopping movement
            moving = false;

            //change animation to move
            if(attack_Delay < (attack_Speed - attack_Speed / 5f))
            {
                arm1Joint.transform.rotation = Quaternion.Slerp(arm1Joint.transform.rotation, Quaternion.Euler(0, 0, -48.155f), Time.deltaTime);
                arm2Joint.transform.rotation = Quaternion.Slerp(arm2Joint.transform.rotation, Quaternion.Euler(0, 0, -39.943f), Time.deltaTime);
                drillJoint.transform.rotation = Quaternion.Slerp(drillJoint.transform.rotation, Quaternion.Euler(0, 0, -15f), Time.deltaTime);
            }
            else
            {
                arm1Joint.transform.rotation = Quaternion.Slerp(arm1Joint.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 20f);
                arm2Joint.transform.rotation = Quaternion.Slerp(arm2Joint.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 20f);
                drillJoint.transform.rotation = Quaternion.Slerp(drillJoint.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 200f);
            }
        }
        return 0;
    }
}
