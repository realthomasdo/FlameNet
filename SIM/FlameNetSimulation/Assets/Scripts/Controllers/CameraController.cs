using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private InputActionReference movement, aiming, leftClick;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        leftClick.action.performed += LeftClickAction;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Aiming();
    }
    private void Movement()
    {
        Vector3 moveDir = movement.action.ReadValue<Vector3>().normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
    private void Aiming()
    {
        Vector2 mouseDelta = aiming.action.ReadValue<Vector2>();

        transform.localEulerAngles += new Vector3(-mouseDelta.y, mouseDelta.x, 0) * mouseSensitivity * Time.deltaTime;
    }
    private void LeftClickAction(InputAction.CallbackContext callBack)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.CompareTag("Beacon"))
            {
                hit.collider.GetComponent<BeaconController>().ToggleDisplay();
            }
        }
    }
}
