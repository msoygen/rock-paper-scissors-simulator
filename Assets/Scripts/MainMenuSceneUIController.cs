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

    private ColorBlock dummyColorBlock = new ColorBlock();

    private List<Color> colorOptions = new List<Color>();
    private List<Material> materialOptions = new List<Material>();

    private void Start()
    {
        colorOptions.Add(GameData.GameDataScriptableObject.ROCK_TEXT_COLOR);
        colorOptions.Add(GameData.GameDataScriptableObject.PAPER_TEXT_COLOR);
        colorOptions.Add(GameData.GameDataScriptableObject.SCISSORS_TEXT_COLOR);

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
        StartCoroutine(SwitchUIStyle(simulationSizeSlider));
        StartCoroutine(SwitchUIStyle(PlayButton));
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(LOADING_SCENE_BUILD_INDEX);
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

    IEnumerator SwitchUIStyle(Slider slider)
    {
        yield return new WaitForSeconds(.1f);
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
