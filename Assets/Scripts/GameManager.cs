using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const int GAME_SCENE_BUILD_INDEX = 1;

    public static GameManager instance;

    public float speed = 9f;
    public int totalObjectCount = 0;

    public Vector2 GameViewBoundaries { get { return GetGameViewBoundaries(); } }
    private Vector2 gameViewBoundaries;

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
        if (SceneManager.GetActiveScene().buildIndex == GAME_SCENE_BUILD_INDEX && SceneManager.GetActiveScene().isLoaded)
        {
            Bounds ortographicCameraBounds = Camera.main.OrthographicBounds();

            float ortographicCameraBounds_X_Half = ortographicCameraBounds.size.x / 2;
            float ortographicCameraBounds_Y_Half = ortographicCameraBounds.size.y / 2;

            gameViewBoundaries = new Vector2(ortographicCameraBounds_X_Half, ortographicCameraBounds_Y_Half);
        }
    }


    public Vector2 GetGameViewBoundaries()
    {
        return gameViewBoundaries;
    }
}
