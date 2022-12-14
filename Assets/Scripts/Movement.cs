using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 direction;
    private int speed = 5;

    private void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Vertical Bound"))
        {
            direction.y *= -1;
            direction.x = Random.Range(-1f, 1f);
        }
        else if(collision.gameObject.CompareTag("Horizontal Bound"))
        {
            direction.x *= -1;
            direction.y = Random.Range(-1f, 1f);
        }
    }
}
