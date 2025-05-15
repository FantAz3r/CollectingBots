using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraMoveSpeed = 10f;
    [SerializeField] private float _edgePanSize = 15f;
    [SerializeField] private Vector2 _panLimitMin;
    [SerializeField] private Vector2 _panLimitMax;

    private PlayerInput _playerInput;
    private Vector2 _mousePosition;
    private Vector3 _rawMove = Vector3.zero;
    private Vector3 _move = Vector3.zero;
    private float _angle = -45f * Mathf.Deg2Rad;

    private Coroutine _moveCoroutine;
    private bool _isMiddleMousePressed = false;
    private bool _isEdgePanActive = false;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.MousePosition.performed += OnMove; 
        _playerInput.Player.MousePosition.canceled += OnMove;

        _playerInput.Player.MiddleClick.started += ctx => OnMiddleMouseDown();
        _playerInput.Player.MiddleClick.canceled += ctx => OnMiddleMouseUp();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        _mousePosition = Mouse.current.position.ReadValue();

        bool edgePan = IsMouseNearEdge();

        if (edgePan && _isEdgePanActive == false)
        {
            _isEdgePanActive = true;
            StartMovementCoroutine();
        }
        else if (edgePan == false && _isEdgePanActive)
        {
            _isEdgePanActive = false;
            StopMovementCoroutineIfNeeded();
        }
    }

    private void OnMiddleMouseDown()
    {
        _isMiddleMousePressed = true;
        StartMovementCoroutine();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnMiddleMouseUp()
    {
        _isMiddleMousePressed = false;
        StopMovementCoroutineIfNeeded();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }

    private bool IsMouseNearEdge()
    {
        return (_mousePosition.x <= _edgePanSize
                || _mousePosition.x >= Screen.width - _edgePanSize
                || _mousePosition.y <= _edgePanSize
                || _mousePosition.y >= Screen.height - _edgePanSize);
    }

    private void StartMovementCoroutine()
    {
        if (_moveCoroutine == null)
        {
            _moveCoroutine = StartCoroutine(MoveCameraCoroutine());
        }
    }

    private void StopMovementCoroutineIfNeeded()
    {
        if (_isMiddleMousePressed == false && _isEdgePanActive == false)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
                _rawMove = Vector3.zero;
            }
        }
    }

    private IEnumerator MoveCameraCoroutine()
    {
        while (enabled)
        {
            float cos = Mathf.Cos(_angle);
            float sin = Mathf.Sin(_angle);

            if (_isMiddleMousePressed)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                _rawMove = new Vector3(-mouseDelta.x, 0f, -mouseDelta.y) * _cameraMoveSpeed * Time.deltaTime;
            }
            else
            {
                _rawMove = Vector3.zero;

                if (_mousePosition.x <= _edgePanSize)
                    _rawMove.x = -_cameraMoveSpeed * Time.deltaTime;
                else if (_mousePosition.x >= Screen.width - _edgePanSize)
                    _rawMove.x = _cameraMoveSpeed * Time.deltaTime;

                if (_mousePosition.y <= _edgePanSize)
                    _rawMove.z = -_cameraMoveSpeed * Time.deltaTime;
                else if (_mousePosition.y >= Screen.height - _edgePanSize)
                    _rawMove.z = _cameraMoveSpeed * Time.deltaTime;
            }

            _move.x = _rawMove.x * cos - _rawMove.z * sin;
            _move.z = _rawMove.x * sin + _rawMove.z * cos;

            Vector3 newPosition = transform.position + _move;
            newPosition.x = Mathf.Clamp(newPosition.x, _panLimitMin.x, _panLimitMax.x);
            newPosition.z = Mathf.Clamp(newPosition.z, _panLimitMin.y, _panLimitMax.y);
            transform.position = newPosition;

            yield return null;
        }
    }
}