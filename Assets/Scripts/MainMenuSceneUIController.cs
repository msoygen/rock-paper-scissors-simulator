using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneUIController : MonoBehaviour
{
    private const int LOADING_SCENE_BUILD_INDEX = 1;

    [SerializeField]
    private Canvas canvas;

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(LOADING_SCENE_BUILD_INDEX);
    }
}
