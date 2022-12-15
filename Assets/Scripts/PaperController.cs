using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : MonoBehaviour
{
    bool createNewInstanceOnDestroy = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Scissors"))
        {
            createNewInstanceOnDestroy = true;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (createNewInstanceOnDestroy)
        {
            SessionManager.instance.InstantiateScissorsPrefab(transform.position);
            createNewInstanceOnDestroy = false;
        }
    }
}
