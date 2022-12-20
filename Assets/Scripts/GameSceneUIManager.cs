using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
    private const int LOADING_SCENE_BUILD_INDEX = 1;
    private const int MAIN_MENU_SCENE_BUILD_INDEX = 0;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject selectWinnerParent;
    [SerializeField]
    private GameObject gameStatsParent;
    [SerializeField]
    private GameObject gameOverParent;

    [SerializeField]
    private TMP_Text rockCountText;
    [SerializeField]
    private TMP_Text paperCountText;
    [SerializeField]
    private TMP_Text scissorsCountText;

    [SerializeField]
    private TMP_Text currentRockCountText;
    [SerializeField]
    private TMP_Text currentPaperCountText;
    [SerializeField]
    private TMP_Text currentScissorsCountText;
    [SerializeField]
    private TMP_Text playersPickText;
    [SerializeField]
    private Image playersPickImage;

    [SerializeField]
    private Image gameOverParent_Image;
    [SerializeField]
    private TMP_Text winnerText_GameOver;
    [SerializeField]
    private TMP_Text wonText_GameOver;
    [SerializeField]
    private TMP_Text playersPickText_GameOver;
    [SerializeField]
    private Image playersPickImage_GameOver;
    [SerializeField]
    private Button RestartButton;
    [SerializeField]
    private Button MainMenuButton;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        UpdateGameStatsObject();
        UpdateSelectWinnerObject();
    }

    public void ToggleCanvas()
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
    }

    public void ToggleSelectWinnerPanel()
    {
        selectWinnerParent.SetActive(!selectWinnerParent.gameObject.activeSelf);
    }
    public void ToggleGameOverPanel()
    {
        selectWinnerParent.SetActive(!gameOverParent.gameObject.activeSelf);
    }

    public void ToggleGameStatsPanel()
    {
        gameStatsParent.SetActive(!gameStatsParent.gameObject.activeSelf);
    }

    public void OnRockButtonClicked()
    {
        GameData.GameDataScriptableObject.winnerPick = GameDataScriptableObject.ObjectType.Rock;

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = GameData.GameDataScriptableObject.RockTextMaterial;
        playersPickText.color = GameData.GameDataScriptableObject.ROCK_TEXT_COLOR;
        playersPickImage.sprite = GameData.GameDataScriptableObject.RockSprite;

        Time.timeScale = 1f;
    }

    public void OnPaperButtonClicked()
    {
        GameData.GameDataScriptableObject.winnerPick = GameDataScriptableObject.ObjectType.Paper;

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = GameData.GameDataScriptableObject.PaperTextMaterial;
        playersPickText.color = GameData.GameDataScriptableObject.PAPER_TEXT_COLOR;
        playersPickImage.sprite = GameData.GameDataScriptableObject.PaperSprite;

        Time.timeScale = 1f;
    }

    public void OnScissorsButtonClicked()
    {
        GameData.GameDataScriptableObject.winnerPick = GameDataScriptableObject.ObjectType.Scissors;

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = GameData.GameDataScriptableObject.ScissorsTextMaterial;
        playersPickText.color = GameData.GameDataScriptableObject.SCISSORS_TEXT_COLOR;
        playersPickImage.sprite = GameData.GameDataScriptableObject.ScissorsSprite;

        Time.timeScale = 1f;
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(LOADING_SCENE_BUILD_INDEX);
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE_BUILD_INDEX);
    }

    public void OnGameOver(GameDataScriptableObject.ObjectType winner)
    {
        gameOverParent.SetActive(true);
        if (GameData.GameDataScriptableObject.winnerPick == winner)
        {
            gameOverParent_Image.color = GameData.GameDataScriptableObject.GAME_OVER_WIN_COLOR;
        }
        else
        {
            gameOverParent_Image.color = GameData.GameDataScriptableObject.GAME_OVER_LOSE_COLOR;
        }

        if (winner == GameDataScriptableObject.ObjectType.Rock)
        {
            SetGameOverUIStyle("Rock", GameData.GameDataScriptableObject.ROCK_TEXT_COLOR, GameData.GameDataScriptableObject.RockTextMaterial);
        }
        else if (winner == GameDataScriptableObject.ObjectType.Paper)
        {
            SetGameOverUIStyle("Paper", GameData.GameDataScriptableObject.PAPER_TEXT_COLOR, GameData.GameDataScriptableObject.PaperTextMaterial);
        }
        else if (winner == GameDataScriptableObject.ObjectType.Scissors)
        {
            SetGameOverUIStyle("Scissors", GameData.GameDataScriptableObject.SCISSORS_TEXT_COLOR, GameData.GameDataScriptableObject.ScissorsTextMaterial);
        }
    }

    private void UpdateGameStatsObject()
    {
        currentRockCountText.SetText(GameData.GameDataScriptableObject.rockCount.ToString());
        currentPaperCountText.SetText(GameData.GameDataScriptableObject.paperCount.ToString());
        currentScissorsCountText.SetText(GameData.GameDataScriptableObject.scissorsCount.ToString());
    }

    private void UpdateSelectWinnerObject()
    {
        rockCountText.SetText(GameData.GameDataScriptableObject.rockCount.ToString());
        paperCountText.SetText(GameData.GameDataScriptableObject.paperCount.ToString());
        scissorsCountText.SetText(GameData.GameDataScriptableObject.scissorsCount.ToString());
    }

    private void SetGameOverUIStyle(string winner, Color color, Material material)
    {
        ColorBlock dummyColorBlock;

        winnerText_GameOver.text = winner;
        winnerText_GameOver.color = color;
        winnerText_GameOver.fontMaterial = material;

        wonText_GameOver.color = color;
        wonText_GameOver.fontMaterial = material;

        playersPickText_GameOver.color = color;
        playersPickText_GameOver.fontMaterial = material;

        playersPickImage_GameOver.sprite = playersPickImage.sprite;

        dummyColorBlock = RestartButton.colors;
        dummyColorBlock.normalColor = color;

        var highlightedColor = color;
        highlightedColor.a = 0.78f;
        dummyColorBlock.highlightedColor = highlightedColor;

        RestartButton.colors = dummyColorBlock;

        dummyColorBlock = MainMenuButton.colors;
        dummyColorBlock.normalColor = color;

        highlightedColor = color;
        highlightedColor.a = 0.78f;
        dummyColorBlock.highlightedColor = highlightedColor;

        MainMenuButton.colors = dummyColorBlock;
    }
}
