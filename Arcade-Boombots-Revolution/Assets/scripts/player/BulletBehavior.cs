using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, 0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-velocity * Time.deltaTime, 0, 0);
    }
}
