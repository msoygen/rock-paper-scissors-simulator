using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Movement : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 velocity;

    private float speed = 3f;
    [SerializeField]
    private Rigidbody2D rb2D;

    private Vector2 gameViewBoundaries;

    private void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        velocity = direction * speed;
    }

    private void FixedUpdate()
    {
        Profiler.BeginSample("Boundaries");
        gameViewBoundaries = GameManager.instance.GetGameViewBoundaries(); // this can be done in only once and cache in game manager instance
        if (rb2D.position.x > gameViewBoundaries.x || rb2D.position.x < -gameViewBoundaries.x)
        {
            direction.x *= -1;
            direction.y = Random.Range(-1f, 1f);
        }
        else if (rb2D.position.y > gameViewBoundaries.y || rb2D.position.y < -gameViewBoundaries.y)
        {
            direction.y *= -1;
            direction.x = Random.Range(-1f, 1f);
        }

        Profiler.EndSample(); 

        Profiler.BeginSample("MovePosition");
        rb2D.MovePosition(rb2D.position + direction * speed * Time.fixedDeltaTime);
        Profiler.EndSample();
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Vertical Bound"))
        {
            direction.y *= -1;
            direction.x = Random.Range(-1f, 1f);
        }
        else if (collision.gameObject.CompareTag("Horizontal Bound"))
        {
            direction.x *= -1;
            direction.y = Random.Range(-1f, 1f);
        }
    }*/
}
