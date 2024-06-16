using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Rigidbody rigidBody;


    private float lifeTime;
    private float lifeTimeCounter;

    protected void OnEnable()
    {
        lifeTimeCounter = 0;
    }

    protected void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        rigidBody.velocity = spawnPoint.forward * Time.deltaTime;
        transform.position = spawnPoint.position;
        lifeTime = 2;
    }
}
