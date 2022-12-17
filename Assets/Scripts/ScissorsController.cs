using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsController : MonoBehaviour
{
    bool createNewInstanceOnDestroy = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            createNewInstanceOnDestroy = true;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (createNewInstanceOnDestroy)
        {
            GameManager.instance.UpdateScissorsCount(-1);
            GameManager.instance.UpdateRockCount(1);
            GameManager.instance.InstantiateRockPrefab(transform.position);
            createNewInstanceOnDestroy = false;
        }
    }
}
