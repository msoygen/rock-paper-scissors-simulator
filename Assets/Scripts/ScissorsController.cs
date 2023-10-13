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
            GameManager.instance.AddNonActiveGameObjectToThePool(GameDataScriptableObject.ObjectType.Scissors, gameObject);

            GameData.GameDataScriptableObject.UpdateRockCount(1);
            GameManager.instance.InstantiatePrefabByType(transform.position, GameDataScriptableObject.ObjectType.Rock);

            anyContactPointTriggered = true;
            GameManager.instance.PlayRockSFX();
        }
    }

    private void OnEnable()
    {
        anyContactPointTriggered = false;
    }
}
