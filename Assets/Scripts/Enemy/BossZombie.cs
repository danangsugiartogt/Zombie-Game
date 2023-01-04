using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombie : Enemy
{
    [SerializeField] private float[] attackRanges;
    [SerializeField] private float[] moveSpeeds;
    [SerializeField] private float[] sizeScales;

    private int indexForm = 0;

    public override void Initialize()
    {
        base.Initialize();
        SetBossForm();
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (collider.gameObject.tag == "explosion")
        {
            hp -= ExplodingDamageToEnemy;

            CheckLives();
        }
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

        if (HP <= 0)
            OnDeath();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        if(indexForm < 2)
        {
            indexForm++;
            SetBossForm();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetBossForm()
    {
        hp = hpList[indexForm];
        moveSpeed = moveSpeeds[indexForm];
        attackRange = attackRanges[indexForm];
        transform.localScale = new Vector3(sizeScales[indexForm], sizeScales[indexForm], sizeScales[indexForm]);
        agent.speed = moveSpeed;
    }
}
