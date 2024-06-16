using System;
using UnityEngine;
using UnityEngine.Pool;
using GrappleZ_Player;
using UnityEngine.AI;


public class EnemyFirst : MonoBehaviour, IDamager, IDamageble
{
    public NavMeshAgent agent;
    protected Animator anim;
    protected Transform player;
    public LayerMask IsPlayerLayer;
    public Camera AttackingRaycastArea;

    [Header("Enemy variables")]
    [SerializeField]
    private float healt;
    private float currentHealt;

    [SerializeField]
    private float damage;
    [SerializeField]
    protected float timeBetweenAttack;
    [SerializeField]
    private float enemySpeed;

    protected bool hasAttacked = false;

    [SerializeField]
    private float attackingRadius;

    private bool playerInAttackingRadius;

    ///ENEMY-POOL///
    private IObjectPool<EnemyFirst> enemyPool;

    public void SetPool(IObjectPool<EnemyFirst> enemyPool)
    {
        this.enemyPool = enemyPool;
    }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        agent.stoppingDistance = attackingRadius;

        anim = GetComponent<Animator>();
        currentHealt = healt;
        player = FindObjectOfType<Player>().transform;
        
        

    }
    private void Update()
    {
        if (agent != null)
        {
            playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, IsPlayerLayer);

            if (!playerInAttackingRadius) Chase();
            if (playerInAttackingRadius) Attack();
        }
    }

    protected virtual void Attack()
    {
        //agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!hasAttacked)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius)) 
            {
                Debug.Log("Attacking" + hitInfo.transform.name);
                if (!hitInfo.collider.CompareTag("Player")) return;

                IDamageble damageble = hitInfo.collider.gameObject.GetComponent<IDamageble>();
                if (damageble != null)
                {
                    DamageContainer damageContainer = new DamageContainer();
                    damageContainer.Damage = damage;
                    
                    damageble.TakeDamage(damageContainer);

                }
                

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
        agent.SetDestination(player.position);
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

    //private void Patrol()
    //{
    //    if (Vector3.Distance(WalkPoints[currentPosition].transform.position, transform.position)<walkingPointRadius)
    //    {
    //        currentPosition = UnityEngine.Random.Range(0, WalkPoints.Length);
    //        if (currentPosition >= WalkPoints.Length)
    //        {
    //            currentPosition = 0;
    //        }
    //    }
    //    transform.position = Vector3.MoveTowards(transform.position, WalkPoints[currentPosition].transform.position, Time.deltaTime * enemySpeed);

    //    //FaceDirection
    //    transform.LookAt(WalkPoints[currentPosition].transform.position);
    //}

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
        playerInAttackingRadius = false;

        enemyPool.Release(this);
        //Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(DamageContainer damage)
    {
        
    }
}
