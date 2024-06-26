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
    public int damage = 10; // Damage dealt to the player
    public float shootInterval = 1.5f; // Interval between shots
    public float accuracy = 0.75f; // Accuracy of the enemy (1.0 = 100% accurate, 0.0 = 0% accurate)
    private float lastShootTime;

    void Start()
    {
        startPoint = transform.position;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void DamageEnemy(int damage) {
        currentHealth -= damage;
        animator.SetTrigger("isHurt");
        Debug.Log("Current Health: " + currentHealth);
        
        if (currentHealth <= 0) {
            animator.SetTrigger("isDead");
            agent.isStopped = true;            
            // Destroy(gameObject);
        }
    }

    void Chase() 
    {
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
        animator.SetBool("isShooting", true);
        if (Time.time >= lastShootTime + shootInterval)
        {
            lastShootTime = Time.time;
            if (Random.value <= accuracy)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, distanceToShoot))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        PlayerHealth playerHealth = PlayerController.instance.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(damage);
                            Debug.Log("Enemy hit the player!");
                        }
                        else
                        {
                            Debug.Log("PlayerHealth component not found!");
                        }
                    }
                    else
                    {
                        Debug.Log("Enemy missed the player!");
                    }
                }
            }
            else
            {
                Debug.Log("Enemy missed due to accuracy!");
            }
        }
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
                animator.SetBool("isShooting", false);
                isShooting = false;
                agent.isStopped = false; // Resume chasing
            }
        }
    }
}
