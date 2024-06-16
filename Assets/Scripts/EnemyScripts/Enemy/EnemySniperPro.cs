using GrappleZ_Player;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class EnemySniperPro : EnemyFirst
{
    //[SerializeField]
    //private GameObject bulletPrefab;
    //private void Awake()
    //{
    //    bulletPrefab = Instantiate(bulletPrefab);
    //    bulletPrefab.SetActive(false);
    //}

    protected override void Attack()
    {

        transform.LookAt(player);

        if (!hasAttacked)
        {
            //SHOOOT
            Shoot();

            anim.SetBool("Walking", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Attacking", false);
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

    private void Shoot()
    {
        

    }

}
