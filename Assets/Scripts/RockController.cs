using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private bool anyContactPointTriggered = false;

    private void Awake()
    {
        anyContactPointTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Paper") && !anyContactPointTriggered)
        {
            gameObject.SetActive(false);
            GameData.GameDataScriptableObject.UpdateRockCount(-1);
            GameManager.instance.AddNonActiveGameObjectToThePool(GameDataScriptableObject.ObjectType.Rock, gameObject);

            GameData.GameDataScriptableObject.UpdatePaperCount(1);
            GameManager.instance.InstantiatePaperPrefab(transform.position);

            anyContactPointTriggered = true;
            GameManager.instance.PlayPaperSFX();
        }
    }

    private void OnEnable()
    {
        anyContactPointTriggered = false;
    }
}
