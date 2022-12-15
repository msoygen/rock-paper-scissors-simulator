using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    public GameObject rockPrefab;
    public GameObject paperPrefab;
    public GameObject scissorsPrefab;

    public GameObject sessionObjects;

    public int totalObjectCount = 0;
    public float speed = 5f;

    private int rockObjectCount = 0;
    private int paperObjectCount = 0;
    private int scissorsObjectCount = 0;

    // 0 rock 1 paper 2 scissors
    private List<int> pickList = new List<int>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        PopulateGameScene();
    }

    private void PopulateGameScene()
    {
        CreatePickList();
        // instantiate using a scattering formation
        foreach (int pick in pickList)
        {
            switch (pick)
            {
                case 0: // rock
                    InstantiateRockPrefab(FindNonOverlappingPosition());
                    break;
                case 1: // paper
                    InstantiatePaperPrefab(FindNonOverlappingPosition());
                    break;
                case 2: // scissors
                    InstantiateScissorsPrefab(FindNonOverlappingPosition());
                    break;
                default:
                    break;
            }
        }
    }

    private void CreatePickList()
    {
        pickList.Clear();

        AssignObjectCountsEach();

        PopulatePickListSorted();

        pickList.Shuffle();
    }


    private Vector3 FindNonOverlappingPosition()
    {
        Bounds ortographicCameraBounds = Camera.main.OrthographicBounds();

        float ortographicCameraBounds_X_Half = ortographicCameraBounds.size.x / 2;
        ortographicCameraBounds_X_Half -= 1; // margin from object center

        float ortographicCameraBounds_Y_Half = ortographicCameraBounds.size.y / 2;
        ortographicCameraBounds_Y_Half -= 1; // margin from object center

        float minDistance = 1f;

        Collider2D[] neighbours;
        Vector2 pos;

        do
        {
            pos = new Vector2(
                Random.Range(-1 * ortographicCameraBounds_X_Half, ortographicCameraBounds_X_Half),
                Random.Range(-1 * ortographicCameraBounds_Y_Half, ortographicCameraBounds_Y_Half));
            neighbours = Physics2D.OverlapCircleAll(pos, minDistance);
        } while (neighbours.Length > 0);

        return new Vector3(pos.x, pos.y, 0f);
    }

    // Generates uniformly distributed random numbers for each object that add up to the totalObjectCount.
    private void AssignObjectCountsEach()
    {
        List<int> fields = new List<int> { 0, 0, 0 };
        int sum = 0;
        for (int i = 0; i < fields.Count - 1; i++)
        {
            fields[i] = Random.Range(1, totalObjectCount);
            sum += fields[i];
        }
        int actualSum = sum * fields.Count / (fields.Count - 1);
        sum = 0;
        for (int i = 0; i < fields.Count - 1; i++)
        {
            fields[i] = fields[i] * totalObjectCount / actualSum;
            sum += fields[i];
        }
        fields[fields.Count - 1] = totalObjectCount - sum;

        fields.Shuffle();

        rockObjectCount = fields[0];
        paperObjectCount = fields[1];
        scissorsObjectCount = fields[2];
    }

    private void PopulatePickListSorted()
    {
        for (int i = 0; i < rockObjectCount; i++)
        {
            pickList.Add(0);
        }

        for (int i = 0; i < paperObjectCount; i++)
        {
            pickList.Add(1);
        }

        for (int i = 0; i < scissorsObjectCount; i++)
        {
            pickList.Add(2);
        }
    }

    public void InstantiateRockPrefab(Vector3 pos)
    {
        Instantiate(rockPrefab, pos, Quaternion.identity, sessionObjects.transform);
    }

    public void InstantiatePaperPrefab(Vector3 pos)
    {
        Instantiate(paperPrefab, pos, Quaternion.identity, sessionObjects.transform);
    }

    public void InstantiateScissorsPrefab(Vector3 pos)
    {
        Instantiate(scissorsPrefab, pos, Quaternion.identity, sessionObjects.transform);
    }
}