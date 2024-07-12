using System;
using UnityEngine;
using UnityEngine.Pool;
using GrappleZ_Player;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using System.Collections;




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
    protected float damage;
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
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] attackClipList;
    [SerializeField]
    private AudioClip deathSound;
    //[SerializeField]
    //private GameObject hitEffect;
    [SerializeField] 
    private AudioClip hitSound;
    [SerializeField] 
    private float knockbackForce = 0.5f;
    [SerializeField] 
    private float knockbackDuration = 0.15f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        agent.stoppingDistance = attackingRadius+2;

        anim = GetComponent<Animator>();
        currentHealt = healt;
        player = Player.Get().transform;
        lookPoint = Player.instance.LookPointForEnemies;
        spawnController = FindObjectOfType<EnemySpawnerBase>();

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
        GlobalEventManager.CastEvent(GlobalEventIndex.ScoreIncreased, GlobalEventArgsFactory.ScoreIncreaseFactory(givenScorePoint));
        enemySpeed = 0;
        attackingRadius = 0;
        playerInAttackingRadius = false;
        agent.enabled = false;

        audioSource.PlayOneShot(deathSound);

        StartCoroutine(DieCoroutine());
    }

    //private static readonly int DeathState = Animator.StringToHash("Death");

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        //Debug.Log("Current animation state: " + anim.GetCurrentAnimatorStateInfo(0).fullPathHash);
        //Debug.Log("Is in Death state: " + anim.GetCurrentAnimatorStateInfo(0).IsName("Death"));

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }
        yield return null;

        spawnController.DespawnToPool(gameObject);
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

            audioSource.PlayOneShot(attackClipList[UnityEngine.Random.Range(0, attackClipList.Length-1)]);

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

    protected bool SetAgentDestination()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            return true;
        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            //Debug.Log("Agent is not on a NavMesh");
            //return false;
            return true;
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
           // Debug.Log("Setting Dead to true");
            Die();
        }
    }

    private void HitReaction()
    {
        //Vector3 hitOffset= transform.up * 2f + transform.forward * 0.5f;
        //GameObject onHitEffect = Instantiate(hitEffect, transform.position + hitOffset, Quaternion.LookRotation(-transform.forward));
       // Destroy(onHitEffect , 0.5f);
        StartCoroutine(FlashEnemy());

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
        StartCoroutine(KnockbackCoroutine());
    }

    private IEnumerator FlashEnemy()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            renderer.material.color = originalColor;
        }
        else
        {
            Debug.Log("No Renderer found");
        }
    }

    private IEnumerator KnockbackCoroutine()
    {
        Vector3 knockbackDirection = (transform.position - player.position).normalized;
        knockbackDirection.y = 0;

        agent.enabled = false;

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + knockbackDirection * knockbackForce;

        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        agent.enabled = true;
        agent.Warp(transform.position);

        SetAgentDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //InternalTakeDamage(damage);
        }
    }

    public void TakeDamage(DamageContainer damage)
    {
        Debug.Log($"Enemy took {damage.Damage} damage");
        HitReaction();
        InternalTakeDamage(damage.Damage);
    }
}
