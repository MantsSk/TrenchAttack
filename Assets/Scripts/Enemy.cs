using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 6;
    private Rigidbody rb;
    private Vector3 targetPoint, startPoint;
    private NavMeshAgent agent;
    int currentHealth = 100;
    private bool chasing = false;

    public float distanceToChase = 10;
    public float distanceToLose = 15;

    void Start()
    {
        startPoint = transform.position;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void DamageEnemy() {
        currentHealth -= 20;
        Debug.Log("Current Health: " + currentHealth);
        
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    void Chase() 
    {
        agent.SetDestination(targetPoint);

        if (Vector3.Distance(transform.position, targetPoint) > distanceToLose) {
            chasing = false;
        }
    }

    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing) 
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase) {
                chasing = true;
            }
        }
        else 
        {
            Chase();
        }
    }
}
