using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] protected Text text;
    [SerializeField] protected bool isDebug = true;
    protected int hp;

    protected Action onDestroy;

    public int HP => hp;
    public Type EnemyType => type;

    private Transform target;

    protected void OnEnable()
    {
        CameraEvent.OnSwitch += OnCameraSwitch;
    }

    protected void OnDisable()
    {
        CameraEvent.OnSwitch -= OnCameraSwitch;
    }

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

    protected virtual void OnCameraSwitch(bool isThirdPerson)
    {
        var rect = text.GetComponent<RectTransform>();

        if (rect == null) return;

        if (isThirdPerson)
            rect.localRotation = Quaternion.Euler(new Vector3(0, rect.localRotation.y, 0));
        else
            rect.localRotation = Quaternion.Euler(new Vector3(EnemyType == Type.Boss ? -90 : 90, rect.localRotation.y, 0));
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

    protected virtual void SetText()
    {
        text.text = $"Hp: {hp}";
    }

    public virtual void TakeHeadDamage()
    {
        SetText();
    }

    public virtual void TakeBodyDamage()
    {
        SetText();
    }

    protected virtual void CheckLives()
    {

    }

    protected virtual void OnDeath()
    {

    }

    public virtual void SetOnDestroyEvent(Action onDestroy)
    {
        this.onDestroy = onDestroy;
    }
}
