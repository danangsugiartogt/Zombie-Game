using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerTouchMovement playerMovement;
    [SerializeField] private PlayerTouchAction playerAction;
    [SerializeField] private PlayerAnimations playerAnim;

    [SerializeField] private int lives = 5;

    private bool isCollideWithEnemy = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        playerAnim.SetAnimState(PlayerAnimations.AnimState.Idle);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            OnCollideNormalZombie(other.gameObject);
        }
        else if(other.gameObject.tag == "explosion")
        {
            SubstractLives();
        }
    }

    private void OnCollideNormalZombie(GameObject go)
    {
        if (!isCollideWithEnemy)
        {
            var enemy = go.GetComponent<Enemy>();
            if (enemy != null && enemy.EnemyType == Enemy.Type.Special) return;

            isCollideWithEnemy = true;
            SubstractLives();
            StartCoroutine(DelayCollision());
        }
    }

    private void SubstractLives()
    {
        lives--;
        
        if(lives <= 0)
        {
            playerAnim.SetAnimState(PlayerAnimations.AnimState.Death, true);
            playerMovement.enabled = false;
            playerAction.enabled = false;
        }
    }

    private IEnumerator DelayCollision()
    {
        yield return new WaitForSeconds(.5f);
        isCollideWithEnemy = false;
    }
}
