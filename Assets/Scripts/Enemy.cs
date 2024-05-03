using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 6;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 targetPoint, startPoint;
    private NavMeshAgent agent;
    int currentHealth = 100;
    private bool chasing = false;
    private bool isShooting = false;

    public float distanceToChase = 20; // Changed to 20 meters to detect player
    public float distanceToShoot = 5; // New variable for shooting range

    void Start()
    {
        startPoint = transform.position;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        animator.SetBool("isShooting", false);
        agent.SetDestination(targetPoint);

        if (Vector3.Distance(transform.position, targetPoint) > distanceToChase) {
            chasing = false;
        }
        else if (Vector3.Distance(transform.position, targetPoint) <= distanceToShoot) {
            isShooting = true;
        }
    }

    void Shoot() 
    {
        // Implement shooting logic here
        animator.SetBool("isShooting", true);
        Debug.Log("Shooting at player!");
    }

    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!isShooting) 
        {
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
        else 
        {
            // Stop chasing and start shooting
            agent.isStopped = true;
            Shoot();

            // Check if the player is out of shooting range
            if (Vector3.Distance(transform.position, targetPoint) > distanceToShoot) {
                isShooting = false;
                agent.isStopped = false; // Resume chasing
            }
        }
    }
}
