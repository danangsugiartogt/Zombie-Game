using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialZombie : Enemy
{
    [SerializeField] private float moveSpeedBeforeExploding = 8.0f;
    [SerializeField] private float explodingRadius = 2;
    [SerializeField] private float extraTimeBeforeDeath = 3;
    [SerializeField] private GameObject explosionPrefab;

    private bool isDeath = false;

    public override void Initialize()
    {
        base.Initialize();
        hp = hpList[0];

        agent.speed = moveSpeed;
        SetText();
    }

    public override void TakeHeadDamage()
    {
        hp -= BodyDamage * 2;
        CheckLives();

        base.TakeHeadDamage();
    }

    public override void TakeBodyDamage()
    {
        hp -= BodyDamage;
        CheckLives();

        base.TakeBodyDamage();
    }

    protected override void CheckLives()
    {
        base.CheckLives();

        if (hp <= 0)
            OnDeath();
    }

    protected override void OnDeath()
    {
        if (isDeath) return;

        base.OnDeath();
        StartCoroutine(RageBeforeDeath());
        isDeath = true;
    }

    private IEnumerator RageBeforeDeath()
    {
        agent.speed = moveSpeedBeforeExploding;
        yield return new WaitForSeconds(extraTimeBeforeDeath);
        explosionPrefab.gameObject.SetActive(true);

        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        var collider = explosion.GetComponent<SphereCollider>();

        if (collider != null) collider.radius = explodingRadius;

        Destroy(gameObject);
    }
}
