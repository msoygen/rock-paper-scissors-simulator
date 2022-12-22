using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{
    private bool anyContactPointTriggered = false;

    private void Awake()
    {
        anyContactPointTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Scissors") && !anyContactPointTriggered)
        {
            gameObject.SetActive(false);
            GameData.GameDataScriptableObject.UpdatePaperCount(-1);
            GameManager.instance.AddNonActiveGameObjectToThePool(GameDataScriptableObject.ObjectType.Paper, gameObject);

            GameData.GameDataScriptableObject.UpdateScissorsCount(1);
            GameManager.instance.InstantiateScissorsPrefab(transform.position);

            anyContactPointTriggered = true;
            GameManager.instance.PlayScissorsSFX();
        }
    }

    private void OnEnable()
    {
        anyContactPointTriggered = false;
    }
}
