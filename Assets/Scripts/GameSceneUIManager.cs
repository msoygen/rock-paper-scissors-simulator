using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject selectWinnerParent;
    [SerializeField]
    private GameObject gameStatsParent;
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

    public void ToggleGameStatsPanel()
    {
        gameStatsParent.SetActive(!gameStatsParent.gameObject.activeSelf);
    }

    public void OnRockButtonClicked()
    {
        GameData.GameDataScriptableObject.winnerPick = GameDataScriptableObject.ObjectType.Rock;
        GameManager.instance.PopulateGameScene();

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = GameData.GameDataScriptableObject.RockTextMaterial;
        playersPickText.color = new Color(155, 150, 163);
        playersPickImage.sprite = GameData.GameDataScriptableObject.RockSprite;
    }

    public void OnPaperButtonClicked()
    {
        GameData.GameDataScriptableObject.winnerPick = GameDataScriptableObject.ObjectType.Paper;
        GameManager.instance.PopulateGameScene();

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = GameData.GameDataScriptableObject.PaperTextMaterial;
        playersPickText.color = new Color(229, 223, 238);
        playersPickImage.sprite = GameData.GameDataScriptableObject.PaperSprite;
    }

    public void OnScissorsButtonClicked()
    {
        GameData.GameDataScriptableObject.winnerPick = GameDataScriptableObject.ObjectType.Scissors;
        GameManager.instance.PopulateGameScene();

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = GameData.GameDataScriptableObject.ScissorsTextMaterial;
        playersPickText.color = new Color(148, 3, 21);
        playersPickImage.sprite = GameData.GameDataScriptableObject.ScissorsSprite;
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
}
