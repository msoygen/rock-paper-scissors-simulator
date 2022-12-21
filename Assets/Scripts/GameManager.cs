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

    // 0 rock 1 paper 2 scissors
    private List<int> pickList = new List<int>();

    [SerializeField]
    private AudioSource audioSoruce;

    private int populateLoopIndex = 0;

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

        StartCoroutine(PopulateGameScene());
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

    IEnumerator PopulateGameScene()
    {
        // instantiate using a scattering formation
        foreach (int pick in pickList)
        {
            populateLoopIndex++;
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

    // d_squared against minDistance squared
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

    // Generates (almost)uniformly distributed integers for each object that add up to the totalObjectCount.
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

        pickList.Shuffle();
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
            instancePositionsPool.Add(pos);
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
            instancePositionsPool.Add(pos);
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
            instancePositionsPool.Add(pos);
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
