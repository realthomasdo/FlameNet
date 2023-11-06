using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private InputActionReference movement, rotation, leftClick;
    [SerializeField] private GameObject camera;
    void Start()
    {
        leftClick.action.performed += LeftClickAction;
    }
    void Update()
    {
        if (Application.isFocused)
        {
            Movement();
            Rotation();
        }
    }
    private void Movement()
    {
        Vector3 moveDir = movement.action.ReadValue<Vector3>().normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
    private void Rotation()
    {
        transform.Rotate(0, rotation.action.ReadValue<float>() * mouseSensitivity, 0);
    }
    private void LeftClickAction(InputAction.CallbackContext callBack)
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, 100, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Beacon"))
            {
                hit.collider.GetComponent<BeaconController>().ToggleDisplay();
            }
        }
    }
}
