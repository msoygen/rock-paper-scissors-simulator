using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Movement : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 gameViewBoundaries;

    [SerializeField]
    private Rigidbody2D rb2D;

    private void Start()
    {
        direction = new Vector3(GetRandomDirection1D(), GetRandomDirection1D());
        gameViewBoundaries = GameData.GameDataScriptableObject.GameViewBoundaries;
    }

    private void FixedUpdate()
    {
        // keep object in bounds
        if (rb2D.position.x > gameViewBoundaries.x || rb2D.position.x < -gameViewBoundaries.x)
        {
            direction.x *= -1;
            direction.y = GetRandomDirection1D();
        }
        else if (rb2D.position.y > gameViewBoundaries.y || rb2D.position.y < -gameViewBoundaries.y)
        {
            direction.y *= -1;
            direction.x = GetRandomDirection1D();
        }

        rb2D.MovePosition(rb2D.position + direction * GameData.GameDataScriptableObject.objectSpeed * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        // bounce back on re-instantiate from pool
        direction = new Vector3(GetRandomDirection1D(), GetRandomDirection1D());
    }

    private float GetRandomDirection1D()
    {
        return Random.Range(0.5f, 1f) * (Random.Range(0, 2) * 2 - 1); 
    }
}
