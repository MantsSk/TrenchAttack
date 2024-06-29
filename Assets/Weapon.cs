using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { SemiAuto, Auto }
    public WeaponType weaponType;
    public string weaponName;
    public int totalAmmo; // Total bullets available for the weapon
    public int maxAmmo; // Max bullet in single magazine
    public int currentAmmo;
    public float rpm; // Rounds per minute
    public float reloadTime;
    public Transform firePoint;
    public float range = 100f; // The range of the weapon
    public int damage = 30; // Damage dealt by the weapon
    private float fireRate; // Time between shots
    private float nextTimeToFire = 0f;
    public float adsFOV = 15f; // Field of view when aiming down sights
    public Vector3 adsPosition; // Position of the weapon when aiming down sights
    private Vector3 originalPosition; // Original position of the weapon
    private Camera mainCamera;
    private float originalFOV;
    public float adsSpeed = 5f; // Speed of transition to ADS
    private bool isAiming = false;
    public ParticleSystem muzzleFlash;
    public AudioClip shootingSound; // Add this line
    private AudioSource audioSource; // Add this line


    // Start is called before the first frame update
    void Start()
    {
        fireRate = 60f / rpm;
        currentAmmo = maxAmmo;

        mainCamera = Camera.main;
        originalFOV = mainCamera.fieldOfView;
        originalPosition = transform.localPosition;

        audioSource = GetComponent<AudioSource>(); // Initialize AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        Aim(isAiming);
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    public void Aim(bool isAiming)
    {
        float targetFOV = isAiming ? adsFOV : originalFOV;
        Vector3 targetPosition = isAiming ? adsPosition : originalPosition;

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * adsSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * adsSpeed);
    }


    public void Fire()
    {
        if (Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + fireRate; // Update next fire time based on fire rate
            Shoot();
        }
    }

    private void Shoot()
    {
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().DamageEnemy(damage);
            }
        }
        PlayMuzzleFlash();
        PlayShootingSound(); // Play the shooting sound
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    private void PlayShootingSound()
    {
        if (shootingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        int ammoNeeded = maxAmmo - currentAmmo;
        if (totalAmmo >= ammoNeeded)
        {
            currentAmmo = maxAmmo;
            totalAmmo -= ammoNeeded;
        }
        else
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
        }
    }
}
