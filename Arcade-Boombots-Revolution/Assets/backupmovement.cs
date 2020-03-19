using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backupmovement : MonoBehaviour
{
    public float movementSpeed;
    public float floatingSpeed;
    public float floatingDistance;
    private float floatingDistanceMinus;

    private Vector3 startingposition;

    // Start is called before the first frame update
    void Start()
    {
        floatingDistanceMinus = transform.position.y - floatingDistance ;
        floatingDistance += transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementSpeed * Time.deltaTime, floatingSpeed * Time.deltaTime, 0);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, floatingDistanceMinus, floatingDistance));
        if (transform.position.y >= floatingDistance || transform.position.y <= floatingDistanceMinus)
        {
            floatingSpeed = -floatingSpeed;
        }
    }
}
