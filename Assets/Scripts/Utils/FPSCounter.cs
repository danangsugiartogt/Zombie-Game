using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private string displayText = "";

    void Update()
    {
        // Calculate the frame rate
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;

        // Display the frame rate in the top-left corner of the screen
        displayText = "FPS: " + fps.ToString("F2");
    }

    void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };

        GUI.Label(new Rect(10, 10, 200, 20), displayText, labelStyle);
    }
}
