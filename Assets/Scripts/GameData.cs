using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static GameDataScriptableObject GameDataScriptableObject { get => GetGameDataScriptableObject(); }
    private static GameDataScriptableObject gameDataScriptableObject;

    private static GameDataScriptableObject GetGameDataScriptableObject()
    {
        if(gameDataScriptableObject == null)
        {
            gameDataScriptableObject = Resources.Load("GameData", typeof(GameDataScriptableObject)) as GameDataScriptableObject;
        }

        return gameDataScriptableObject;
    }
}
