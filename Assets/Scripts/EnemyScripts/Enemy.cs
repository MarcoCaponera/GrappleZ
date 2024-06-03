using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform lookPoint;
    public LayerMask IsGroundLayer, IsPlayerLayer;

    [Header("Enemy Healt and Damage")]

    private float healt = 10;
    private float currentHealt;

    public float damage = 5f;
    public Camera AttackingRaycastArea;
    public float timeBetweenAttack;
    bool hasAttacked;





    [Header("Enemy variables")]
    public GameObject[] WalkPoints;
    private int currentPosition = 0;
    public float walkingPointRadius = 2;
    public float enemySpeed;


    [Header("Enemy States")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInVisionRadius;
    public bool playerInAttackingRadius;

    private void Awake()
    {
        currentHealt = healt;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent != null)
        {
            playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, IsPlayerLayer);
            playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, IsPlayerLayer);

            if (!playerInVisionRadius && !playerInAttackingRadius) Guard();
            if (playerInVisionRadius && !playerInAttackingRadius) Chase();
            if (playerInVisionRadius && playerInAttackingRadius) Attack();

        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(lookPoint);

        if (!hasAttacked)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius)) 
            {
                Debug.Log("Attacking");
                if (!hitInfo.collider.CompareTag("Player")) return;
                Debug.Log("PlayerGetDamage");
            
                
            }
            hasAttacked = true;
            Invoke(nameof(ActiveAttacking), timeBetweenAttack);
        }
    }

    private void ActiveAttacking()
    {
        hasAttacked = false;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Guard()
    {
        if (Vector3.Distance(WalkPoints[currentPosition].transform.position, transform.position)<walkingPointRadius)
        {
            currentPosition = UnityEngine.Random.Range(0, WalkPoints.Length);
            if (currentPosition >= WalkPoints.Length)
            {
                currentPosition = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, WalkPoints[currentPosition].transform.position, Time.deltaTime * enemySpeed);

        //FaceDirection
        transform.LookAt(WalkPoints[currentPosition].transform.position);
    }

    public void TakeDamage(float dmg)
    {
        currentHealt -= dmg;
        if (currentHealt <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        agent.SetDestination(transform.position);
        enemySpeed = 0;
        attackingRadius = 0;
        visionRadius = 0;
        playerInAttackingRadius = false;
        playerInVisionRadius = false;
        Destroy(gameObject, 5f);
    }
}
