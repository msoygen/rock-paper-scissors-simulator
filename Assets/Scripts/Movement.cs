using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Movement : MonoBehaviour
{
    private Vector3 direction;
    private Vector2 gameViewBoundaries;

    [SerializeField]
    private Rigidbody2D rb2D;

    private void Start()
    {
        direction = new Vector3(GetRandomDirection1D(), GetRandomDirection1D());
        gameViewBoundaries = GameData.GameDataScriptableObject.GameViewBoundaries;
    }

    private void Update()
    {
        CheckBounds();
        transform.position += direction * GameData.GameDataScriptableObject.objectSpeed * Time.deltaTime;
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

    private void CheckBounds()
    {
        if (transform.position.x > gameViewBoundaries.x)
        {
            if (direction.x < 0f) return;
            direction.x *= -1;
            direction.y = GetRandomDirection1D();
        }
        else if (transform.position.x < -gameViewBoundaries.x)
        {
            if (direction.x > 0f) return;
            direction.x *= -1;
            direction.y = GetRandomDirection1D();
        }
        else if (transform.position.y > gameViewBoundaries.y)
        {
            if (direction.y < 0f) return;
            direction.y *= -1;
            direction.x = GetRandomDirection1D();
        }
        else if (transform.position.y < -gameViewBoundaries.y)
        {
            if (direction.y > 0f) return;
            direction.y *= -1;
            direction.x = GetRandomDirection1D();
        }
    }
}
