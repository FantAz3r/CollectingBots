using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private PlayerInput _playerInput;


    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.GetTarget.performed += ctx => OnLeftButtonDown();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnLeftButtonDown()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, _layerMask ))
        {
            Vector3 intersectionPoint = hit.point;
            Debug.Log($"Пересечение в точке: {intersectionPoint}");
        }
    }
}
