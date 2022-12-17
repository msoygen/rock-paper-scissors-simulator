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

    [SerializeField]
    private Material rockTextMaterial;
    [SerializeField]
    private Material paperTextMaterial;
    [SerializeField]
    private Material scissorsTextMaterial;

    [SerializeField]
    private Sprite rockSprite;
    [SerializeField]
    private Sprite paperSprite;
    [SerializeField]
    private Sprite scissorsSprite;

    private int winnerPick = 0; // 0 rock, 1 paper, 2 scissors

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
        winnerPick = 0;
        GameManager.instance.PopulateGameScene();

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = rockTextMaterial;
        playersPickText.color = new Color(155, 150, 163);
        playersPickImage.sprite = rockSprite;
    }

    public void OnPaperButtonClicked()
    {
        winnerPick = 1;
        GameManager.instance.PopulateGameScene();

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = paperTextMaterial;
        playersPickText.color = new Color(229, 223, 238);
        playersPickImage.sprite = paperSprite;
    }

    public void OnScissorsButtonClicked()
    {
        winnerPick = 2;
        GameManager.instance.PopulateGameScene();

        ToggleGameStatsPanel();
        ToggleSelectWinnerPanel();

        playersPickText.fontMaterial = scissorsTextMaterial;
        playersPickText.color = new Color(148, 3, 21);
        playersPickImage.sprite = scissorsSprite;
    }

    private void UpdateGameStatsObject()
    {
        currentRockCountText.SetText(GameManager.instance.RockCount.ToString());
        currentPaperCountText.SetText(GameManager.instance.PaperCount.ToString());
        currentScissorsCountText.SetText(GameManager.instance.ScissorsCount.ToString());
    }

    private void UpdateSelectWinnerObject()
    {
        rockCountText.SetText(GameManager.instance.RockCount.ToString());
        paperCountText.SetText(GameManager.instance.PaperCount.ToString());
        scissorsCountText.SetText(GameManager.instance.ScissorsCount.ToString());
    }
}
