using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const int GAME_SCENE_BUILD_INDEX = 2;

    public static GameManager instance;

    public GameSceneUIManager gameSceneUIManager;

    public GameObject sessionObjects;

    private Dictionary<GameDataScriptableObject.ObjectType, Dictionary<int, GameObject>> nonActiveObjectsPool = new Dictionary<GameDataScriptableObject.ObjectType, Dictionary<int, GameObject>>();

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
        Camera.main.orthographicSize = GameData.GameDataScriptableObject.orthograpicCameraSize;
        GameData.GameDataScriptableObject.UpdateGameViewBoundaries();
        CreatePickList();
        PopulateGameScene();
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void LateUpdate()
    {
        if (GameData.GameDataScriptableObject.rockCount == 0 && GameData.GameDataScriptableObject.paperCount == 0)
        {
            gameSceneUIManager.OnGameOver(GameDataScriptableObject.ObjectType.Scissors);
        }
        else if (GameData.GameDataScriptableObject.paperCount == 0 && GameData.GameDataScriptableObject.scissorsCount == 0)
        {
            gameSceneUIManager.OnGameOver(GameDataScriptableObject.ObjectType.Rock);
        }
        else if (GameData.GameDataScriptableObject.scissorsCount == 0 && GameData.GameDataScriptableObject.rockCount == 0)
        {
            gameSceneUIManager.OnGameOver(GameDataScriptableObject.ObjectType.Paper);
        }
    }

    public void PopulateGameScene()
    {
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
        float minDistance = 1f;

        Collider2D[] neighbours;
        Vector2 pos;

        do
        {
            pos = new Vector2(
                Random.Range(-1 * (GameData.GameDataScriptableObject.GameViewBoundaries.x - 1), (GameData.GameDataScriptableObject.GameViewBoundaries.x - 1)),
                Random.Range(-1 * (GameData.GameDataScriptableObject.GameViewBoundaries.y - 1), (GameData.GameDataScriptableObject.GameViewBoundaries.y - 1)));
            neighbours = Physics2D.OverlapCircleAll(pos, minDistance);
        } while (neighbours.Length > 0);

        return new Vector3(pos.x, pos.y, 0f);
    }

    // Generates uniformly distributed random numbers for each object that add up to the totalObjectCount.
    // TODO generates 0 values
    private void AssignObjectCountsEach()
    {
        List<int> fields = new List<int> { 0, 0, 0 };
        int sum = 0;
        for (int i = 0; i < fields.Count - 1; i++)
        {
            fields[i] = Random.Range(1, GameData.GameDataScriptableObject.totalObjectCount);
            sum += fields[i];
        }
        int actualSum = sum * fields.Count / (fields.Count - 1);
        sum = 0;
        for (int i = 0; i < fields.Count - 1; i++)
        {
            fields[i] = fields[i] * GameData.GameDataScriptableObject.totalObjectCount / actualSum;
            sum += fields[i];
        }
        fields[fields.Count - 1] = GameData.GameDataScriptableObject.totalObjectCount - sum;

        fields.Shuffle();

        GameData.GameDataScriptableObject.rockCount = fields[0];
        GameData.GameDataScriptableObject.paperCount = fields[1];
        GameData.GameDataScriptableObject.scissorsCount = fields[2];
    }

    private void PopulatePickListSorted()
    {
        for (int i = 0; i < GameData.GameDataScriptableObject.rockCount; i++)
        {
            pickList.Add(0);
        }

        for (int i = 0; i < GameData.GameDataScriptableObject.paperCount; i++)
        {
            pickList.Add(1);
        }

        for (int i = 0; i < GameData.GameDataScriptableObject.scissorsCount; i++)
        {
            pickList.Add(2);
        }
    }

    public void InstantiateRockPrefab(Vector3 pos)
    {
        GameObject nonActiveRockObject;
        if (TryGetNonActiveGameObject(GameDataScriptableObject.ObjectType.Rock, out nonActiveRockObject))
        {
            nonActiveRockObject.SetActive(true);
            nonActiveRockObject.transform.position = pos;
        }
        else
        {
            Instantiate(GameData.GameDataScriptableObject.rockPrefab, pos, Quaternion.identity, sessionObjects.transform);
        }
    }

    public void InstantiatePaperPrefab(Vector3 pos)
    {
        GameObject nonActivePaperObject;
        if (TryGetNonActiveGameObject(GameDataScriptableObject.ObjectType.Paper, out nonActivePaperObject))
        {
            nonActivePaperObject.SetActive(true);
            nonActivePaperObject.transform.position = pos;
        }
        else
        {
            Instantiate(GameData.GameDataScriptableObject.paperPrefab, pos, Quaternion.identity, sessionObjects.transform);
        }
    }

    public void InstantiateScissorsPrefab(Vector3 pos)
    {
        GameObject nonActiveScissorsObject;
        if (TryGetNonActiveGameObject(GameDataScriptableObject.ObjectType.Scissors, out nonActiveScissorsObject))
        {
            nonActiveScissorsObject.SetActive(true);
            nonActiveScissorsObject.transform.position = pos;
        }
        else
        {
            Instantiate(GameData.GameDataScriptableObject.scissorsPrefab, pos, Quaternion.identity, sessionObjects.transform);
        }
    }

    public void AddNonActiveGameObject(GameDataScriptableObject.ObjectType objectType, GameObject _gameObject)
    {
        if (!nonActiveObjectsPool.ContainsKey(objectType))
        {
            nonActiveObjectsPool.Add(objectType, new Dictionary<int, GameObject>());
        }
        if (!nonActiveObjectsPool[objectType].ContainsKey(_gameObject.GetInstanceID()))
        {
            nonActiveObjectsPool[objectType].Add(_gameObject.GetInstanceID(), _gameObject);
        }
    }

    private bool TryGetNonActiveGameObject(GameDataScriptableObject.ObjectType type, out GameObject nonActiveGameObject)
    {
        nonActiveGameObject = null;

        if (!nonActiveObjectsPool.ContainsKey(type)) return false;
        if (nonActiveObjectsPool[type].Count == 0) return false;

        var pair = nonActiveObjectsPool[type].First();

        nonActiveGameObject = pair.Value;
        nonActiveObjectsPool[type].Remove(pair.Key);

        return true;
    }
}
