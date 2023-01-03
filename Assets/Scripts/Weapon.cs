using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private PlayerTouchAction playerTouchAction;
    [SerializeField] private PlayerAnimations playerAnim;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10;

    [SerializeField] private float cooldownDuration = .3f;
    [SerializeField] private float timerCooldown = 0;

    [SerializeField] private int bulletPerClip = 24;
    [SerializeField] private float reloadDuration = 2.5f;
    private int remainingBullet = 24;

    private bool isReloading = false;

    private void Update()
    {
        if (playerTouchAction.IsShooting && !isReloading)
        {
            if (timerCooldown <= 0)
            {
                if(remainingBullet > 0)
                {
                    timerCooldown = cooldownDuration;
                    var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
                    remainingBullet--;
                }
                else
                {
                    StartCoroutine(Reload());
                }
            }
        }

        if (timerCooldown > 0) timerCooldown -= Time.deltaTime;
    }

    private IEnumerator Reload()
    {
        playerAnim.SetAnimState(PlayerAnimations.AnimState.Reload);
        isReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        remainingBullet = bulletPerClip;
        playerAnim.SetAnimState(PlayerAnimations.AnimState.Idle, true);
        isReloading = false;
    }
}
