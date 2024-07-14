using UnityEngine;
using GrappleZ_Player;
using UnityEngine.AI;
using System.Collections;

namespace GrappleZ_Enemy
{
    public class EnemyFirst : MonoBehaviour, IDamager, IDamageble
    {

        #region Public members
        public NavMeshAgent Agent;
        public LayerMask IsPlayerLayer;
        public Camera AttackingRaycastArea;
        #endregion

        #region Protected members
        protected Animator animator;
        protected Transform player;
        protected Transform lookPoint;
        #endregion

        #region Serializable Enemy Variables
        [Header("Enemy variables")]

        [SerializeField]
        protected float healt;
        [SerializeField]
        protected float damage;
        [SerializeField]
        protected float timeBetweenAttack;
        [SerializeField]
        protected float enemySpeed;
        [SerializeField]
        protected float attackingRadius;
        [SerializeField]
        protected float givenScorePoint;
        [SerializeField]
        protected float knockbackForce = 0.5f;
        [SerializeField]
        protected float knockbackDuration = 0.15f;
        #endregion

        #region Private Members
        private float currentHealt;

        private bool playerInAttackingRadius;
        protected bool hasAttacked = false;

        private const string walkingAnimatorParameter = "Walking";
        private const string idleAnimatorParameter = "Idle";
        private const string attackAnimatorParameter = "Attacking";
        private const string deadAnimatorParameter = "Dead";

        private EnemySpawnerBase spawnController;
        #endregion

        #region Audio
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip[] attackClipList;
        [SerializeField]
        private AudioClip deathSound;
        [SerializeField]
        private AudioClip hitSound;
        #endregion

        #region AnimatorMethods
        protected void SetAnimatorParameter(string name)
        {
            animator.SetTrigger(Animator.StringToHash(name));
        }
        protected void SetAnimatorParamerer(string name, bool value)
        {
            animator.SetBool(Animator.StringToHash(name), value);
        }
        protected void SetAnimatorParameter(string name, float value)
        {
            animator.SetFloat(Animator.StringToHash(name), value);
        }
        protected void SetAnimatorParameter(string name, int value)
        {
            animator.SetInteger(Animator.StringToHash(name), value);
        }
        protected void SetAttackAnimationParameters()
        {
            SetAnimatorParamerer(walkingAnimatorParameter, false);
            SetAnimatorParamerer(idleAnimatorParameter, false);
            SetAnimatorParamerer(attackAnimatorParameter, true);
            SetAnimatorParamerer(deadAnimatorParameter, false);
        }
        protected void SetWalkingAnimationParameters()
        {
            SetAnimatorParamerer(walkingAnimatorParameter, true);
            SetAnimatorParamerer(idleAnimatorParameter, false);
            SetAnimatorParamerer(attackAnimatorParameter, false);
            SetAnimatorParamerer(deadAnimatorParameter, false);
        }
        protected void SetIdleAnimationParameters()
        {
            SetAnimatorParamerer(walkingAnimatorParameter, false);
            SetAnimatorParamerer(idleAnimatorParameter, true);
            SetAnimatorParamerer(attackAnimatorParameter, false);
            SetAnimatorParamerer(deadAnimatorParameter, false);
        }
        protected void SetDeadAnimationParameters()
        {
            SetAnimatorParamerer(walkingAnimatorParameter, false);
            SetAnimatorParamerer(idleAnimatorParameter, false);
            SetAnimatorParamerer(attackAnimatorParameter, false);
            SetAnimatorParamerer(deadAnimatorParameter, true);
        }
        #endregion

        #region Mono
        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = enemySpeed;
            Agent.stoppingDistance = attackingRadius + 2;

            animator = GetComponent<Animator>();

            currentHealt = healt;

            player = Player.Get().transform;
            lookPoint = Player.instance.LookPointForEnemies;

            spawnController = FindObjectOfType<EnemySpawnerBase>();
        }

        private void Update()
        {
            if (Agent != null)
            {
                playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, IsPlayerLayer);

                if (!playerInAttackingRadius) Chase();
                if (playerInAttackingRadius) Attack();
            }
        }
        #endregion

        #region Pool
        public void OnDespawn()
        {
            gameObject.SetActive(false);
        }

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }
        #endregion

        #region AI
        protected virtual void Attack()
        {
            transform.LookAt(lookPoint);

            if (!hasAttacked)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
                {
                    //Debug.Log("Attacking" + hitInfo.transform.name);
                    if (!hitInfo.collider.CompareTag("Player")) return;

                    IDamageble damageble = hitInfo.collider.gameObject.GetComponent<IDamageble>();
                    if (damageble != null)
                    {
                        DamageContainer damageContainer = new DamageContainer();
                        damageContainer.Damage = damage;

                        damageble.TakeDamage(damageContainer);
                    }
                }

                SetAttackAnimationParameters();

                audioSource.PlayOneShot(attackClipList[UnityEngine.Random.Range(0, attackClipList.Length - 1)]);
                hasAttacked = true;
                Invoke(nameof(ActiveAttacking), timeBetweenAttack);
            }
        }
        private void Chase()
        {
            SetAgentDestination();

            if (SetAgentDestination())
            {
                SetWalkingAnimationParameters();
            }
            else
            {
                SetIdleAnimationParameters();
            }
        }
        protected virtual void Die()
        {
            GlobalEventManager.CastEvent(GlobalEventIndex.ScoreIncreased, GlobalEventArgsFactory.ScoreIncreaseFactory(givenScorePoint));
            enemySpeed = 0;
            attackingRadius = 0;
            playerInAttackingRadius = false;
            Agent.enabled = false;

            audioSource.PlayOneShot(deathSound);

            StartCoroutine(DieCoroutine());
        }
        private bool SetAgentDestination()
        {
            if (!Agent.isOnNavMesh)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                }
                else
                {
                    Debug.Log("Agent cannot be placed on NavMesh");
                    return false;
                }
            }

            if (Agent.isOnNavMesh)
            {
                Vector3 targetPosition;
                if (player.position.y > 1)
                {
                    // Player is in air
                    targetPosition = new Vector3(player.position.x, 0, player.position.z);
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(targetPosition, out hit, 5f, NavMesh.AllAreas))
                    {
                        targetPosition = hit.position;
                    }
                }
                else
                {
                    targetPosition = player.position;
                }

                Agent.SetDestination(targetPosition);
                return true;
            }
            return false;
        }
        #endregion

        #region Interface Methods
        public void TakeDamage(DamageContainer damage)
        {
            HitReaction();
            InternalTakeDamage(damage.Damage);
        }
        #endregion

        #region Internal Methods
        private float GetBodyDamage()
        {
            return damage;
        }
        private void ActiveAttacking()
        {
            hasAttacked = false;
            SetAnimatorParamerer(attackAnimatorParameter, false);
        }
        private void InternalTakeDamage(float dmg)
        {
            currentHealt -= dmg;
            if (currentHealt <= 0)
            {
                SetDeadAnimationParameters();
                Die();
            }
        }
        private void HitReaction()
        {
            StartCoroutine(FlashEnemyCoroutine());

            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
            StartCoroutine(KnockbackCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(0.1f);

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                yield return null;
            }
            yield return null;

            spawnController.DespawnToPool(gameObject);
        }
        private IEnumerator FlashEnemyCoroutine()
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

            Agent.enabled = false;

            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + knockbackDirection * knockbackForce;

            while (elapsedTime < knockbackDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Agent.enabled = true;
            Agent.Warp(transform.position);

            SetAgentDestination();
        }
        #endregion

        #region Cb
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //InternalTakeDamage(damage);
            }
        }
        #endregion
    }
}
