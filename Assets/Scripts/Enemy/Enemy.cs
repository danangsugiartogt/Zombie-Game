using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected const int ExplodingDamageToEnemy = 125;
    protected const int BodyDamage = 40;
    public const int ExplodingDamageToPlayer = 1;

    public enum Type
    {
        Normal,
        Special,
        Boss
    }

    [SerializeField] protected Type type;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int[] hpList;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected bool isDebug = true;
    protected int hp;

    protected Action onDestroy;

    public int HP => hp;
    public Type EnemyType => type;

    private Transform target;

    protected virtual void OnDestroy()
    {
        onDestroy?.Invoke();
    }

    protected virtual void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        target = FindObjectOfType<PlayerTouchMovement>().transform;
        agent.speed = moveSpeed;
    }

    protected virtual void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance > attackRange)
        {
            agent.SetDestination(target.position);
        }

        if(distance <= agent.stoppingDistance)
        {
            FaceTarget();
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                if(isDebug) Destroy(gameObject); // debug
                break;
        }
    }

    public virtual void TakeHeadDamage()
    {

    }

    public virtual void TakeBodyDamage()
    {

    }

    protected virtual void CheckLives()
    {

    }

    protected virtual void OnDeath()
    {
        Debug.Log("OnDeath");
    }

    public virtual void SetOnDestroyEvent(Action onDestroy)
    {
        this.onDestroy = onDestroy;
    }
}
