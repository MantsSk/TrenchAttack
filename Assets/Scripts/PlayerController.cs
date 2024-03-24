using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    private CharacterController characterController;
    private Vector3 moveInput;
    public Transform camTrans;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
       moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
       moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; 

       characterController.Move(moveInput);

       Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

       transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

       camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}