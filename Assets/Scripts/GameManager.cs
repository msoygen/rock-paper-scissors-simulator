using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject rockPrefab;
    public GameObject paperPrefab;
    public GameObject scissorsPrefab;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void InstantiateRockPrefab(Transform transform)
    {
        Instantiate(rockPrefab, transform.position, transform.rotation);
    }

    public void InstantiatePaperPrefab(Transform transform)
    {
        Instantiate(paperPrefab, transform.position, transform.rotation);
    }

    public void InstantiateScissorsPrefab(Transform transform)
    {
        Instantiate(scissorsPrefab, transform.position, transform.rotation);
    }
}
