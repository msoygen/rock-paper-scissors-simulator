using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject selectWinnerPanel;
    [SerializeField]
    private GameObject gameStatsPanel;
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

    private int winnerPick = 0; // 0 rock, 1 paper, 2 scissors

    private void Start()
    {
        rockCountText.text = GameManager.instance.RockCount.ToString();
        paperCountText.text = GameManager.instance.PaperCount.ToString();
        scissorsCountText.text = GameManager.instance.ScissorsCount.ToString();
    }

    private void LateUpdate()
    {
        UpdateGameStatsPanel();
    }

    public void ToggleCanvas()
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
    }

    public void ToggleSelectWinnerPanel()
    {
        selectWinnerPanel.SetActive(!selectWinnerPanel.gameObject.activeSelf);
    }

    public void ToggleGameStatsPanel()
    {
        gameStatsPanel.SetActive(!gameStatsPanel.gameObject.activeSelf);
    }

    public void OnRockButtonClicked()
    {
        winnerPick = 0;
        playersPickText.text = "Your Pick: Rock";
        GameManager.instance.PopulateGameScene();
        ToggleSelectWinnerPanel();
        ToggleGameStatsPanel();
        UpdateGameStatsPanel();
    }

    public void OnPaperButtonClicked()
    {
        winnerPick = 1;
        playersPickText.text = "Your Pick: Paper";
        GameManager.instance.PopulateGameScene();
        ToggleSelectWinnerPanel();
        ToggleGameStatsPanel();
        UpdateGameStatsPanel();
    }

    public void OnScissorsButtonClicked()
    {
        winnerPick = 2;
        playersPickText.text = "Your Pick: Scissors";
        GameManager.instance.PopulateGameScene();
        ToggleSelectWinnerPanel();
        ToggleGameStatsPanel();
        UpdateGameStatsPanel();
    }

    private void UpdateGameStatsPanel()
    {
        rockCountText.text = GameManager.instance.RockCount.ToString();
        paperCountText.text = GameManager.instance.PaperCount.ToString();
        scissorsCountText.text = GameManager.instance.ScissorsCount.ToString();
    }
}
