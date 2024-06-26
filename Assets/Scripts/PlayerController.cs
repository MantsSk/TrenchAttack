using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private CharacterController characterController;
    private Vector3 moveInput;
    private bool canJump = true;
    public bool invertX = false;
    public bool invertY = false;

    public float gravityModifier = 1.0f;
    public float jumpPower = 3.0f;
    public float runSpeed = 12.0f;
    public float mouseSensitivity = 2.0f;

    public List<Weapon> weapons;
    private int currentWeaponIndex;
    private Weapon currentWeapon;

    public static PlayerController instance;

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (weapons.Count > 0)
        {
            currentWeapon = weapons[0];
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleRunning();
        HandleMouseLook();
        HandleShooting();
        HandleWeaponSwitching();
        HandleAiming();
    }

    private void HandleMovement()
    {
        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = vertMove + horiMove;
        moveInput = moveInput.normalized * moveSpeed;
        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        characterController.Move(moveInput * Time.deltaTime);
    }

    private void HandleShooting()
    {
        if (currentWeapon != null)
        {
            if (currentWeapon.weaponType == Weapon.WeaponType.SemiAuto)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    currentWeapon.Fire();
                }
            }
            else if (currentWeapon.weaponType == Weapon.WeaponType.Auto)
            {
                if (Input.GetButton("Fire1"))
                {
                    currentWeapon.Fire();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentWeapon.Reload();
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            moveInput.y = jumpPower;
            canJump = false;
        }

        if (characterController.isGrounded)
        {
            canJump = true;
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }
    }

    private void HandleRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput.normalized * runSpeed;
        }
    }

    private void HandleMouseLook()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
    }

    private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    private void HandleAiming()
    {
        if (Input.GetButtonDown("Fire2")) // Right mouse button
        {
            currentWeapon.SetAiming(true);
        }
        if (Input.GetButtonUp("Fire2")) // Release right mouse button
        {
            currentWeapon.SetAiming(false);
        }
    }

    private void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        currentWeapon = weapons[currentWeaponIndex];
        ActivateWeapon(currentWeaponIndex);
    }

    private void ActivateWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }
    }
}
