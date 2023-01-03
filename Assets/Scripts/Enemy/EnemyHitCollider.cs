using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCollider : MonoBehaviour
{
    public enum HitType
    {
        Head,
        Body
    }

    [SerializeField] private HitType hitType;
    [SerializeField] private Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "bullet")
        {
            if (hitType == HitType.Head)
                enemy.TakeHeadDamage();
            else
                enemy.TakeBodyDamage();
        }
    }
}
