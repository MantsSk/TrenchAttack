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
    public GameObject weapon; // Reference to the weapon
    public Transform firePoint; // The point from which the enemy shoots

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

        // Get the weapon's fire point and direction
        Vector3 direction = (PlayerController.instance.transform.position - firePoint.position).normalized;

        Debug.DrawRay(firePoint.position, direction * distanceToShoot, Color.red, 1.0f); // Draw the ray for 1 second

        // Debug.Log("Enemy shooting! Direction: " + direction);

        if (Random.value <= accuracy)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, direction, out hit, distanceToShoot))
            {
                Debug.Log("Raycast hit something!");

                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log("Raycast hit the player!");

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
                    Debug.Log("Raycast hit something other than the player: " + hit.transform.name);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
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
