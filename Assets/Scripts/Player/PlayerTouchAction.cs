using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchAction : MonoBehaviour
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private NavMeshAgent player;
    [SerializeField] private PlayerAnimations playerAnim;

    private bool isShooting = false;

    private Finger movementFinger;
    private Vector2 movementAmount;

    public bool IsShooting =>  isShooting;

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

    public void EnableJoystick(bool isEnabled)
    {
        joystick.gameObject.SetActive(isEnabled);
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
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
            playerAnim.SetAnimState(PlayerAnimations.AnimState.IdleShoot);
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
            isShooting = false;
            playerAnim.SetAnimState(PlayerAnimations.AnimState.Idle);
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x > Screen.width / 2f)
        {
            isShooting = true;
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.RectTransform.sizeDelta = joystickSize;
            joystick.RectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
            playerAnim.SetAnimState(PlayerAnimations.AnimState.IdleShoot);
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

        player.transform.LookAt(player.transform.position + scaledMovement, Vector3.up);
    }
}
