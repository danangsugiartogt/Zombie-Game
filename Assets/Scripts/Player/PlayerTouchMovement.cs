using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] private PlayerTouchAction playerTouchAction;
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private PlayerAnimations playerAnim;

    private bool isWalking = false;

    private Finger movementFinger;
    private Vector2 movementAmount;

    public bool IsWalking => isWalking;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            isWalking = true;

            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2f;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(
                    currentTouch.screenPosition,
                    joystick.RectTransform.anchoredPosition
                ) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - joystick.RectTransform.anchoredPosition
                    ).normalized
                    * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
            }

            joystick.Knob.anchoredPosition = knobPosition;
            movementAmount = knobPosition / maxMovement;
            playerAnim.SetAnimState(PlayerAnimations.AnimState.Walk);
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            isWalking = false;
            movementFinger = null;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
            playerAnim.SetAnimState(PlayerAnimations.AnimState.Idle);
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.RectTransform.sizeDelta = joystickSize;
            joystick.RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
        {
            startPosition.x = joystickSize.x / 2;
        }

        if (startPosition.y < joystickSize.y / 2)
        {
            startPosition.y = joystickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - joystickSize.y / 2)
        {
            startPosition.y = Screen.height - joystickSize.y / 2;
        }

        return startPosition;
    }

    private void Update()
    {
        Vector3 scaledMovement = player.speed * Time.deltaTime * new Vector3(
            movementAmount.x,
            0,
            movementAmount.y
        );

        if (!playerTouchAction.IsShooting)
            player.transform.LookAt(player.transform.position + scaledMovement, Vector3.up);

        player.Move(scaledMovement);
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        if (movementFinger != null)
        {
            GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {movementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {movementFinger.currentTouch.screenPosition}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}
