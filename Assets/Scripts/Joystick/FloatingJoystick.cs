using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class FloatingJoystick : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform knob;

    public RectTransform RectTransform => rectTransform;
    public RectTransform Knob => knob;

    // Update is called once per frame
    void Update()
    {
        
    }
}
