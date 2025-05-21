using UnityEngine;
using UnityEngine.InputSystem;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Camera _camera;

    private Selectable _currentSelectable;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.GetTarget.performed += ctx => OnLeftButtonDown();
        _playerInput.Player.Select.performed += ctx => OnRightButtonDown();

    }

    private void OnDisable()
    {
        _playerInput.Player.GetTarget.performed -= ctx => OnLeftButtonDown();
        _playerInput.Player.Select.performed -= ctx => OnRightButtonDown();
        _playerInput.Disable();
    }

    private void OnLeftButtonDown()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, _layerMask))
        {
            if (_currentSelectable != null)
            {
                Vector3 intersectionPoint = hit.point;

                if(_currentSelectable.TryGetComponent(out FlagSeter flagSeter))
                {
                    flagSeter.SetPosition(intersectionPoint);
                }
            }
        }
    }

    private void OnRightButtonDown()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Selectable selectable = hit.collider.gameObject.GetComponent<Selectable>();

            if (selectable != null)
            {
                if (_currentSelectable && _currentSelectable != selectable)
                {
                    _currentSelectable.Deselect();
                }

                _currentSelectable = selectable;
                selectable.Select();
            }
            else
            {
                Deselect();
            }

        }
        else
        {
            Deselect();
        }
    }

    private void Deselect()
    {
        if (_currentSelectable)
        {
            _currentSelectable.Deselect();
            _currentSelectable = null;
        }
    }
}
