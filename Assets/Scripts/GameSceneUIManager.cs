using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    public void ToggleCanvas()
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
    }
}
