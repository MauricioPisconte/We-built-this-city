using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInitializer : MonoBehaviour
{
    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

}
