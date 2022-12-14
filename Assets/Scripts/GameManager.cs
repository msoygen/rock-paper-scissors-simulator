using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const int GAME_SCENE_BUILD_INDEX = 1;

    public static GameManager instance;

    public GameObject rockPrefab;
    public GameObject paperPrefab;
    public GameObject scissorsPrefab;

    public int totalObjectCount = 20;

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
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == GAME_SCENE_BUILD_INDEX)
        {
            PopulateGameScene();
        }
    }

    private void PopulateGameScene()
    {
        CreatePickList();
    }

    private void CreatePickList()
    {
        pickList.Clear();

        AssignObjectCountsEach();

        pickList.Clear();

        PopulatePickListSorted();

        pickList.Shuffle();

        Debug.Log("zort");
    }

    private void AssignObjectCountsEach()
    {
        pickList.Add(0); // rock
        pickList.Add(1); // paper
        pickList.Add(2); // scissors

        pickList.Shuffle();

        for (int i = 0; i < pickList.Count; i++)
        {
            if (totalObjectCount > 0)
            {
                switch (pickList[i])
                {
                    case 0: // rock
                        AssignObjectCount(ref rockObjectCount);
                        break;
                    case 1: // paper
                        AssignObjectCount(ref paperObjectCount);
                        break;
                    case 2: // scissors
                        AssignObjectCount(ref scissorsObjectCount);
                        break;
                    default:
                        break;
                }
            }

            if (i == pickList.Count - 1 && totalObjectCount > 0)
            {
                switch (pickList[i])
                {
                    case 0: // rock
                        rockObjectCount += totalObjectCount;
                        break;
                    case 1: // paper
                        paperObjectCount += totalObjectCount;
                        break;
                    case 2: // scissors
                        scissorsObjectCount += totalObjectCount;
                        break;
                    default:
                        break;
                }
            }
        }
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

    private void AssignObjectCount(ref int count)
    {
        count = Random.Range(1, totalObjectCount + 1);
        totalObjectCount -= count;
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
