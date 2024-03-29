using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int GAME_SCENE_BUILD_INDEX = 2;

    public static GameManager instance;

    public GameSceneUIManager gameSceneUIManager;

    public GameObject sessionObjects;

    private Dictionary<GameDataScriptableObject.ObjectType, Dictionary<int, GameObject>> nonActiveObjectsPool = new Dictionary<GameDataScriptableObject.ObjectType, Dictionary<int, GameObject>>();
    private List<Vector3> instancePositionsPool = new List<Vector3>();

    private List<GameDataScriptableObject.ObjectType> pickList = new List<GameDataScriptableObject.ObjectType>();

    [SerializeField]
    private AudioSource audioSoruce;

    private int populateLoopIndex = 0;

    private bool _sessionComplete = false;

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
        Application.targetFrameRate = 60;
        Camera.main.orthographicSize = GameData.GameDataScriptableObject.orthograpicCameraSize;
        GameData.GameDataScriptableObject.UpdateGameViewBoundaries();

        populateLoopIndex = 0;
        CreatePickList();

        StartCoroutine(PopulateGameScene()); // Using coroutine for this prevents browser from crashing
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        if (!_sessionComplete) GameOvercheck();

    }

    private void GameOvercheck()
    {
        if (GameData.GameDataScriptableObject.rockCount == 0 && GameData.GameDataScriptableObject.paperCount == 0)
        {
            gameSceneUIManager.OnGameOver(GameDataScriptableObject.ObjectType.Scissors);
            _sessionComplete = true;
        }
        else if (GameData.GameDataScriptableObject.paperCount == 0 && GameData.GameDataScriptableObject.scissorsCount == 0)
        {
            gameSceneUIManager.OnGameOver(GameDataScriptableObject.ObjectType.Rock);
            _sessionComplete = true;
        }
        else if (GameData.GameDataScriptableObject.scissorsCount == 0 && GameData.GameDataScriptableObject.rockCount == 0)
        {
            gameSceneUIManager.OnGameOver(GameDataScriptableObject.ObjectType.Paper);
            _sessionComplete = true;
        }
    }

    /// <summary>
    /// Spreads instantiating pick list throughout frames.
    /// </summary>
    IEnumerator PopulateGameScene()
    {
        // instantiate using a scattering formation
        foreach (GameDataScriptableObject.ObjectType pick in pickList)
        {
            populateLoopIndex++;
            InstantiatePrefabByType(FindNonOverlappingPosition(), pick);

            // for every 50 instantiate operation, take a break
            if (populateLoopIndex % 50 == 0)
            {
                yield return null;
            }
        }
        gameSceneUIManager.OnObjectsInstantiated();
    }

    private Vector3 FindNonOverlappingPosition()
    {
        float minDistance = 1.01f;
        Vector2 pos = new Vector2(0, 0);

        do
        {
            pos.x = Random.Range(-1 * (GameData.GameDataScriptableObject.GameViewBoundaries.x - 1), (GameData.GameDataScriptableObject.GameViewBoundaries.x - 1));
            pos.y = Random.Range(-1 * (GameData.GameDataScriptableObject.GameViewBoundaries.y - 1), (GameData.GameDataScriptableObject.GameViewBoundaries.y - 1));
        } while (InstancePositionsPoolPositionOverlapCheck(pos, minDistance));

        return new Vector3(pos.x, pos.y, 0f);
    }

    /// <summary>
    /// Uses non square root Eucledian distance to determine if given position overlaps with any position from instancePositionPool
    /// </summary>
    /// <returns></returns>
    private bool InstancePositionsPoolPositionOverlapCheck(Vector2 pos, float minDistance)
    {
        foreach (var target_pos in instancePositionsPool)
        {
            if ((target_pos.x - pos.x) * (target_pos.x - pos.x) + (target_pos.y - pos.y) * (target_pos.y - pos.y) < minDistance * minDistance) return true;
        }
        return false;
    }

    private void CreatePickList()
    {
        pickList.Clear();

        AssignObjectCountsEach();
        PopulatePickList();
    }

    /// <summary>
    /// Generates (almost)uniformly distributed integers for each object that add up to the totalObjectCount.
    /// </summary>
    private void AssignObjectCountsEach()
    {
        List<int> fields = new List<int> { 0, 0, 0 };
        int sum = 3;

        for (int i = 0; i < fields.Count - 1; i++)
        {
            if (GameData.GameDataScriptableObject.totalObjectCount - sum > 1)
            {
                fields[i] = Random.Range(1, GameData.GameDataScriptableObject.totalObjectCount - sum);
                sum += fields[i];
            }
        }

        fields[fields.Count - 1] = GameData.GameDataScriptableObject.totalObjectCount - sum;

        // make sure each object is present
        fields[0]++;
        fields[1]++;
        fields[2]++;

        fields.Shuffle();

        GameData.GameDataScriptableObject.rockCount = fields[0];
        GameData.GameDataScriptableObject.paperCount = fields[1];
        GameData.GameDataScriptableObject.scissorsCount = fields[2];
    }

    private void PopulatePickList()
    {
        for (int i = 0; i < GameData.GameDataScriptableObject.rockCount; i++)
        {
            pickList.Add(GameDataScriptableObject.ObjectType.Rock);
        }

        for (int i = 0; i < GameData.GameDataScriptableObject.paperCount; i++)
        {
            pickList.Add(GameDataScriptableObject.ObjectType.Paper);
        }

        for (int i = 0; i < GameData.GameDataScriptableObject.scissorsCount; i++)
        {
            pickList.Add(GameDataScriptableObject.ObjectType.Scissors);
        }

        pickList.Shuffle();
    }

    public void InstantiatePrefabByType(Vector3 pos, GameDataScriptableObject.ObjectType objectType)
    {
        GameObject nonActiveScissorsObject;
        if (TryGetNonActiveGameObjectFromThePool(objectType, out nonActiveScissorsObject))
        {
            nonActiveScissorsObject.SetActive(true);
            nonActiveScissorsObject.transform.position = pos;
        }
        else
        {
            switch (objectType)
            {
                case GameDataScriptableObject.ObjectType.Rock:
                    Instantiate(GameData.GameDataScriptableObject.rockPrefab, pos, Quaternion.identity, sessionObjects.transform);
                    break;
                case GameDataScriptableObject.ObjectType.Paper:
                    Instantiate(GameData.GameDataScriptableObject.paperPrefab, pos, Quaternion.identity, sessionObjects.transform);
                    break;
                case GameDataScriptableObject.ObjectType.Scissors:
                    Instantiate(GameData.GameDataScriptableObject.scissorsPrefab, pos, Quaternion.identity, sessionObjects.transform);
                    break;
            }
            instancePositionsPool.Add(pos);
        }
    }

    public void AddNonActiveGameObjectToThePool(GameDataScriptableObject.ObjectType objectType, GameObject _gameObject)
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

    /// <summary>
    /// Fetches specified game object from pool if present.
    /// </summary>
    /// <returns></returns>
    private bool TryGetNonActiveGameObjectFromThePool(GameDataScriptableObject.ObjectType type, out GameObject nonActiveGameObject)
    {
        nonActiveGameObject = null;

        if (!nonActiveObjectsPool.ContainsKey(type)) return false;
        if (nonActiveObjectsPool[type].Count == 0) return false;

        var pair = nonActiveObjectsPool[type].First();

        nonActiveGameObject = pair.Value;
        nonActiveObjectsPool[type].Remove(pair.Key);

        return true;
    }

    public void PlayRockSFX()
    {
        if (!audioSoruce.isPlaying)
        {
            audioSoruce.PlayOneShot(GameData.GameDataScriptableObject.RockSFX, 0.3f);
        }
    }

    public void PlayPaperSFX()
    {
        if (!audioSoruce.isPlaying)
        {
            audioSoruce.PlayOneShot(GameData.GameDataScriptableObject.PaperSFX, 0.9f);
        }
    }

    public void PlayScissorsSFX()
    {
        if (!audioSoruce.isPlaying)
        {
            audioSoruce.PlayOneShot(GameData.GameDataScriptableObject.ScissorsSFX, 1.2f);
        }
    }
}
