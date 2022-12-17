using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameDataScriptableObject", order = 1)]
public class GameDataScriptableObject : ScriptableObject
{
    public enum ObjectType
    {
        Rock,
        Paper,
        Scissors,
        Fallback
    }

    public int totalObjectCount = 0;
    public float objectSpeed = 9f;

    public GameObject rockPrefab;
    public GameObject paperPrefab;
    public GameObject scissorsPrefab;

    public int rockCount = 0;
    public int paperCount = 0;
    public int scissorsCount = 0;

    public Vector2 GameViewBoundaries { get => gameViewBoundaries; }

    private Vector2 gameViewBoundaries;

    public float orthograpicCameraSize;

    public ObjectType winnerPick = ObjectType.Fallback;

    public readonly Color ROCK_TEXT_COLOR = new Color(113f / 255f, 108f / 255f, 116f / 255f);
    public readonly Color PAPER_TEXT_COLOR = new Color(229f / 255f, 223f / 255f, 238f / 255f);
    public readonly Color SCISSORS_TEXT_COLOR = new Color(148f / 255f, 3f / 255f, 21f / 255f);

    public readonly Color ROCK_TEXT_COLOR_DARK = new Color(57f / 255f, 56f / 255f, 69f / 255f);
    public readonly Color PAPER_TEXT_COLOR_DARK = new Color(57f / 255f, 56f / 255f, 69f / 255f);
    public readonly Color SCISSORS_TEXT_COLOR_DARK = new Color(92f / 255f, 0f, 9f / 255f);

    public Material RockTextMaterial { get => rockTextMaterial; }
    private Material rockTextMaterial;

    public Material PaperTextMaterial { get => paperTextMaterial; }
    private Material paperTextMaterial;

    public Material ScissorsTextMaterial { get => scissorsTextMaterial; }
    private Material scissorsTextMaterial;

    public Sprite RockSprite { get => rockSprite; }
    private Sprite rockSprite;
    public Sprite PaperSprite { get => paperSprite; }
    private Sprite paperSprite;
    public Sprite ScissorsSprite { get => scissorsSprite; }
    private Sprite scissorsSprite;

    private void OnEnable()
    {
        rockTextMaterial = Resources.Load("Materials/RockTextFontMaterial", typeof(Material)) as Material;
        paperTextMaterial = Resources.Load("Materials/PaperTextFontMaterial", typeof(Material)) as Material;
        scissorsTextMaterial = Resources.Load("Materials/ScissorsTextFontMaterial", typeof(Material)) as Material;
        rockSprite = Resources.Load("Sprites/rock_pixelart") as Sprite;
        paperSprite = Resources.Load("Sprites/paper_pixelart") as Sprite;
        scissorsSprite = Resources.Load("Sprites/scissors_pixelart") as Sprite;
    }

    public void UpdateGameViewBoundaries()
    {
        Bounds ortographicCameraBounds = Camera.main.OrthographicBounds();

        float ortographicCameraBounds_X_Half = ortographicCameraBounds.size.x / 2;
        float ortographicCameraBounds_Y_Half = ortographicCameraBounds.size.y / 2;

        gameViewBoundaries = new Vector2(ortographicCameraBounds_X_Half, ortographicCameraBounds_Y_Half);
    }

    public void AddToRockCount(int amount)
    {
        rockCount += amount;
    }
    public void AddToPaperCount(int amount)
    {
        paperCount += amount;
    }
    public void AddToScissorsCount(int amount)
    {
        scissorsCount += amount;
    }
}
