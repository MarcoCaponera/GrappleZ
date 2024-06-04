using System;
using UnityEngine;
using UnityEngine.Pool;
using GrappleZ_Player;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player;
    public Transform lookPoint;
    public LayerMask IsGroundLayer, IsPlayerLayer;
    public Camera AttackingRaycastArea;
    public GameObject[] WalkPoints;


    [Header("Enemy variables")]
    [SerializeField]
    private float healt = 10;
    private float currentHealt;

    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private float timeBetweenAttack;
    [SerializeField]
    private float enemySpeed = 10;

    bool hasAttacked = false;
    private int currentPosition = 0;
    private float walkingPointRadius = 5;

    [Header("Enemy States")]
    [SerializeField]
    private float visionRadius = 15;
    [SerializeField]
    private float attackingRadius = 2;
    private bool playerInVisionRadius;
    private bool playerInAttackingRadius;

    ///ENEMY-POOL///
    private IObjectPool<Enemy> enemyPool;

    public void SetPool(IObjectPool<Enemy> enemyPool)
    {
        this.enemyPool = enemyPool;
    }



    private void Awake()
    {
        currentHealt = healt;
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>().transform;

    }
    private void Update()
    {
        if (agent != null)
        {
            playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, IsPlayerLayer);
            playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, IsPlayerLayer);

            if (!playerInVisionRadius && !playerInAttackingRadius) Patrol();
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
                Debug.Log("Attacking" + hitInfo.transform.name);
                if (!hitInfo.collider.CompareTag("Player")) return;
                Debug.Log("PlayerGetDamage");
                
            }
            anim.SetBool("Walking", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Attacking", true);
            anim.SetBool("Dead", false);

            hasAttacked = true;
            Invoke(nameof(ActiveAttacking), timeBetweenAttack);
        }
    }

    private void ActiveAttacking()
    {
        hasAttacked = false;
        anim.SetBool("Attacking", false);


    }

    private void Chase()
    {
        if (agent.SetDestination(player.position))
        {
            anim.SetBool("Walking", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Dead", false);
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Dead", false);
        }
        
    }

    private void Patrol()
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
            anim.SetBool("Walking", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Dead", true);
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

        enemyPool.Release(this);
        //Destroy(gameObject, 5f);
    }
}
