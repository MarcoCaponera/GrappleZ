using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyJumper : EnemyFirst
{
    [SerializeField] 
    private float jumpForce = 10f;
    [SerializeField] 
    private float jumpDuration = 0.5f;
    [SerializeField] 
    private float damageRadius = 1f;

    private bool isJumping = false;

    protected override void Attack()
    {
        if (!hasAttacked)
        {
            float distanceToTarget = Vector3.Distance(transform.position, lookPoint.position);

            if (distanceToTarget <= attackingRadius && !isJumping)
            {
                StartJumpAttack();
                hasAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttack);
            }
        }
    }

    private void StartJumpAttack()
    {
        isJumping = true;

        if (agent != null) agent.enabled = false;

        anim.SetBool("Walking", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Eating", true);
        anim.SetBool("Dead", false);

        StartCoroutine(PerformJump());
    }

    private IEnumerator PerformJump()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = lookPoint.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;

            // Calculate jump 
            Vector3 center = (startPos + endPos) * 0.5f;
            center -= Vector3.up * jumpForce;
            Vector3 relativeStart = startPos - center;
            Vector3 relativeEnd = endPos - center;

            transform.position = Vector3.Slerp(relativeStart, relativeEnd, t);
            transform.position += center;

            ApplyDamage();

            yield return null;
        }

        LandingComplete();
    }

    private void ApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                IDamageble damageble = hitCollider.GetComponent<IDamageble>();
                if (damageble != null)
                {
                    DamageContainer damageContainer = new DamageContainer();
                    damageContainer.Damage = damage;
                    damageble.TakeDamage(damageContainer);
                }
            }
        }
    }

    private void LandingComplete()
    {
        // Place the enemy on the NavMesh
        PlaceOnNavMesh();

        isJumping = false;
        if (agent != null) agent.enabled = true;

        anim.SetBool("Eating", false);
    }

    private void PlaceOnNavMesh()
    {
        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
            else
            {
                Debug.LogWarning("Unable to find NavMesh");
            }
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }
}