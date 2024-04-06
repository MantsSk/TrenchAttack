using UnityEngine;

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

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleRunning();
        HandleMouseLook();
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
}
