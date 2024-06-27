using System;
using UnityEngine;
using UnityEngine.Pool;
using GrappleZ_Player;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;




public class EnemyFirst : MonoBehaviour, IDamager, IDamageble
{
    #region Public members
    public NavMeshAgent agent;
    public LayerMask IsPlayerLayer;
    public Camera AttackingRaycastArea;
    #endregion

    #region Protected members
    protected Animator anim;
    protected Transform player;
    [SerializeField]
    protected Transform lookPoint;
    #endregion

    #region Private members

    [Header("Enemy variables")]
    [SerializeField]
    private float healt;
    [SerializeField]
    private float damage;
    [SerializeField]
    protected float timeBetweenAttack;
    [SerializeField]
    private float enemySpeed;
    [SerializeField]
    protected float attackingRadius;
    [SerializeField]
    protected float givenScorePoint;

    private float currentHealt;
    private bool playerInAttackingRadius;
    protected bool hasAttacked = false;

    #endregion

    private EnemySpawnerBase spawnController;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        agent.stoppingDistance = attackingRadius+2;

        anim = GetComponent<Animator>();
        currentHealt = healt;
        player = Player.Get().transform;
        lookPoint = Player.Get().transform;

    }

    public void Initialize()
    {

    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }
 

    protected virtual void Die()
    {
        enemySpeed = 0;
        attackingRadius = 0;
        playerInAttackingRadius = false;
        OnDespawn();
        GlobalEventManager.CastEvent(GlobalEventIndex.ScoreIncreased, GlobalEventArgsFactory.ScoreIncreaseFactory(givenScorePoint));
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

        transform.LookAt(lookPoint);

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
    protected float GetBodyDamage()
    {
        return damage;
    }

    private void ActiveAttacking()
    {
        hasAttacked = false;
        anim.SetBool("Attacking", false);


    }

    private void Chase()
    {
        if (player.position.y > 1)
        {
            agent.SetDestination(new Vector3(player.position.x, 1, player.position.z));
            
        }else
        {

            SetAgentDestination();
        }
        if (SetAgentDestination())
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

    private bool SetAgentDestination()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            return true;
        }
        else
        {
            Debug.LogError("Agent is not on a NavMesh");
            return false;
        }
    }

    private void InternalTakeDamage(float dmg)
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InternalTakeDamage(damage);
        }
    }

    public void TakeDamage(DamageContainer damage)
    {
        InternalTakeDamage(damage.Damage);
    }
}
