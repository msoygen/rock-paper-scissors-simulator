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
            GameData.GameDataScriptableObject.UpdateScissorsCount(-1);
            GameData.GameDataScriptableObject.UpdateRockCount(1);
            GameManager.instance.InstantiateRockPrefab(transform.position);
            createNewInstanceOnDestroy = false;
        }
    }
}
