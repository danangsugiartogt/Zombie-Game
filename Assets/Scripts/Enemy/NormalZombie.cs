using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : Enemy
{
    public override void Initialize()
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

    public void AddHp(int value)
    {
        hp += value;
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
        base.OnDeath();
        Destroy(gameObject);
    }
}
