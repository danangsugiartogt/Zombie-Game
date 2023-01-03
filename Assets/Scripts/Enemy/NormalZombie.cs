using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : Enemy
{
    protected override void Initialize()
    {
        base.Initialize();

        if (hpList.Length > 1)
        {
            int rand = Random.Range(0, hpList.Length - 1);
            hp = hpList[rand];
        }
        else
        {
            hp = hpList[0];
        }
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if(collider.gameObject.tag == "explosion")
        {
            hp -= ExplodingDamageToEnemy;

            CheckLives();
        }
    }

    public override void TakeHeadDamage()
    {
        base.TakeHeadDamage();

        hp -= BodyDamage * 2;

        CheckLives();
    }

    public override void TakeBodyDamage()
    {
        base.TakeBodyDamage();

        hp -= BodyDamage;

        CheckLives();
    }

    protected override void CheckLives()
    {
        base.CheckLives();

        if (hp <= 0)
            OnDeath();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }
}
