using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsController : MonoBehaviour
{
    private bool anyContactPointTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rock") && !anyContactPointTriggered)
        {
            gameObject.SetActive(false);
            GameData.GameDataScriptableObject.UpdateScissorsCount(-1);
            GameManager.instance.AddNonActiveGameObject(GameDataScriptableObject.ObjectType.Scissors, gameObject);

            GameData.GameDataScriptableObject.UpdateRockCount(1);
            GameManager.instance.InstantiateRockPrefab(transform.position);

            anyContactPointTriggered = true;
        }
    }

    private void OnEnable()
    {
        anyContactPointTriggered = false;
    }
}
