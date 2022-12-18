using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneUIController : MonoBehaviour
{
    private const int LOADING_SCENE_BUILD_INDEX = 1;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject titleScreenParent;
    [SerializeField]
    private GameObject simulationSizeParent;

    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button PlayButton;

    [SerializeField]
    private TMP_Text simulationSizeText;
    [SerializeField]
    private TMP_Text chosenSimulationSizeText;
    [SerializeField]
    private Slider simulationSizeSlider;
    [SerializeField]
    private Image simulationSizeSlider_BackgroundImage;
    [SerializeField]
    private Image simulationSizeSlider_FillImage;
    [SerializeField]
    private Image simulationSizeSlider_HandleImage;

    private ColorBlock dummyColorBlock = new ColorBlock();

    private List<Color> colorOptions = new List<Color>();
    private List<Color> darkColorOptions = new List<Color>();
    private List<Material> materialOptions = new List<Material>();

    private void Start()
    {
        colorOptions.Add(GameData.GameDataScriptableObject.ROCK_TEXT_COLOR);
        colorOptions.Add(GameData.GameDataScriptableObject.PAPER_TEXT_COLOR);
        colorOptions.Add(GameData.GameDataScriptableObject.SCISSORS_TEXT_COLOR);

        darkColorOptions.Add(GameData.GameDataScriptableObject.ROCK_TEXT_COLOR_DARK);
        darkColorOptions.Add(GameData.GameDataScriptableObject.PAPER_TEXT_COLOR_DARK);
        darkColorOptions.Add(GameData.GameDataScriptableObject.SCISSORS_TEXT_COLOR_DARK);

        materialOptions.Add(GameData.GameDataScriptableObject.RockTextMaterial);
        materialOptions.Add(GameData.GameDataScriptableObject.PaperTextMaterial);
        materialOptions.Add(GameData.GameDataScriptableObject.ScissorsTextMaterial);

        StartCoroutine(SwitchUIStyle(StartButton));
    }

    public void OnStartButtonClicked()
    {
        titleScreenParent.SetActive(false);
        simulationSizeParent.SetActive(true);
        StartCoroutine(SwitchUIStyle(simulationSizeText));
        StartCoroutine(SwitchUIStyle(chosenSimulationSizeText));
        StartCoroutine(SwitchUIStyle(simulationSizeSlider, simulationSizeSlider_BackgroundImage, simulationSizeSlider_FillImage, simulationSizeSlider_HandleImage));
        StartCoroutine(SwitchUIStyle(PlayButton));
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(LOADING_SCENE_BUILD_INDEX);

        int sliderVal = (int)simulationSizeSlider.value;
        GameData.GameDataScriptableObject.totalObjectCount = sliderVal;

        /*
            0-50 -> 5 -> 3
            50-100 -> 7 -> 3
            100-200 -> 10 -> 4
            200-300 -> 13 -> 4
            300-400 -> 15 -> 6
            400-500 -> 16.5 -> 6
            500-600 -> 17.5 -> 7
            600-700 -> 18.5 -> 7
            700-800 -> 19.5 -> 8
            800-900 -> 20.5 -> 8
            900-1000 -> 22 -> 9
         */
        if (sliderVal > 0 && sliderVal <= 50)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 5f;
            GameData.GameDataScriptableObject.objectSpeed = 3f;
        }
        else if (sliderVal > 50 && sliderVal <= 100)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 7f;
            GameData.GameDataScriptableObject.objectSpeed = 3f;
        }
        else if (sliderVal > 100 && sliderVal <= 200)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 10f;
            GameData.GameDataScriptableObject.objectSpeed = 4f;
        }
        else if (sliderVal > 200 && sliderVal <= 300)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 13f;
            GameData.GameDataScriptableObject.objectSpeed = 4f;
        }
        else if (sliderVal > 300 && sliderVal <= 400)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 15f;
            GameData.GameDataScriptableObject.objectSpeed = 6f;
        }
        else if (sliderVal > 400 && sliderVal <= 500)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 16.5f;
            GameData.GameDataScriptableObject.objectSpeed = 6f;
        }
        else if (sliderVal > 500 && sliderVal <= 600)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 17.5f;
            GameData.GameDataScriptableObject.objectSpeed = 7f;
        }
        else if (sliderVal > 600 && sliderVal <= 700)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 18.5f;
            GameData.GameDataScriptableObject.objectSpeed = 7f;
        }
        else if (sliderVal > 700 && sliderVal <= 800)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 19.5f;
            GameData.GameDataScriptableObject.objectSpeed = 8f;
        }
        else if (sliderVal > 800 && sliderVal <= 900)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 20.5f;
            GameData.GameDataScriptableObject.objectSpeed = 8f;
        }
        else if (sliderVal > 900 && sliderVal <= 1000)
        {
            GameData.GameDataScriptableObject.orthograpicCameraSize = 22f;
            GameData.GameDataScriptableObject.objectSpeed = 9f;
        }
    }

    public void OnSliderValueChanged()
    {
        chosenSimulationSizeText.text = simulationSizeSlider.value.ToString();
    }

    IEnumerator SwitchUIStyle(TMP_Text text)
    {
        while (true)
        {
            for (int i = 0; i < colorOptions.Count; i++)
            {
                text.color = colorOptions[i];
                text.fontMaterial = materialOptions[i];

                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator SwitchUIStyle(Slider slider, Image backgroundImage, Image fillImage, Image handleImage)
    {
        while (true)
        {
            for (int i = 0; i < colorOptions.Count; i++)
            {
                backgroundImage.color = colorOptions[i];
                fillImage.color = darkColorOptions[i];
                handleImage.color = darkColorOptions[i];

                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator SwitchUIStyle(Button button)
    {
        while (true)
        {
            for (int i = 0; i < colorOptions.Count; i++)
            {
                dummyColorBlock = button.colors;
                dummyColorBlock.normalColor = colorOptions[i];
                var highlightedColor = colorOptions[i];
                highlightedColor.a = 0.78f;
                dummyColorBlock.highlightedColor = highlightedColor;

                button.colors = dummyColorBlock;

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
