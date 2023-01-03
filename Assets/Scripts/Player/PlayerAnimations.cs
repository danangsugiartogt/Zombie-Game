using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public enum AnimState
    {
        Idle,
        IdleWeapon,
        IdleShoot,
        Walk,
        WalkShoot,
        Reload,
        Death
    }

    private readonly string SpeedFloat = "Speed_f";
    private readonly string WeaponTypeInt = "WeaponType_int";
    private readonly string HeadHorizontalFloat = "Head_Horizontal_f";

    private readonly string FullAutoBool = "FullAuto_b";
    private readonly string ShootBool = "Shoot_b";
    private readonly string ReloadBool = "Reload_b";
    private readonly string DeathBool = "Death_b";

    [SerializeField] private Animator animController;
    [SerializeField] private AnimState animState;

    public void SetAnimState(AnimState animState, bool force = false)
    {
        if (this.animState == AnimState.Reload && !force) return;

        this.animState = animState;
        switch (this.animState)
        {
            case AnimState.Idle:
                SetToIdle();
                break;
            case AnimState.IdleShoot:
                SetToIdleShoot();
                break;
            case AnimState.Walk:
                SetToWalk();
                break;
            case AnimState.WalkShoot:
                SetToWalkShoot();
                break;
            case AnimState.Reload:
                SetToReloadBullet();
                break;
            case AnimState.Death:
                SetToDeath();
                break;
        }
    }

    private void SetToIdle()
    {
        animController.SetFloat(HeadHorizontalFloat, 0.0f);
        animController.SetFloat(SpeedFloat, 0.0f);
        animController.SetInteger(WeaponTypeInt, 0);

        animController.SetBool(FullAutoBool, false);
        animController.SetBool(ShootBool, false);
        animController.SetBool(ReloadBool, false);
    }

    private void SetToWalk()
    {
        animController.SetFloat(SpeedFloat, .5f);
        animController.SetInteger(WeaponTypeInt, 5);
    }

    private void SetToAimIdle()
    {
        animController.SetFloat(SpeedFloat, 0);
        animController.SetInteger(WeaponTypeInt, 5);
    }

    private void SetToIdleShoot()
    {
        //animController.SetFloat(HeadHorizontalFloat, -.2f);
        animController.SetInteger(WeaponTypeInt, 1);
        animController.SetBool(FullAutoBool, true);
        animController.SetBool(ShootBool, true);
        animController.SetBool(ReloadBool, false);
    }

    private void SetToWalkShoot()
    {
        animController.SetFloat(SpeedFloat, .5f);
        animController.SetInteger(WeaponTypeInt, 1);
        animController.SetBool(FullAutoBool, true);
        animController.SetBool(ShootBool, true);
        animController.SetBool(ReloadBool, false);
    }

    private void SetToReloadBullet()
    {
        animController.SetInteger(WeaponTypeInt, 4);
        animController.SetBool(ReloadBool, true);
    }

    private void SetToDeath()
    {
        animController.SetBool(DeathBool, true);
    }
}
